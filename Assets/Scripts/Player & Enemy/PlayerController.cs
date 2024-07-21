using System.Globalization;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Slider healthBar;
    [SerializeField] private AudioSource weaponEffect;
    [SerializeField] private GameObject ring;
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
    private GameObject upgradeMenu;
    
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    void Start()
    {
        health = maxHealth;
        SetMaxHealthBar();
        SetHealthBar();
        XP = 0;
        nextLevelXP = 5;
        coinCounter = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<TMP_Text>();
        SetCoin();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponController = GetComponentInChildren<WeaponController>();
        XPBar = GameObject.FindGameObjectWithTag("XPBar").GetComponent<Slider>();
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
    }

    
    void Update()
    {
        if(animator == null) { animator = GetComponent<Animator>(); }
        if(playerMovement == null) { playerMovement = GetComponent<PlayerMovement>(); }
        if(spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        if(weaponController == null) { weaponController = GetComponentInChildren<WeaponController>(); }
        if(coinCounter == null) { coinCounter = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<TMP_Text>(); }
        if(XPBar == null) { XPBar = GameObject.FindGameObjectWithTag("XPBar").GetComponent<Slider>(); }
        if(gameManager == null) { gameManager = GameObject.FindGameObjectWithTag("GameController"); }
        if(upgradeMenu == null) { upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu"); }
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
        if(IsOwner){
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
    }

    public void DealDamage()
    {
        if(IsOwner){
            weaponEffect.Play();
            weaponController.Attack();
        }
    }

    void Death(){
        if(IsOwner){
            if(health > 0){
                animator.SetTrigger("DeadTrigger");
                animator.SetBool("DeadBoolean", true);
            }
            health = 0;
            SetHealthBar();
            tag = "Dead";
            GetComponent<MagicCasting>().CancelInvoke();
            gameManager.GetComponent<GameManager>().GameOver();
            gameObject.GetComponent<Collider2D>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponentInChildren<WeaponController>().CancelInvoke();
            GameObject.Find("Weapon").SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "silvercoin")
            {
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 1);
                health += 5;
                SetHealthBar();
                Destroy(collision.gameObject);
                SetCoin();
            }
            else if (collision.gameObject.tag == "manycoins")
            {
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 10);
                
                Destroy(collision.gameObject);
                SetCoin();
            }
        
    }
    public void IncreaseXP()
    {
        if(IsOwner){    
            XP++;
            XPBar.value = XP;
            if(XP == nextLevelXP)
            {
                LevelUp();
            }
        }
    }
    public void IncreaseHealth()
    {
        if(IsOwner)
        {
            health += 10;
            SetHealthBar();
        }
    }
    public void IncreaseCoin(int value)
    {
        if(IsOwner){
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + value);
            SetCoin();
        }
    }
    void LevelUp()
    {
        gameManager.GetComponent<GameManager>().UpgradeMenu();
        nextLevelXP = nextLevelXP * 3 / 2;
        XP = 0;
        XPBar.maxValue = nextLevelXP;
        XPBar.value = XP;
    }

    void SetCoin()
    {
        if(IsOwner){
            if(coinCounter == null) { coinCounter = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<TMP_Text>(); }
            coinCounter.text = ": " + PlayerPrefs.GetInt("Coin");
        }
    }

    public void EnableRing(bool condition)
    {
        ring.SetActive(condition);
    }

    public void LevelUpRing()
    {
        ring.GetComponent<RingOfDeath>().LevelUP();
    }
}
