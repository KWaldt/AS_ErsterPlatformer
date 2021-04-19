using System;
using System.Collections;
using UnityEngine;

// Das hier braucht euch noch nicht zu kümmern.

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class VeryBasicPlayer : MonoBehaviour
{
    [Header("Locomotion")]
    public float walkSpeed = 10f;
    public float jumpSpeed = 10f;
    [Range(0, 1)] public float airControl = 0f;
    public bool rawInput;
    public int maxJumps = 1;

    [Header("Ground Checker")]
    public Vector3 groundCheckOffset;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayerMask = -1; // -1 = everything

    [Header("Other")]
    public bool facingRight = true;

    private int jumpCount;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Animator anim;
    private bool willJump;
    private static readonly int VelocityX = Animator.StringToHash("velocityX");
    private static readonly int VelocityY = Animator.StringToHash("velocityY");

    // Property
    private bool allowInput = true;
    public bool AllowInput
    {
        get { return allowInput; }
        set
        {
            if (value == false && rb)
            {
                rb.velocity = Vector2.zero;
            }
            allowInput = value;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
        }
        startPosition = transform.position;
    }

    private void Update()
    {
        UpdateAnimator();
        if (!AllowInput)
        {
            return;
        }

        bool isGrounded = IsGrounded();
        // Könnten IsGrounded() in eine Property umwandeln, und dann bei Wertwechsel jumpCount aufrufen,
        // aber für die Performance ist das irrelevant. -> Überoptimierung
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (isGrounded || (jumpCount < maxJumps))
        {
            if (Input.GetButtonDown("Jump") && !willJump)
            {
                willJump = true;
            }
        }
    }

    private void UpdateAnimator()
    {
        anim.SetFloat(VelocityX, Mathf.Abs(rb.velocity.x));
        anim.SetFloat(VelocityY, rb.velocity.y);
    }

    // Für Physic-Bewegungen
    void FixedUpdate()
    {
        // if AllowInput == false
        if (!AllowInput)
        {
            return;
        }

        // condition ? true : false
        float horizontalInput = rawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        if (IsGrounded())
        {
            Move(horizontalInput * walkSpeed);
        }
        else
        {
            // Linere Interpolation. Wie eine Maske in Photoshop.
            // airControl = 0 : speed = rb.velocity.x | airControl = 1 : speed = horizontalInput * walkSpeed
            // airControl = 0.5 : der Wert zwischen rb.velocity.x & horizontalInput * walkSpeed
            float speed = Mathf.Lerp(rb.velocity.x, horizontalInput * walkSpeed, airControl);
            //float speed = (1 - airControl) * rb.velocity.x + airControl * horizontalInput * walkSpeed; // was intern im Lerp passiert
            Move(speed);
        }

        if (willJump)
        {
            Jump(jumpSpeed);
            jumpCount++;
            willJump = false;
        }
    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + groundCheckOffset, groundCheckRadius, groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
            {
                return true;
            }
        }
        return false;
    }
    
    private void Jump(float speed)
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    private void Move(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        // Abhängig von Geschwindigkeit. Test-Case: airControl 0.1 -> flippt erst spät, könnte verwirren
        if ((facingRight && speed < 0) || (!facingRight && speed > 0))
        {
            Flip();
        }
    }

    private void Flip()
    {
        // localScale flippt auch unsere Collider (und die Kinder).
        // Wir verwenden SpriteRenderer, um sicherzugehen, dass das BaseSprite nach rechts schaut.
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1, 1, 1));
        facingRight = !facingRight;
    }
    
    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
