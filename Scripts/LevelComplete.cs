using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public GameObject nextLevelPanel; // Panel "Next Level"

    private void Start()
    {
        // Pastikan panel "Next Level" tidak aktif saat permainan dimulai
        if (nextLevelPanel != null)
        {
            nextLevelPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Next Level Panel belum diatur di Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hentikan waktu permainan
            Time.timeScale = 0f;

            // Tampilkan panel "Next Level"
            if (nextLevelPanel != null)
            {
                nextLevelPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Next Level Panel tidak ditemukan.");
            }
        }
    }

    // Fungsi untuk melanjutkan ke level berikutnya
    public void LoadNextLevel()
    {
        Debug.Log("Tombol Next Level ditekan."); // Debugging
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Memuat level berikutnya: {nextSceneIndex}");
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
        Time.timeScale = 1f; // Lanjutkan waktu permainan
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang level saat ini
    }

    // Fungsi untuk kembali ke menu utama
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Lanjutkan waktu permainan
        SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene menu utama Anda
    }
}
