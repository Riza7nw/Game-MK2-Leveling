using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public void LoginButtonPressed()
    {
        // Kamu bisa tambahkan validasi di sini jika ingin
        Debug.Log("Berpindah ke Main Menu");
        SceneManager.LoadScene("Login");
    }

    public void MainMenuButtonPressed()
    {
        // Kamu bisa tambahkan validasi di sini jika ingin
        Debug.Log("Berpindah ke Main Menu");
        SceneManager.LoadScene("Main Menu");
    }

    public void LastMainMenuPressed()
    {
        // Kamu bisa tambahkan validasi di sini jika ingin
        Debug.Log("Berpindah ke Main Menu");
        SceneManager.LoadScene("Main Menu 1");
    }

    public void SignupPressed()
    {
        // Kamu bisa tambahkan validasi di sini jika ingin
        Debug.Log("Berpindah ke Main Menu");
        SceneManager.LoadScene("Signup");
    }
}
