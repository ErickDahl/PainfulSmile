using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

namespace Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private StringEventChannel _onPlayerDied;

        [SerializeField]
        private GameObject _winLoseMenu;

        [SerializeField]
        private TextMeshProUGUI _text;

        private void Start()
        {
            _winLoseMenu.SetActive(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _onPlayerDied.OnEventRaised += EndGame;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _onPlayerDied.OnEventRaised -= EndGame;
        }

        public void EndGame(string playerName)
        {
            Player[] playerList = PhotonNetwork.PlayerList;

            foreach (Player player in playerList)
            {
                if (player.NickName != playerName)
                {
                    _winLoseMenu.SetActive(true);
                    _text.text =
                        player.NickName + " is the winner! \nPlease leave the game to play again";

                    break;
                }
            }
        }
    }
}
