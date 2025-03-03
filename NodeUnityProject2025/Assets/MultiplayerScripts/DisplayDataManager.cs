using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class DisplayDataManager : MonoBehaviour
{
    public TextMeshProUGUI playerDataText;
    //private string apiUrl = "http://localhost:3000/updatePlayer"; // Updated route for updating player data
    private string apiUrl = "https://webapiassignment6.onrender.com/updatePlayer";

    void Start()
    {
        // Initially set the player data text
        UpdatePlayerDataText();
    }

    void Update()
    {
        // Check if the "P" key is pressed to increase the score
        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseScore();
        }
    }

    private void IncreaseScore()
    {
        // Increment the player's score
        PlayerDataManager.Score += 1; // You can change the increment value as needed

        // Update the UI to show the new score
        UpdatePlayerDataText();

        // Update the player score in the database (send POST request)
        StartCoroutine(UpdatePlayerDataInDatabase(PlayerDataManager.PlayerID, PlayerDataManager.ScreenName, PlayerDataManager.FirstName, PlayerDataManager.LastName, PlayerDataManager.DateStarted, PlayerDataManager.Score));
    }

    private void UpdatePlayerDataText()
    {
        // Check if player data exists, and update the UI accordingly
        if (string.IsNullOrEmpty(PlayerDataManager.PlayerID))
        {
            playerDataText.text = "No player data found. Please log in again.";
        }
        else
        {
            playerDataText.text = $"{PlayerDataManager.ScreenName}\n\n" +
                                  $"{PlayerDataManager.PlayerID}\n" +
                                  $"All-Time Kills: {PlayerDataManager.Score}";
        }
    }

    private IEnumerator UpdatePlayerDataInDatabase(string playerId, string screenName, string firstName, string lastName, string dateStarted, int newScore)
    {
        // Create a PlayerData object to send in the POST request
        PlayerData playerData = new PlayerData
        {
            playerid = playerId,
            screenName = screenName,
            firstName = firstName,
            lastName = lastName,
            dateStarted = dateStarted,
            score = newScore
        };

        // Convert the player data to JSON
        string json = JsonUtility.ToJson(playerData);

        // Create a POST request to the server to update the player data
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(apiUrl, json))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Set the body to the JSON string
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating player data: " + webRequest.error);
            }
            else
            {
                Debug.Log("Player data updated successfully!");
            }
        }
    }

    // Define the class structure that matches the expected request body
    [System.Serializable]
    public class PlayerData
    {
        public string playerid;
        public string screenName;
        public string firstName;
        public string lastName;
        public string dateStarted;
        public int score;
    }
}
