using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private VoidEventChannel _onConnectedToMaster;

    [SerializeField]
    private StringEventChannel _onSetPlayerNickName;

    [SerializeField]
    StringEventChannel _onCreateRoom;

    [SerializeField]
    private VoidEventChannel _onJoinedRoom;

    [SerializeField]
    private VoidEventChannel _onGameSceneLoaded;

    [SerializeField]
    private PlayerControllerPhoton _playerPrefab;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _onSetPlayerNickName.OnEventRaised += SetPlayerNickName;
        _onCreateRoom.OnEventRaised += CreateRoom;
        _onGameSceneLoaded.OnEventRaised += CreatePlayer;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _onSetPlayerNickName.OnEventRaised -= SetPlayerNickName;
        _onCreateRoom.OnEventRaised -= CreateRoom;
        _onGameSceneLoaded.OnEventRaised -= CreatePlayer;
    }

    private void SetPlayerNickName(string name)
    {
        PhotonNetwork.NickName = name;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    private void CreateRoom(string name)
    {
        RoomOptions roomOptions = new RoomOptions { };
        PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default);
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    //Called on the button event in the editor
    public void ConnectSetting()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //Called on the button event in the editor
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _onConnectedToMaster.RaiseEvent();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
        _onJoinedRoom.RaiseEvent();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = $"room_{Random.Range(1, 99999)}";
        CreateRoom(roomName);
    }
}
