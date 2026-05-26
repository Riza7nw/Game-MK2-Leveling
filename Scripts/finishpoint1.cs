using UnityEngine;
using UnityEngine.SceneManagement;

public class finishpoint : MonoBehaviour
{
    public static finishpoint instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           
        }
        else
        {
      
        }
    }

    public void NextLevel1()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
