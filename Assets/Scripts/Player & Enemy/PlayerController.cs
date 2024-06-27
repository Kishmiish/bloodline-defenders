using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Slider healthBar;
    float health = 100;

    [HideInInspector]
    Animator animator;

    [HideInInspector]
    PlayerMovement playerMovement;

    [HideInInspector]
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (playerMovement.moveDirection.x != 0 || playerMovement.moveDirection.y != 0) {
            animator.SetBool("Move", true);
            SpriteDirectionChecker();
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
    void SpriteDirectionChecker()
    {
        if (playerMovement.lastHorizontalVector < 0) {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void SetHealthBar()
    {
        healthBar.value = health;
    }
    public void receiveDamage(float damage){
        if(damage >= health){
            CancelInvoke();
            Death();
        }
        else{
            health -= damage;
            if(health > 0)
            {
                animator.SetTrigger("Hit");
            }
            SetHealthBar();
        }
    }

    void Death(){
        if(health > 0){
            animator.SetTrigger("DeadTrigger");
            animator.SetBool("DeadBoolean", true);
        }
        health = 0;
        SetHealthBar();
        tag = "Dead";
        gameManager.GameOver();
        gameObject.GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<WeaponController>().CancelInvoke();
        GameObject.Find("Weapon").SetActive(false);
    }
}
