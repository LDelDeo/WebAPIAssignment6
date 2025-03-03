using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class PostData : MonoBehaviour
{
    //string serverURL = "http://localhost:3000/sentdatatodb";
    string serverURL = "https://webapiassignment6.onrender.com/sentdatatodb";
    PlayerData player;
    public TMP_Text responseText;

    [System.Serializable]
    public class PlayerData
    {
        public string screenName;
        public string firstName;
        public string lastName;
        public string dateStarted;
        public int score;
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public string message;
        public string playerid;
    }

    void Start()
    {
        // Example of calling SetupPlayerData
        // SetupPlayerData("CobraKai", "Miguel", "Diaz");
    }

    public void SetupPlayerData(string screenName, string firstName, string lastName)
    {
        player = new PlayerData
        {
            screenName = screenName,
            firstName = firstName,
            lastName = lastName,
            dateStarted = "",
            score = 0
        };

        string json = JsonUtility.ToJson(player);
        Debug.Log("Sending JSON: " + json);
        StartCoroutine(PostPlayerData(json));
    }

    public IEnumerator PostPlayerData(string json)
    {
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(jsonToSend),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("Raw Server Response: " + response); // LOG FULL RESPONSE

            // Deserialize the server response to extract player ID and message
            PlayerResponse playerResponse = JsonUtility.FromJson<PlayerResponse>(response);

            if (playerResponse != null && !string.IsNullOrEmpty(playerResponse.playerid))
            {
                string playerId = playerResponse.playerid;
                string message = playerResponse.message;

                // Display player ID and message in responseText
                responseText.text = $"{message} {playerId}\nDon't share this with anyone, write this string down.";
                responseText.color = Color.green;
            }
            else
            {
                responseText.text = "Player added, but no ID received!";
                responseText.color = Color.red;
            }
        }
        else
        {
            Debug.LogError($"Error sending data: {request.error}");
            responseText.text = "Error sending data!";
            responseText.color = Color.red;
        }
    }
}
