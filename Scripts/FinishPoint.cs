using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletePanel; // Panel "Level Complete"
    public GameObject nextLevelPanel;

    private void Start()
    {
        // Pastikan panel "Level Complete" tidak aktif saat permainan dimulai
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Level Complete Panel belum diatur di Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Hentikan waktu permainan
            Time.timeScale = 0f;

            // Tampilkan panel "Level Complete"
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Level Complete Panel tidak ditemukan.");
            }

            // Hentikan timer speedrun dan tandai level selesai
            TimerManager timer = FindObjectOfType<TimerManager>();
            if (timer != null)
            {
                timer.OnLevelComplete();
            }

            if (nextLevelPanel != null)
            {
                nextLevelPanel.SetActive(true);
            }
        }
    }

    // Fungsi untuk melanjutkan ke level berikutnya
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Tidak ada level selanjutnya!");
            LoadMainMenu();
        }
    }

    // Fungsi untuk memulai ulang level saat ini
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fungsi untuk kembali ke menu utama
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
