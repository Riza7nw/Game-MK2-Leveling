using UnityEngine;

public class AutoWalk : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        // Aktifkan animasi "Walk" saat bergerak
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }
}
