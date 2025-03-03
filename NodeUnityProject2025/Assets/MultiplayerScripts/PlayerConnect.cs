using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnect : MonoBehaviour
{
    public string networkAddress = "http://webapiassignment6.onrender.com/";

    void Start()
    {
        // Set the network address to your online server
        NetworkManager.singleton.networkAddress = networkAddress;

        // Start the client to connect to the server
        NetworkManager.singleton.StartClient();
    }
}
