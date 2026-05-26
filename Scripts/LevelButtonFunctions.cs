using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk bisa load scene

public class LevelButtonFunctions : MonoBehaviour
{
    public void RestartLevel()
    {
        Debug.Log("TOMBOL RESTART DIKLIK!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Debug.Log("TOMBOL NEXT LEVEL DIKLIK!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
