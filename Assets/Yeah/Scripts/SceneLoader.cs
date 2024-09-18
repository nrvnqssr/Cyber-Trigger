using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        FunnyTimeScaler.instance.ResetTime();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("Gamelevel");
    }

    public void Quit()
    {
        Application.Quit();
    }
}