using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTag : NetworkBehaviour
{
    [SyncVar] public bool isIt = false;
    [SyncVar] public bool isFrozen = false;
    public GameObject playerBody;

    private void OnCollisionEnter(Collision other) 
    {
        if (!isServer)return; //only the server handles tagging

        PlayerTag otherPlayer = other.gameObject.GetComponent<PlayerTag>();

        if (otherPlayer != null)
        {
            if (isIt && !otherPlayer.isFrozen)
            {
                otherPlayer.FreezePlayer();
                otherPlayer.GetComponent<PlayerController>().isFrozen = true;
            }
            else if (!isIt && isFrozen && !otherPlayer.isFrozen)
            {
                UnFreezePlayer();
                otherPlayer.GetComponent<PlayerController>().isFrozen = false;
            }
        }
    }

    [Server]
    public void FreezePlayer()
    {
        isFrozen = true;
        RPCUpdateState(isFrozen);
    }

    [Server]
    public void UnFreezePlayer()
    {
        isFrozen = false;
        RPCUpdateState(isFrozen);
    }

    [ClientRpc]
    void RPCUpdateState(bool frozen)
    {
        isFrozen = frozen;
        playerBody.GetComponent<Renderer>().material.color = frozen ? Color.blue : Color.red;
        GetComponent<PlayerController>().isFrozen = frozen;
    }
}
