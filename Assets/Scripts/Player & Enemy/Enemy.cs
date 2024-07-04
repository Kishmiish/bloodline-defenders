using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float initialSpeed;
    [SerializeField] private float initialHealth;
    [SerializeField] private float InitialAttackDelay;
    [SerializeField] private float InitialAttackDamage;
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
    public void Damage(int damage)
    {
        health -= damage;
        setHealthBar();
        if (health <= 0)
        {
            manager.GetComponent<KillCounter>().Kill();
            animator.SetTrigger("Death");
            StartCoroutine(animationDelay());
        }
    }
    IEnumerator animationDelay()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
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
