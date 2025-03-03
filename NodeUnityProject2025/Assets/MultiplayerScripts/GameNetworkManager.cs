using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    // IP and port to connect to (change these to your actual server address and port)
    private string serverIP = "webapiassignment6.onrender.com"; // Replace with your server's address
    private int serverPort = 443; // Use the same port as in the server.js script

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Debug.Log("Starting the game server or client...");
    }

    // Start the server
    public void StartServer()
    {
        NetworkManager.singleton.StartServer();
        Debug.Log("Server started");
    }

    // Start the client and connect to the server
    public void StartClient()
    {
        // Setting the networkAddress to the full address of the server
        NetworkManager.singleton.networkAddress = serverIP + ":" + serverPort;
        
        // Connect to the server
        NetworkManager.singleton.StartClient();
        Debug.Log("Client started and connecting to: " + serverIP + ":" + serverPort);
    }

    // Stop the server or client
    public void StopConnection()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopServer();
        Debug.Log("Stopped the connection");
    }
}
