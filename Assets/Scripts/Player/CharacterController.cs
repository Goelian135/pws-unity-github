using UnityEngine;
using System;

[System.Serializable]
public struct MoveInput
{
    public float horizontal;   // -1 t/m 1
    public bool jumpPressed;   // edge press
    public bool jumpHeld;      // held down
    public bool dashPressed;   // edge press
}

public class CharacterController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundMask;

    public event Action onJump;
    public event Action onDash;

    // ---------------------------------------------------------
    // Movement settings
    // ---------------------------------------------------------

    [Header("Movement")]
    public float moveSpeed = 12f;
    public float acceleration = 20f;
    public float deceleration = 30f;

    [Header("Jump")]
    public float jumpForce = 22f;
    public float apexThreshold = 0.2f;

    [Header("Gravity")]
    public float normalGravity = 3f;
    public float jumpGravity = 2f;
    public float fallGravity = 5f;
    public float apexGravity = 1.2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer = 0f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferTimer = 0f;

    [Header("Dash")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;
    public bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;

    // ---------------------------------------------------------

    private float targetVelocityX = 0f;
    private int facing = 1;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    // ---------------------------------------------------------
    // PUBLIC UPDATE CALLED BY PlayerMovement / InputManager
    // ---------------------------------------------------------

    public void ApplyInput(MoveInput input)
    {
        HandleTimers();
        GroundCheck();

        // Jump buffer & coyote
        if (input.jumpPressed)
            jumpBufferTimer = jumpBufferTime;

        bool doJump = jumpBufferTimer > 0 && coyoteTimer > 0;

        // Dash
        if (input.dashPressed && canDash && !isDashing)
            StartDash();

        if (isDashing)
        {
            HandleDash();
            return; // no normal movement while dashing
        }

        // Movement
        HandleMovement(input.horizontal);

        // Jumping
        if (doJump)
        {
            jumpBufferTimer = 0;
            coyoteTimer = 0;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            onJump?.Invoke();
        }

        HandleGravity(input.jumpHeld);
    }

    // ---------------------------------------------------------

    void HandleMovement(float horizontal)
    {
        // Flip direction
        if (horizontal > 0.1f && facing < 0) Flip();
        if (horizontal < -0.1f && facing > 0) Flip();

        // Smooth accel / decel
        if (Mathf.Abs(horizontal) > 0.05f)
        {
            targetVelocityX = Mathf.MoveTowards(rb.velocity.x, horizontal * moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            targetVelocityX = Mathf.MoveTowards(rb.velocity.x, 0f, deceleration * Time.deltaTime);
        }

        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
    }

    // ---------------------------------------------------------

    void HandleGravity(bool jumpHeld)
    {
        if (isDashing) return;

        float y = rb.velocity.y;

        if (y > apexThreshold)
            rb.gravityScale = jumpHeld ? jumpGravity : normalGravity;
        else if (y < -apexThreshold)
            rb.gravityScale = fallGravity;
        else
            rb.gravityScale = apexGravity;
    }

    // ---------------------------------------------------------

    void HandleTimers()
    {
        if (!isGrounded)
            coyoteTimer -= Time.deltaTime;
        else
            coyoteTimer = coyoteTime;

        if (jumpBufferTimer > 0)
            jumpBufferTimer -= Time.deltaTime;

        if (!canDash)
            dashTimer -= Time.deltaTime;

        if (dashTimer <= 0 && !isDashing)
            canDash = true;
    }

    // ---------------------------------------------------------

    public bool isGrounded = false;

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundMask);
    }

    // ---------------------------------------------------------

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimer = dashCooldown;

        rb.gravityScale = 0;
        rb.velocity = new Vector2(facing * dashSpeed, 0);

        onDash?.Invoke();

        Invoke(nameof(StopDash), dashDuration);
    }

    void HandleDash()
    {
        rb.velocity = new Vector2(facing * dashSpeed, 0);
    }

    void StopDash()
    {
        isDashing = false;
        rb.gravityScale = normalGravity;
    }

    // ---------------------------------------------------------

    void Flip()
    {
        facing *= -1;

        // ⚠️ Later: put sprite in a child and flip only that child
        transform.localScale = new Vector3(facing, 1, 1);
    }

    // ---------------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.15f);
        }
    }
}
