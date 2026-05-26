using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private TimerManager timer;

    private void Start()
    {
        timer = FindObjectOfType<TimerManager>();
        if (timer == null)
        {
            Debug.LogError("TimerManager tidak ditemukan di scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            HealtManager.health--;

            if (HealtManager.health <= 0)
            {
                PlayerManager.isGameOver = true;

                if (timer != null)
                {
                    timer.InvalidateTimer(); // Tandai waktu tidak sah
                    timer.StopTimer();       // Hentikan timer
                }

                gameObject.SetActive(false); // Nonaktifkan player
            }
            else
            {
                StartCoroutine(GetHurt());
            }
        }
    }

    IEnumerator GetHurt()
    {
        Physics2D.IgnoreLayerCollision(6, 8);
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(6, 8, false);
    }
}
