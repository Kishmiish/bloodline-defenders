using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float initialTimeToAttack;
    [SerializeField] GameObject rigthWeapon;
    [SerializeField] GameObject leftWeapon;
    [SerializeField] Vector2 powerOfAttack = new Vector2(4f, 2f);
    [SerializeField] int initialWeaponDamage;
    float timetoattack;
    int weaponDamage;
    SpriteRenderer spriteRenderer;
    Animator animator;
    private GameObject gameManager;
    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
        weaponDamage = initialWeaponDamage;
        timetoattack = initialTimeToAttack;
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void Start()
    {
        InvokeRepeating(nameof(AttackAnimation),timetoattack,timetoattack);
    }
    private void AttackAnimation(){
        animator.SetTrigger("Attack");
    }
    public void Attack() {
        if (spriteRenderer.flipX == false) {
            Collider2D[] colliders =  Physics2D.OverlapBoxAll(rigthWeapon.transform.position, powerOfAttack, 0f);
            if(colliders.Length > 0)
            {
                ApplyDamage(colliders);
            }
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(leftWeapon.transform.position, powerOfAttack, 0f);
            if(colliders.Length > 0)
            {
                ApplyDamage(colliders);
            }
        }
    }

    private void ApplyDamage(Collider2D[] colliders)
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Enemy>() != null)
            {
                if(colliders[i].GetComponent<Enemy>().Damage(weaponDamage))
                {
                    gameManager.GetComponent<KillCounter>().Kill();
                }
            }
        }
    }
}
