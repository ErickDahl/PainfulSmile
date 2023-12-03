using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomUIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _roomNameInputField;

    [SerializeField]
    private Button _createOrJoinRoomButton;

    [SerializeField]
    StringEventChannel _onCreateRoom;

    private string _roomName;

    void Start()
    {
        SetRoomLocalName();
    }

    void OnEnable()
    {
        _createOrJoinRoomButton.onClick.AddListener(CreateRoom);
        _roomNameInputField.onValueChanged.AddListener(SetRoomName);
    }

    void OnDisable()
    {
        _createOrJoinRoomButton.onClick.RemoveListener(CreateRoom);
        _roomNameInputField.onValueChanged.RemoveListener(SetRoomName);
    }

    private void CreateRoom()
    {
        if (_roomName != "")
        {
            _onCreateRoom.RaiseEvent(_roomName);
        }
    }

    private void SetRoomLocalName()
    {
        _roomName = $"room_{Random.Range(1, 99999)}";
        _roomNameInputField.text = _roomName;
    }

    private void SetRoomName(string value)
    {
        _roomName = value;
    }
}
