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

    [Header ("Jump assist")]
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    private float coyoteTimer;
    private float jumpBufferTimer;

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

    public int facing = 1; //1 for right, -1 for left

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
        //check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        //coyote timer
        if (isGrounded) {
            coyoteTimer = coyoteTime; //reset coyote timer
        } else {
            coyoteTimer -= Time.deltaTime;
        }

        //jump buffer timer
        if (InputManager.Instance.GetKeyDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime; //reset jump buffer timer
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        //gravity for jump
        Gravity();

        //reset dash
        if (!canDash && !isDashing && Time.time > dashTimeStart + dashDuration + dashCooldown)
        {
            canDash = true;
        }
    }

    public void Move(float move, bool jump, bool dash)
    {
        //no movement control while dashing
        if (isDashing)
        {
            HandleDashMovement();
            return;
        }

        //movement
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        //flip the character's facing direction
        if (move < 0 && facing > 0) Flip();
        else if (move > 0 && facing < 0) Flip();

        //jump
        bool canJump = (coyoteTimer > 0) && (jumpBufferTimer > 0);

        if (canJump)
        {
            coyoteTimer = 0;
            jumpBufferTimer = 0;

            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            onJump?.Invoke();
        }

        //Dash
        if (dash && !isDashing && canDash)
        {
            dashTimeStart = Time.time;
            isDashing = true;
            canDash = false;

            rb.gravityScale = 0; //disable gravity during dash

            onDash?.Invoke();
        }
    }

    private void HandleDashMovement()
    {
        if (Time.time < dashTimeStart + dashDuration)
            {
                rb.velocity = new Vector2(facing * dashSpeed, 0);
            }
        else
        {
            isDashing = false;

            rb.gravityScale = gravityScale; //re-enable gravity after dash
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
        float yVel = rb.velocity.y;

        if(isDashing) return; //no gravity change while dashing

        if (yVel > apexThresehold) //omhoog
        {
            rb.gravityScale = InputManager.Instance.GetKey("Jump") ? jumpGravityScale : gravityScale;
        }
        else if (yVel < -apexThresehold) //omlaag
        {
            rb.gravityScale = fallGravityScale;
        }
        else //apex
        {
            rb.gravityScale = apexGravityScale;
        }
    }

}