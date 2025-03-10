using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ReadyButton : MonoBehaviour
{
    NetworkRoomPlayer roomPlayer;
    Button readyButton;

    void Start()
    {
        readyButton = GetComponent<Button>();
        roomPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayer>();

        readyButton.onClick.AddListener(() => {
            if (roomPlayer)
            {
                roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
            }
        });
    }
}
