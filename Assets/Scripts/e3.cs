using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class e3 : MonoBehaviour
{
    [SerializeField] float speed = 3.43f;
    [SerializeField] float health = 3.43f;
    [SerializeField] float attackDelay = 0.22f;
    [SerializeField] float attackDamage = 3.52f;
    private Slider healthBar;
    private GameObject manager;
    private GameManager gameManager;
    GameObject[] players;
    GameObject Player;
    Rigidbody2D rigidBody2D;
    Vector3 currentPosition;
    Animator animator;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("GameController");
        gameManager = manager.GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
    }
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    bool CheckForClosestEnemy()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null) { return false; }
        Player = players[0];
        foreach (GameObject temp in players)
        {
            if (Vector3.Distance(temp.transform.position, gameObject.transform.position) < Vector3.Distance(Player.transform.position, gameObject.transform.position))
            {
                Player = temp;
            }
        }
        return true;
    }
    void FixedUpdate()
    {
        if (CheckForClosestEnemy())
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            rigidBody2D.velocity = direction * speed;
        }
        else
        {
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
    public void Freeze()
    {
        rigidBody2D.velocity = Vector2.zero;
        CancelInvoke();
    }
    void setHealthBar()
    {
        healthBar.value = health;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == Player)
        {
            InvokeRepeating(nameof(Attack), 0, attackDelay);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == Player)
        {
            CancelInvoke();
        }
    }
}
