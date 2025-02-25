using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;  // Include the TextMeshPro namespace

public class PlayerDataFetcher : MonoBehaviour
{
    public TextMeshProUGUI playerDataText;  // Change to TextMeshProUGUI for TextMeshPro
    private string apiUrl = "http://localhost:3000/player/";  // The endpoint of your API

    // Start is called before the first frame update
    void Update()
    {
        // Start the coroutine to fetch the player data
        StartCoroutine(GetPlayerData());
    }


    // Coroutine to fetch player data from the server
    IEnumerator GetPlayerData()
    {
        // Create a UnityWebRequest to fetch the player data
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Wait for the request to complete
            yield return webRequest.SendWebRequest();

            // Check if there was an error
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                playerDataText.text = "Error: " + webRequest.error;
            }
            else
            {
                // If the request is successful, parse the data
                string responseText = webRequest.downloadHandler.text;

                // Optionally, you can parse the JSON if you want to display specific data
                PlayerResponse playerResponse = JsonUtility.FromJson<PlayerResponse>(responseText);
                
                // Display the player data
                if (playerResponse.players != null && playerResponse.players.Length > 0)
                {
                    playerDataText.text = "Player List:\n";
                    foreach (var player in playerResponse.players)
                    {
                        playerDataText.text += $"Username: {player.screenName}, First Name: {player.firstName}, Last Name: {player.lastName}, Date Started: {player.dateStarted}, Score: {player.score}\n";
                    }
                }
                else
                {
                    playerDataText.text = "No players found.";
                }
            }
        }
    }

    // This is the structure that matches the JSON response you expect
    [System.Serializable]
    public class Player
    {
        public string playerid;
        public string screenName;
        public string firstName;
        public string lastName;
        public string dateStarted;
        public int score;
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public Player[] players;
    }
}
