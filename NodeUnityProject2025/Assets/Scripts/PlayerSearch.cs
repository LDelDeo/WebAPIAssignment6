using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;  // Import TextMeshPro

public class PlayerSearch : MonoBehaviour
{
    public TextMeshProUGUI playerDataText;  // UI element to display player data
    public TMP_InputField playerIdInput;    // Input field for entering player ID
    private string apiUrl = "http://localhost:3000/player/";  // API endpoint

    // Function to search for a player by ID
    public void SearchPlayer()
    {
        string playerId = playerIdInput.text.Trim(); // Get input from the user
        if (!string.IsNullOrEmpty(playerId))
        {
            StartCoroutine(GetPlayerData(playerId));
        }
        else
        {
            playerDataText.text = "Please enter a valid Player ID.";
        }
    }

    // Coroutine to fetch player data from the server
    IEnumerator GetPlayerData(string playerId)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl + playerId))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                playerDataText.text = "Error: " + webRequest.error;
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                Player player = JsonUtility.FromJson<Player>(responseText);

                if (player != null && !string.IsNullOrEmpty(player.screenName))
                {
                    playerDataText.text = $"Player Found:\nUsername: {player.screenName}\nFirst Name: {player.firstName}\nLast Name: {player.lastName}\nDate Started: {player.dateStarted}\nScore: {player.score}";
                }
                else
                {
                    playerDataText.text = "Player not found.";
                }
            }
        }
    }

    // Player class to match JSON response
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
}
