using UnityEngine.SceneManagement;

public class PainfulSmileSceneManager : GenericSingleton<PainfulSmileSceneManager>
{
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        GameManager.Instance.EnableTimer();
        SpawnManager.Instance.EnableTimer();
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
