using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public float stepRate = 0.5f; // Waktu antar langkah
    private float stepTimer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stepTimer = stepRate;
    }

    void Update()
    {
        // Deteksi jika karakter sedang bergerak
        if (rb.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(footstepClip);
                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
