using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float initialTimeToAttack;
    [SerializeField] GameObject rigthWeapon;
    [SerializeField] GameObject leftWeapon;
    [SerializeField] Vector2 AttackRange = new Vector2(4f, 2f);
    [SerializeField] float initialWeaponDamage;
    float timetoattack;
    float weaponDamage;
    SpriteRenderer spriteRenderer;
    Animator animator;
    private GameObject gameManager;

    public float GetInitialTimeToAttack()
    {
        return initialTimeToAttack;
    }
    public float GetInitialWeaponDamage()
    {
        return initialWeaponDamage;
    }
    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
        weaponDamage = initialWeaponDamage;
        timetoattack = initialTimeToAttack;
        InitializeValue();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void Start()
    {
        InvokeRepeating(nameof(AttackAnimation),timetoattack,timetoattack);
    }
    void Update()
    {
        if(spriteRenderer == null) { spriteRenderer = GetComponentInParent<SpriteRenderer>(); }
        if(animator == null) { animator = GetComponentInParent<Animator>(); }
        if(gameManager == null) { gameManager = GameObject.FindGameObjectWithTag("GameController"); }
    }
    private void AttackAnimation()
    {

        animator.SetTrigger("Attack");
    }
    public void Attack() {
        if (spriteRenderer.flipX == false) {
            Collider2D[] colliders =  Physics2D.OverlapBoxAll(rigthWeapon.transform.position, AttackRange, 0f);
            if(colliders.Length > 0)
            {
                ApplyDamage(colliders);
            }
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(leftWeapon.transform.position, AttackRange, 0f);
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

    public void LevelUp()
    {
        weaponDamage++;
    }

    void InitializeValue()
    {
        int damageLevel = PlayerPrefs.GetInt("WeaponDamageLeve");
        int cooldownLevel = PlayerPrefs.GetInt("WeaponCooldownLevel");
        for (int i = 0; i < damageLevel; i++)
        {
            weaponDamage *= 1.1f;
        }
        for (int i = 0; i < cooldownLevel; i++)
        {
            timetoattack *= 0.95f;
        }
    }
}
