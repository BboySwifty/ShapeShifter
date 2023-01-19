using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour
{

    [Header("Particle Systems")]
    public GameObject shiftEffect;

    [Header("Audio Clips")]
    public AudioClip poofClip;
    public AudioClip jumpClip;
    public AudioClip hitClip;

    [Header("UI")]
    public UICooldownBar cooldownBar;

    [Header("WallJumping")]
    public Transform frontCheck;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float checkRadius;
    public float wallJumpTimer;

    [Header("ShapeShiftSO")]
    public ShapeShiftSO[] shapes;

    public bool IsInvisible { get; private set; }

    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rigidbody2d;
    private float horizontal;

    private int currentPhaseIndex = 0;
    private ShapeShiftSO currentPhase;
    private SpriteLibrary spriteLibrary;

    private bool isGrounded;
    private bool wallSliding;
    private bool wallJumping;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ShapeShift(0);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (!wallSliding)
        {
            if (!Mathf.Approximately(horizontal, 0.0f))
            {
                float angle = currentPhase.spritesAreReversed ? 180 : 0;
                if (horizontal < 0)
                {
                    angle -= 180;
                }
                transform.localRotation = Quaternion.Euler(0, angle, 0);
            }

            rigidbody2d.velocity = new Vector2(horizontal * currentPhase.speedModifier, rigidbody2d.velocity.y);

            animator.SetFloat("VelX", Mathf.Abs(rigidbody2d.velocity.x));
            animator.SetFloat("VelY", rigidbody2d.velocity.y);
        }

        // Jumping

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);

        if (!isGrounded && currentPhase.canWallJump && !wallJumping)
        {
            wallSliding = Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundMask);
        }
        else
        {
            wallSliding = false;
        }

        animator.SetBool("WallSliding", wallSliding);

        if (wallSliding)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Clamp(rigidbody2d.velocity.y, -currentPhase.wallSlidingSpeed, float.MaxValue));
        }

        // Input

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded)
                Jump();
            else if (wallSliding)
            {
                wallJumping = true;
                Jump();
                Invoke(nameof(StopWallJump), wallJumpTimer);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShapeShift(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShapeShift(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShapeShift(3);
        }
    }

    public void Kill()
    {
        rigidbody2d.velocity = new Vector2(-5, 5);
        rigidbody2d.gravityScale = 5;
        animator.SetTrigger("Hit");
        PlaySound(hitClip);
        Disable();
        StartCoroutine(RestartLevel());
    }

    public void Disable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.RestartLevel();
    }

    private void Jump()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, currentPhase.jumpModifier);
        PlaySound(jumpClip);
    }

    private void StopWallJump()
    {
        wallJumping = false;
    }

    private void ShapeShift(int phase)
    {
        if (phase >= shapes.Length)
            return;

        currentPhaseIndex = phase;
        currentPhase = shapes[currentPhaseIndex];
        rigidbody2d.gravityScale = currentPhase.gravityScale;
        if (currentPhase.gravityScale == 0)
        {
            rigidbody2d.velocity = new Vector2(0, 0);
        }

        Color color = spriteRenderer.color;
        IsInvisible = currentPhase.isInvisible;
        color.a = IsInvisible ? 0.5f : 1f;
        spriteRenderer.color = color;

        spriteLibrary.spriteLibraryAsset = currentPhase.spriteLibraryAsset;
        if(currentPhaseIndex != 0)
        {
            PlaySound(poofClip);
            Instantiate(shiftEffect, rigidbody2d.position, Quaternion.identity);
        }
    }
}
