using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerSearch : MonoBehaviour
{
    public TextMeshProUGUI playerDataText;
    public TMP_InputField playerIdInput;
    public GameObject loginButton;
    private string apiUrl = "http://localhost:3000/player/";

    public void Start()
    {
        loginButton.SetActive(false);
    }
    public void SearchPlayer()
    {
        string playerId = playerIdInput.text.Trim();
        if (!string.IsNullOrEmpty(playerId))
        {
            StartCoroutine(GetPlayerData(playerId));
        }
        else
        {
            playerDataText.text = "Please enter a valid Player ID.";
        }
    }

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
                    // Store player data in PlayerDataManager
                    PlayerDataManager.SetPlayerData(player);
                    playerDataText.text = $"Player Found:\nUsername: {player.screenName}\nFirst Name: {player.firstName}\nLast Name: {player.lastName}\nDate Started: {player.dateStarted}\nScore: {player.score}";

                    loginButton.SetActive(true);

                    // Load the next scene
                    //SceneManager.LoadScene(1);
                }
                else
                {
                    playerDataText.text = "Player not found.";
                }
            }
        }
    }

    public void Login()
    {
        SceneManager.LoadScene(1);
        loginButton.SetActive(false);
    }
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
