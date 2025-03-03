using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoConnect : MonoBehaviour
{
    [Header("Network Settings")]
    public string serverAddress = "webapiassignment6.onrender.com"; // Your Render server domain
    public int serverPort = 443; // Correct port for HTTPS traffic

    void Start()
    {
        // Ensure NetworkManager exists and is configured
        if (NetworkManager.singleton != null)
        {
            // Set the network address to your Render server address with the port
            string fullAddress = $"{serverAddress}:{serverPort}";
            NetworkManager.singleton.networkAddress = fullAddress;

            // Debug log to check if NetworkManager is active
            Debug.Log("NetworkManager found. Checking server status...");

            // Check if the server is already running
            if (NetworkServer.active)
            {
                Debug.Log("Server is already active. Starting as client.");
                // If the server is active, this player will join as a client
                NetworkManager.singleton.StartClient();
            }
            else
            {
                Debug.Log("No active server. Starting as host.");
                // If the server is not active, this player becomes the host
                NetworkManager.singleton.StartHost();
            }
        }
        else
        {
            Debug.LogError("NetworkManager not found in the scene. Please add a NetworkManager component.");
        }
    }

    void OnApplicationQuit()
    {
        // Ensure we stop the server or client when the application quits
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.active)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
