using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed;
    
    [HideInInspector]
    public float lastHorizontalVector;

    [HideInInspector]
    public float lastVerticalVector;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeSpeed();
    }

    void Update()
    {
        if(IsOwner)
        {
            InputManegement();  
        }
    }
    void FixedUpdate()
    {
        if(IsOwner)
        {
            Move();
        }
    }

    void InputManegement()
    {
        float mx = Input.GetAxisRaw("Horizontal");
        float my = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(mx, my).normalized;

        if (moveDirection.x != 0) { lastHorizontalVector = moveDirection.x; }
        if (moveDirection.y != 0) { lastVerticalVector = moveDirection.y; }
    }

    void Move()
    {
        transform.Translate(moveDirection.x * speed, moveDirection.y * speed, 0);
        //rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
    void InitializeSpeed()
    {
        int level = PlayerPrefs.GetInt("PlayerSpeedLevel");
        for (int i = 0; i < level; i++)
        {
            speed *= 1.1f;
        }
    }
}