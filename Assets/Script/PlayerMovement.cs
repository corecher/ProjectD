using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("바닥 감지 설정")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    public int jumpCount;
    private int maxJumpCount = 1;
    private Animator animator;
    public Collider2D collider;
    public int hp;
    public CoreManager coreManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = 0;
            collider.offset = new Vector2(0,-1);
        }
        if (Input.GetButtonDown("Jump")||Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded || jumpCount < maxJumpCount)
            {
                Jump();
            }
        }
        if (Input.GetKeyDown(KeyCode.G)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            FastFall();
        }
        FlipCharacter();
        animator.SetBool("Jump",!isGrounded);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        collider.offset = new Vector2(0,0);
        jumpCount++;
    }
    private void FastFall()
    {
        animator.SetTrigger("FastFall");
        if (!isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -45f);
        }
    }
    public void GetDamage(int damage)
    {
        if(hp <= 0) return; 
        hp -= damage;
        if( hp <= 0 )
        {
            coreManager.GameOver(false,5);
        }
    }
    private void FlipCharacter()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
            
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 1);
        }
    }
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
