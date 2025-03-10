using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkRoomPlayer
{
    public TMP_Text playerStatusText;

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateReadyStatus();
        Debug.Log("Player Joined Lobby");
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        UpdateReadyStatus();
    }

    private void UpdateReadyStatus()
    {
        playerStatusText.text = "Is Ready";
    }
}
