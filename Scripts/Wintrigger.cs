using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f; // Pause game
            winScreen.SetActive(true);

            TimerManager timer = FindObjectOfType<TimerManager>();
            if (timer != null)

            {
                timer.OnLevelComplete();
            }
        }
    }
}
