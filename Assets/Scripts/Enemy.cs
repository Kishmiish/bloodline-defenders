using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float health = 3f;
    [SerializeField] float attackDelay = 0.5f;
    [SerializeField] float attackDamage = 3f;
    GameObject Player;
    Rigidbody2D rigidBody2D;
    Vector3 currentPosition;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
        {
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        
        rigidBody2D.velocity = direction * speed;      
    }
    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.gameObject == Player)
    //     {
    //         Attack();
    //     }
    // }
    void Attack()
    {
        PlayerController playerController = Player.GetComponent<PlayerController>();
        playerController.receiveDamage(attackDamage);
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0) { Destroy(gameObject); }
    }
    public void Freeze(){
        CancelInvoke();
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
