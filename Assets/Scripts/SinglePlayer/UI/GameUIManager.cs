using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverPanel;

    void Start()
    {
        _gameOverPanel.SetActive(false);
    }

    // void OnEnable()
    // {
    //     GameManager.Instance.OnGameOverEvent += EnableGameOverPanel;
    // }

    // void OnDisable()
    // {
    //     GameManager.Instance.OnGameOverEvent -= EnableGameOverPanel;
    // }

    private void EnableGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
    }
}
