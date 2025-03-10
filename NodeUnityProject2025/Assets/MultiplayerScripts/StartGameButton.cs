using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class StartGameButton : MonoBehaviour
{
    NetworkRoomManager roomManager;
    Button startButton;

    void Start()
    {
        startButton = GetComponent<Button>();
        roomManager = FindAnyObjectByType<NetworkRoomManager>();

        startButton.onClick.AddListener(() => {
            if (roomManager)
            {
                roomManager.CheckReadyToBegin();
            }
        });
    }

    void Update()
    {
        startButton.interactable = NetworkServer.active && roomManager.allPlayersReady;
    }
}
