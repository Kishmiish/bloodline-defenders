using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float timetoattack = 3f;
    [SerializeField] GameObject rigthWeapon;
    [SerializeField] GameObject leftWeapon;
    [SerializeField] Vector2 powerOfAttack = new Vector2(4f, 2f);
    [SerializeField] int weaponDamage = 1;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(Attack),timetoattack,timetoattack);
        InvokeRepeating(nameof(Clear),timetoattack + 0.3f, timetoattack + 0.3f);
    }
    private void Update()
    {
        
    }
    private void Attack() {
        if (spriteRenderer.flipX == false) {
            rigthWeapon.SetActive(true);
            Collider2D[] colliders =  Physics2D.OverlapBoxAll(rigthWeapon.transform.position, powerOfAttack, 0f);
            ApplyDamage(colliders);
        }
        else
        {
            leftWeapon.SetActive(true);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(leftWeapon.transform.position, powerOfAttack, 0f);
            ApplyDamage(colliders);
        }
    }

    private void ApplyDamage(Collider2D[] colliders)
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Enemy>() != null)
            {
                colliders[i].GetComponent<Enemy>().Damage(weaponDamage);
            }
        }
    }

    void Clear()
    {
        rigthWeapon.SetActive(false);
        leftWeapon.SetActive(false);
    }

}
