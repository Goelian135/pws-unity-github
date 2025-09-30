using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header ("References")]
    public PlayerHealth health;
    public PlayerMovement movement;
    public BloodRythmBar bloodRythmBar;
    public Rigidbody2D rb;

    [Header ("Jump variables")]
    [SerializeField] private float jumpForce = 40.0f;
    public float gravityScale = 3.0f;
    public float jumpGravityScale = 2.0f;
    public float fallGravityScale = 5.0f;
    public float apexGravityScale = 1.5f;
    public float apexThresehold = 0.2f;

    [Header("")]
    // How much to smooth out the movement
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;

    [Header("Groundcheck")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    const float groundedRadius = .2f;
    public bool isGrounded;

    //iets voor movement smoothness
    private Vector3 velocity = Vector3.zero;

    private int facing = 1; //1 for right, -1 for left

    [Header ("Dash variables")]
    [SerializeField] private float dashSpeed = 20f;    
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    public bool isDashing = false;
    private bool canDash = true;
    private float dashTimeStart;

    public event Action onJump;
    public event Action onDash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (health == null)
            health = GetComponent<PlayerHealth>();
        if (movement == null)
            movement = GetComponent<PlayerMovement>();
        if (bloodRythmBar == null)
            bloodRythmBar = GetComponent<BloodRythmBar>();
    }


    private void Update()
    {
        //gravity for jump
        Gravity();

        //reset dash
        if (!canDash && !isDashing && Time.time > dashTimeStart + dashDuration + dashCooldown)
        {
            canDash = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }

    public void Move(float move, bool jump, bool dash)
    {
        //move the character
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        //smooth the movement
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        //if input is making the player move left and the player is facing right
        if (move < 0 && facing > 0)
        {
            Flip();
        }
        //if input is making the player move right and the player is facing left
        else if (move > 0 && facing < 0)
        {
            Flip();
        }

        //if the player should jump
        if (jump && isGrounded)
        {
            //add a vertical force to the player
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            onJump?.Invoke();
        }

        //check if the player should dash
        if (dash && !isDashing && canDash)
        {
            dashTimeStart = Time.time;            
            isDashing = true;
            canDash = false;
            
            onDash?.Invoke();
        }

        //dash movement
        if (isDashing)
        {
            if (Time.time < dashTimeStart + dashDuration)
            {
                rb.velocity = new Vector2(facing * dashSpeed, 0);
            }
            else
            {
                isDashing = false;
            }
        }
    }

    private void Flip()
    {
        //switch the way the player is labelled as facing
        facing *= -1;

        //multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Gravity()
    ///Change the gravity scale based on whether the player is going up, down or at the apex of their jump
    {
        //Gravity change based on apex, makes jumping feel better
        if (rb.velocity.y > apexThresehold) //omhoog
        {
            rb.gravityScale = InputManager.Instance.GetKey("Jump") ? jumpGravityScale : gravityScale;
        }
        else if (rb.velocity.y < -apexThresehold) //omlaag
        {
            rb.gravityScale = fallGravityScale;
        }
        else //apex
        {
            rb.gravityScale = apexGravityScale;
        }
    }

}