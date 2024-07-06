using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Slider healthBar;
    float maxHealth = 100;
    float health;
    int XP;
    int nextLevelXP;

    [HideInInspector]
    Animator animator;

    [HideInInspector]
    PlayerMovement playerMovement;

    [HideInInspector]
    SpriteRenderer spriteRenderer;
    private WeaponController weaponController;
    private Slider XPBar;
    private TMP_Text coinCounter;
    
    void Start()
    {
        health = maxHealth;
        SetMaxHealthBar();
        XP = 0;
        nextLevelXP = 5;
        coinCounter = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<TMP_Text>();
        SetCoin();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponController = GetComponentInChildren<WeaponController>();
        XPBar = GameObject.FindGameObjectWithTag("XPBar").GetComponent<Slider>();
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

    void SetMaxHealthBar()
    {
        healthBar.maxValue = maxHealth;
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

    public void DealDamage()
    {
        weaponController.Attack();
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "XP")
        {
            XP++;
            XPBar.value = XP;
            if(XP == nextLevelXP)
            {
                LevelUp();
            }
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "HP")
        {
            health++;
            SetHealthBar();
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "Coin")
        {
            PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin") + 1);
            Destroy(collision.gameObject);
            SetCoin();
        }
    }
    void LevelUp()
    {
        maxHealth *= 1.1f;
        SetMaxHealthBar();
        XP = 0;
        XPBar.value = XP;
        nextLevelXP = nextLevelXP * 3 / 2;
        XPBar.maxValue = nextLevelXP;
    }

    void SetCoin()
    {
        coinCounter.text = ": " + PlayerPrefs.GetInt("Coin");
    }
}
