using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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
    public void receiveDamage(float damage){
        if(damage >= health){
            Death();
        }
        else{
            health -= damage;
        }
    }
    void Death(){
        if(health > 0){
            animator.SetTrigger("Dead");
            Debug.Log("Trigger");
        }
        health = 0;
        gameManager.GameOver();
        gameObject.GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<WeaponController>().CancelInvoke();
        GameObject.Find("Weapon").SetActive(false);
    }
}
