using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    bool facingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 25;
    public LayerMask enemyLayers;

    // internals
    Rigidbody2D rb;
    bool isGrounded = false;
    float horizInput;
    bool controlEnabled = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        var qm = FindFirstObjectByType<QuizManager>();
        if (qm != null)
        {
            qm.OnQuizStarted += () => SetControlEnabled(false);
            qm.OnQuizEnded += (s) => SetControlEnabled(true);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void Update()
    {
        horizInput = controlEnabled ? Input.GetAxisRaw("Horizontal") : 0f;
        if (horizInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizInput < 0 && facingRight)
        {
            Flip();
        }

        if (controlEnabled && Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();
    }

    void FixedUpdate()
    {
        // ground check here
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // apply horizontal movement
        Vector2 v = rb.linearVelocity;
        v.x = horizInput * moveSpeed;
        rb.linearVelocity = v;
    }

    void Jump()
    {
        // clear vertical velocity before jumping to ensure consistent jump height
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound);
    }

    public void SetControlEnabled(bool enabled)
    {
        controlEnabled = enabled;
        if (!enabled)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    // Visualize ground check circle in Scene view and attack range
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
