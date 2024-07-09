using System;
using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private float initialSpeed;
    [SerializeField] private float initialHealth;
    [SerializeField] private float InitialAttackDelay;
    [SerializeField] private float InitialAttackDamage;
    [SerializeField] private GameObject[] enemyDrop;
    private float speed;
    private float health;
    private float attackDelay;
    private float attackDamage;
    private Slider healthBar;
    private GameObject manager;
    private GameManager gameManager;
    private GameObject[] players;
    private GameObject Player;
    private Rigidbody2D rigidBody2D;
    private Vector3 currentPosition;
    private Animator animator;
    public NetworkObject obejctToDestroy;
    void Awake()
    {
        ResetValues();
        Player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("GameController");
        gameManager = manager.GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        healthBar.maxValue = health;
        setHealthBar();
    }
    void ResetValues(){
        health = initialHealth;
        speed = initialSpeed;
        attackDamage = InitialAttackDamage;
        attackDelay = InitialAttackDelay;
    }

    bool CheckForClosestEnemy()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players == null || players.Length == 0) { return false; }
        Player = players[0];
        foreach (GameObject temp in players)
        {
            if(Vector3.Distance(temp.transform.position, gameObject.transform.position) < Vector3.Distance(Player.transform.position, gameObject.transform.position))
            {
                Player = temp;
            }
        }
        return true;
    }
    void FixedUpdate()
    {
        if(CheckForClosestEnemy())
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            rigidBody2D.velocity = direction * speed;      
        }
        else{
            Freeze();
        }
    }

    void Attack()
    {
        PlayerController playerController = Player.GetComponent<PlayerController>();
        playerController.receiveDamage(attackDamage);
    }
    public bool Damage(float damage)
    {
        health -= damage;
        setHealthBar();
        if (health <= 0)
        {
            DropRandomItem();
            Freeze();
            attackDamage = 0;
            animator.SetTrigger("Death");
            obejctToDestroy = this.NetworkObject;
            StartCoroutine(animationDelay());
            return true;
        }
        return false;
    }
    IEnumerator animationDelay()
    {
        yield return new WaitForSeconds(0.7f);
        if(IsOwner){
            DestroyObjectServerRpc();
        }
    }
    [ServerRpc]
    public void DestroyObjectServerRpc()
    {
        if(IsServer)
        {
            if(obejctToDestroy != null)
            {
                obejctToDestroy.Despawn();
                Destroy(obejctToDestroy.gameObject);
            }
        }
    }
    public void Freeze(){
        rigidBody2D.velocity = Vector2.zero;
        CancelInvoke();
    }
    void setHealthBar()
    {
        if(healthBar == null){
            healthBar = GetComponentInChildren<Slider>();
        }
        healthBar.value = health;
    }
    public void IncreaseDifficulty()
    {
        health *= 1.05f;
        setHealthBar();
        attackDamage *= 1.05f;
    }
    void DropRandomItem()
    {
        float chance = UnityEngine.Random.Range(0, 1f);
         if (0f < chance && chance < 0.018f)//many coins
        {
            Instantiate(enemyDrop[3], new Vector3(transform.position.x, transform.position.y, -2), quaternion.identity);
        }
       else if (0.0181f < chance && chance < 0.079f)
        {
            Instantiate(enemyDrop[0], new Vector3(transform.position.x, transform.position.y, -2), quaternion.identity);
        } else if (0.079f < chance && chance < 0.27f)
        {
            Instantiate(enemyDrop[1], new Vector3(transform.position.x, transform.position.y, -2), quaternion.identity);
        }
        else if (0.27f < chance && chance < 0.47f)//silvercoin
        {
            Instantiate(enemyDrop[2], new Vector3(transform.position.x, transform.position.y, -2), quaternion.identity);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == Player){
            InvokeRepeating(nameof(Attack),0,attackDelay);
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject == Player){
            CancelInvoke();
        }
    }
}
