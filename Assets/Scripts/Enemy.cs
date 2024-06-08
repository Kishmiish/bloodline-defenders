using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float health = 3f;
    [SerializeField] float attackDelay = 0.5f;
    [SerializeField] float attackDamage = 3f;
    private Slider healthBar;
    private GameObject manager;
    private GameManager gameManager;
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


    void FixedUpdate()
    {
        if(gameManager.isPlayerAlive)
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
        healthBar.value = health;
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
