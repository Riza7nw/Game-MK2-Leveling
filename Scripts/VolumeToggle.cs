using UnityEngine;
using UnityEngine.UI;

public class VolumeToggle : MonoBehaviour
{
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;
    public Image buttonImage;
    public AudioSource musicSource;

    private bool isMuted = false;

    public void ToggleVolume()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            musicSource.Pause(); // atau musicSource.volume = 0;
            buttonImage.sprite = volumeOffSprite;
        }
        else
        {
            musicSource.Play(); // atau musicSource.volume = 1;
            buttonImage.sprite = volumeOnSprite;
        }
    }
}
