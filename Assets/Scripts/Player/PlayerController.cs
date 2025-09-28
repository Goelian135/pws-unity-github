using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform player;
    public Animator animator;

    //variables
    public float moveSpeed = 5.0f;
    public float horizontalInput;
    private float lastFacingDir;
    public PlayerController playerController;
    public PlayerHealth playerHealth;

    //jump stuff
    public float jumpForce = 5.0f;
    public float gravityScale = 3.0f; 
    public float jumpGravityScale = 2.0f;
    public float fallGravityScale = 5.0f;
    public float apexGravityScale = 1.5f;
    public float apexThresehold = 0.2f;

    //rigidbody component
    public Rigidbody2D rb;

    //ground stuff
    private bool isGrounded;
    public Transform groundCheckPosition;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    //coyote+buffer time
    [SerializeField] float jumpCoyoteTimeMax = 0.1f;
    [SerializeField] float jumpBufferMax = 0.15f;
    private float jumpCoyoteTime;
    private float jumpBufferTime;

    //dash stuff
    public float dashSpeed = 10.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    private bool isDashing;
    private bool canDash;
    private float dashTime;
    private float dashTimeStart;

    //blood rythm stuff
    public int maxBloodRythm = 100;
    public float currentBloodRythm;
    public float bloodRythmDecreaseRate = 5.0f; // Amount of blood rhythm to decrease per second
    public float waitTimeBeforeDecrease = 2.0f; // Time to wait before starting to decrease blood rhythm
    public BloodRythmBar bloodRythmBar;

    // Start is called before the first frame update
    void Start()
    {
        //de rigidbody van player ophalen
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       if (lastFacingDir > 0)
        {
            player.transform.localScale = new Vector3(10, 10, 1);
        } 
        else if (lastFacingDir < 0)
        {
            player.transform.localScale = new Vector3(-10, 10, 1);
        }

        //countdowns verlagen
        jumpCoyoteTime -= Time.deltaTime;
        jumpBufferTime -= Time.deltaTime;

        //Input ophalen
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            lastFacingDir = horizontalInput; // Update the last facing direction
        }   

        //controleren op gromd
        CheckIfgrounded();

        //coyotetime reset
        if (isGrounded)
        {
            jumpCoyoteTime = jumpCoyoteTimeMax;
        }

        //buffer time reset
        if (InputManager.Instance.GetKeyDown("Jump"))
        {
            jumpBufferTime = jumpBufferMax;
        }

        if ((isGrounded || jumpCoyoteTime > 0) && jumpBufferTime > 0) 
        {
            Jump();
            jumpBufferTime = 0;
        }

        if (!canDash && !isDashing && isGrounded)
        {
            canDash = true;
        }

        //dash check
        if (InputManager.Instance.GetKeyDown("Dash") && !isDashing && canDash)
        {
            StartDash();
            animator.SetBool("isDashing", true);
        }

        //gravity aanpassen
        if (rb.velocity.y > apexThresehold) //omhoog
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("upwards", true);
            if (!InputManager.Instance.GetKey("Jump")) //meteen stoppen met springen
            {
                rb.gravityScale = fallGravityScale; 
            }
            else
            {
                rb.gravityScale = jumpGravityScale; 
            }
        }
        else if (Mathf.Abs(rb.velocity.y) <= apexThresehold) //apex
        {
            rb.gravityScale = apexGravityScale;
        }
        else if (rb.velocity.y < -apexThresehold) //omlaag
        {
            rb.gravityScale = fallGravityScale;
            animator.SetBool("upwards", false);
        }

        //blood rythm verhogen
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseBloodRythm(10); // Verhoog de bloedritme met 10 bij elke sprong
        }
        //blood rythm verlagen
        if (waitTimeBeforeDecrease > 0)
        {
            waitTimeBeforeDecrease -= Time.deltaTime; // Verminder de wachttijd
        }
        else
        {
            waitTimeBeforeDecrease = 0;
        }
        if (waitTimeBeforeDecrease == 0)
        {
            DecreaseBloodRythm(bloodRythmDecreaseRate * Time.deltaTime); // Verlaag de bloedritme continu
        }

        //heal met H
        if (InputManager.Instance.GetKeyDown("Heal") && currentBloodRythm >= 20 && !(playerHealth.currentHealth>=playerHealth.maxHealth))
        {
            Heal(1);
            DecreaseBloodRythm(20);
        }
    }

    void FixedUpdate()
    {
        //Beweging
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        if (horizontalInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        //dash movement
        if (isDashing)
        {
            if (Time.time < dashTimeStart + dashDuration)
            {
                rb.velocity = new Vector2(lastFacingDir * dashSpeed, rb.velocity.y);
            }
            else
            {
                isDashing = false;
                animator.SetBool("isDashing", false);
                animator.SetBool("canDash", true);
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void CheckIfgrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("upwards", false);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeStart = Time.time;
        canDash = false;
    }

    public void IncreaseBloodRythm(int amount)
    {
        currentBloodRythm += amount;
        if (currentBloodRythm > maxBloodRythm)
        {
            currentBloodRythm = maxBloodRythm;
        }
        bloodRythmBar.SetBloodRythm(currentBloodRythm);
        waitTimeBeforeDecrease = 2.0f; // Reset the wait time before decreasing blood rhythm
    }

    public void DecreaseBloodRythm(float amount)
    {
        currentBloodRythm -= amount;
        if (currentBloodRythm < 0)
        {
            currentBloodRythm = 0;
        }
        bloodRythmBar.SetBloodRythm(currentBloodRythm);
    }

    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
    }
}