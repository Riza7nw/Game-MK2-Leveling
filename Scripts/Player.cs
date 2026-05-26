using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected bool facingRight = true;
    protected bool isGrounded = true;

    public float speed = 5f;
    public float jumpForce = 5f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (rb == null)
            Debug.LogError("Rigidbody2D tidak ditemukan! Tambahkan Rigidbody2D ke GameObject ini.");
        if (anim == null)
            Debug.LogError("Animator tidak ditemukan! Tambahkan Animator ke GameObject ini.");
    }

    protected virtual void Update()
    {
        float moveInput = GetHorizontalInput();
        Move(moveInput);

        if (IsJumpPressed() && isGrounded)
            Jump();

        UpdateAnimationState(moveInput);
    }

    protected virtual void Move(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        Flip(moveInput);
    }

    protected virtual void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    protected virtual void Flip(float moveInput)
    {
        if (moveInput > 0 && !facingRight || moveInput < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    protected virtual void UpdateAnimationState(float moveInput)
    {
        anim.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f && isGrounded);
    }

    protected abstract float GetHorizontalInput();
    protected abstract bool IsJumpPressed();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
// FINAL VERSION of Player class
public sealed class Player : BaseCharacter
{
    private float lastInput = 0f;

    protected override float GetHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    protected override bool IsJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    protected override void UpdateAnimationState(float moveInput)
    {
        base.UpdateAnimationState(moveInput);
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f && speed == 5f && isGrounded);
    }

    protected override void Update()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? 5f : 2f;

        float moveInput = GetHorizontalInput();
        if (Mathf.Abs(moveInput - lastInput) > 0.01f)
        {
            Debug.Log("Input horizontal: " + moveInput);
            lastInput = moveInput;
        }

        base.Update();
    }
}
