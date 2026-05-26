using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle musicToggle;

    void Start()
    {
        // Set posisi toggle sesuai status musik
        musicToggle.isOn = MusicManager.Instance.IsMusicPlaying();

        // Tambahkan listener untuk toggle
        musicToggle.onValueChanged.AddListener(OnToggleMusic);
    }

    void OnToggleMusic(bool isOn)
    {
        MusicManager.Instance.SetMusicEnabled(isOn);
    }
}
