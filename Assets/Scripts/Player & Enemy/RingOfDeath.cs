using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class RingOfDeath : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private float initialDamage;
    [SerializeField] private float initialRange;
    [SerializeField] private CircleCollider2D circleCollider;
    private float damage;
    private float range;
    List<Enemy> enemiesInRange;
    void Awake()
    {
        enemiesInRange = new List<Enemy>();
        damage = initialDamage;
        range = initialRange;
    }
    void Start()
    {
        InvokeRepeating(nameof(DamageEnemies), 0, cooldown);
        transform.localScale = new Vector3(range, range, range);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Add(collision.gameObject.GetComponent<Enemy>());
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(enemiesInRange.Contains(collision.gameObject.GetComponent<Enemy>()))
        {
            enemiesInRange.Remove(collision.gameObject.GetComponent<Enemy>());
        }
    }
    void DamageEnemies()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.Damage(damage);
        }
    }
    public void LevelUP()
    {
        range += 0.2f;
        damage *= 1.05f;
        transform.localScale = new Vector3(range, range, range);
    }
}
