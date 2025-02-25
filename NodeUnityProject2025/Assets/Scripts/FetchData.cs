using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class FetchData : MonoBehaviour
{
    string serverURL = "http://localhost:3000/player";
    PlayerData player;
    public GameObject playerData;
    
    void Start()
    {
        StartFetch();
    }



    public IEnumerator GetData()
{
    using (UnityWebRequest request = UnityWebRequest.Get(serverURL))
    {
        yield return request.SendWebRequest(); 

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Success
            string json = request.downloadHandler.text;
            Debug.Log($"Received the data: {json}");  // Log the raw response

            // Deserialize the data
            player = JsonUtility.FromJson<PlayerData>(json);

            // Debug the player object to check the values
            Debug.Log($"Username: {player.screenName}, First Name: {player.firstName}, Last Name: {player.lastName}, Date Started: {player.dateStarted}, Score: {player.score}");
        }
        else
        {
            // Failed
            Debug.Log($"Error fetching data: {request.error}");
        }
    }
}


    public IEnumerator GetDataByID(string playerid, string playerName)
{
    // Construct the URL by appending playerID and name
    string url = $"{serverURL}?playerid={playerid}&name={playerName}";

    using (UnityWebRequest request = UnityWebRequest.Get(url))
    {
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log($"Success: {response}");

            // Extract player data and set the player object
            player = JsonUtility.FromJson<PlayerData>(response);
            Debug.Log($"Player ID: {playerid}, Name: {player.screenName}, Score: {player.score}");

            // Update UI
            GetPlayer();
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }
}


    public void StartFetch()
    {
        StartCoroutine(GetData());
    }

    public void SetupPlayerSearchData(string screenName, string playerid)
    {

        player = new PlayerData();

        player.screenName = screenName;


        string json = JsonUtility.ToJson(player);
        Debug.Log(json);
        StartCoroutine(GetDataByID(json, playerid));
    } 

    public void GetPlayer()
    {

        if (player != null)
        {
            playerData.transform.GetChild(0).GetComponent<TMP_Text>().text = player.screenName;
            playerData.transform.GetChild(1).GetComponent<TMP_Text>().text = player.score.ToString();
        }

    }

    public string ExtractPlayerId(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"playerid\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);
    }
}

public class PlayerData 
{
    public string screenName;
    public string firstName;
    public string lastName;
    public string dateStarted;
    public int score;
}