using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameTimer : NetworkBehaviour
{
    [SyncVar] public float timeRemaining = 60.0f;

    void Update()
    {
        if (!isServer) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }

    [Server]
    private void EndGame()
    {
        int frozenCount = 0;

        PlayerTag[] players = FindObjectsByType<PlayerTag>(FindObjectsSortMode.None);

        foreach(var player in players)
        {
            if (player.isFrozen)
            {
                frozenCount++;
            }
        }

        if (frozenCount == players.Length - 1)
        {
            //It Player Wins
            RPCShowWin(true);
        }
        else
        {
            //Survivors Win
            RPCShowWin(false);
        }
    }

    [ClientRpc]
    void RPCShowWin(bool itWins)
    {
        Debug.Log(itWins? "It WINS": "Survivors WIN");
    }
}
