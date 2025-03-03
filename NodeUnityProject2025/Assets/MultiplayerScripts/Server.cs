using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server : MonoBehaviour
{
    void Start()
    {
        // Start the server (or host to start both client and server)
        NetworkManager.singleton.StartHost();
    }
}
