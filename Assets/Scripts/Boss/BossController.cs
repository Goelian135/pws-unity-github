using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    public AttackHitbox attackHitbox;

    [Header("Boss Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    public bool isDead { get; private set; }

    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public int attackDamage = 20;
    public float moveSpeed = 2f;

    private bool canAttack = true;
    private int faceDirection = 1; // 1 for right, -1 for left
    private int attack;

    void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isDead)
        {

            //flip boss to face player
            if (player.position.x > transform.position.x && faceDirection < 0)
                Flip();
            if (player.position.x < transform.position.x && faceDirection > 0)
                Flip();

            if (distanceToPlayer > attackRange)
            {
                animator.SetBool("isRunning", true);
                // Move towards player
                MoveTowardsPlayer();
            }
            else
            {
                animator.SetBool("isRunning", false);
                rb.velocity = new Vector2(0, rb.velocity.y);

                // Attack player
                if (canAttack)
                {
                    attack = Random.Range(1, 3); // Randomly choose between 1 and 2
                    rb.velocity = Vector2.zero;
                    if (attack == 1)
                        animator.SetTrigger("Attack01");
                    else if (attack == 2)
                        animator.SetTrigger("Attack02");

                    canAttack = false;
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        faceDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        rb.velocity = Vector2.zero;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
        // Disable further interactions
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // Animation Events

    public void EnableAttackHitbox()
    {
        attackHitbox.EnableHitbox();
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.DisableHitbox();
    }

    public void ResetAttack()
    {
        canAttack = true;
    }
}


