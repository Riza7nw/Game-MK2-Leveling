using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f; // Pause game
            winScreen.SetActive(true);
        }
    }

    // Tombol Next Level
    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Tombol Restart Level
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ✅ Tombol Home ke Menu Utama
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(3); // 0 adalah build index untuk Main Menu
    }
}
