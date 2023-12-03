using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonSceneManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel _onJoinedRoom;

    [SerializeField]
    private VoidEventChannel _onGameSceneLoaded;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        _onJoinedRoom.OnEventRaised += LoadNextScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        _onJoinedRoom.OnEventRaised -= LoadNextScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PhotonNetwork.LoadLevel(currentSceneIndex + 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GamePhoton")
        {
            _onGameSceneLoaded.RaiseEvent();
        }
        Debug.Log("Scene loaded: " + scene.name);
    }
}
