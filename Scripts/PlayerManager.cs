using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool isGameOver;
    [SerializeField]
    private GameObject gameOverScreen;

    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {
        // Saat scene dimulai, pastikan layar game over dimatikan
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver && gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
