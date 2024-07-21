using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [SerializeField] private float InitialSpeed;
    [SerializeField] private float initialDamage;
    private float speed;
    private float damage;
    Enemy closest;
    void Awake()
    {
        speed = InitialSpeed;
        damage = initialDamage;
        FindClosestEnemy();
    }
    void Update()
    {
        if(closest == null) { Destroy(gameObject); }
        transform.Translate((closest.gameObject.transform.position - transform.position).normalized * speed);
    }
    void FindClosestEnemy()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        if(enemies == null) { return; }
        closest = enemies[0];
        double dis = Vector3.Distance(transform.position, enemies[0].gameObject.transform.position);
        foreach (Enemy enemy in enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < dis)
            {
                dis = Vector3.Distance(transform.position, enemy.transform.position);
                closest = enemy;
            }
        }
    }
    public void LevelUP()
    {
        damage *= 1.05f;
        speed *= 1.05f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
