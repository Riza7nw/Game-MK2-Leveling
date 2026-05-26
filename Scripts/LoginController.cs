using UnityEngine;

public class LoginController : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject mainMenuPanel;

    public void OnLoginButtonClicked()
    {
        // Sembunyikan panel login
        loginPanel.SetActive(false);

        // Tampilkan panel main menu
        mainMenuPanel.SetActive(true);

        Debug.Log("Berpindah ke Main Menu");
    }
}
