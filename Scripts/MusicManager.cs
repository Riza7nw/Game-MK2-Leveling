using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource musicSource;

    private string currentSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene != currentSceneName)
        {
            musicSource.Stop();
            Destroy(gameObject);
        }
    }

    // 🔽 Tambahan ini WAJIB agar OptionsMenu tidak error
    public void SetMusicEnabled(bool isEnabled)
    {
        if (isEnabled)
            musicSource.Play();
        else
            musicSource.Stop(); // atau Stop()
    }

    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }
}
