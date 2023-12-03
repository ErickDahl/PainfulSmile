using UnityEngine;
using TMPro;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _loginMenu;

    [SerializeField]
    private GameObject _roomMenu;

    [SerializeField]
    private TMP_InputField _nickNameInputField;

    [SerializeField]
    private VoidEventChannel _onConnectedToMaster;

    [SerializeField]
    private StringEventChannel _onSetPlayerNickName;

    private string _playerNickName;

    void Start()
    {
        SetPlayerNickNameLocal();
        SetDefaultUIState();
    }

    void OnEnable()
    {
        _onConnectedToMaster.OnEventRaised += SetConfigsToRoomMenu;
        _nickNameInputField.onValueChanged.AddListener(SetPlayerNickName);
    }

    void OnDisable()
    {
        _onConnectedToMaster.OnEventRaised -= SetConfigsToRoomMenu;
        _nickNameInputField.onValueChanged.RemoveListener(SetPlayerNickName);
    }

    private void SetPlayerNickNameLocal()
    {
        _playerNickName = $"Player_{Random.Range(1, 99999)}";
        _nickNameInputField.text = _playerNickName;
    }

    private void SetDefaultUIState()
    {
        _loginMenu.gameObject.SetActive(true);
        _roomMenu.gameObject.SetActive(false);
    }

    private void SetPlayerNickName(string value)
    {
        _playerNickName = value;
    }

    private void SetConfigsToRoomMenu()
    {
        if (_playerNickName != "")
        {
            _onSetPlayerNickName.RaiseEvent(_playerNickName);
            EnableRoomMenu();
        }
    }

    private void EnableRoomMenu()
    {
        _loginMenu.gameObject.SetActive(false);
        _roomMenu.gameObject.SetActive(true);
    }
}
