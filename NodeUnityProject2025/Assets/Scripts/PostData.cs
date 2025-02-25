using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class PostData : MonoBehaviour
{
    string serverURL = "http://localhost:3000/sentdatatodb";
    PlayerData player;
    public TMP_Text responseText;
    void Start()
    {
        //SetupPlayerData("CobraKai", "Miguel", "Diaz", "1/1/25", 25);
    }

    public void SetupPlayerData(string screenName, string firstName, string lastName, string dateStarted, int score)
    {
        player = new PlayerData();

        player.screenName = screenName;
        player.firstName = firstName;
        player.lastName = lastName;
        player.dateStarted = dateStarted;
        player.score = score;

        string json = JsonUtility.ToJson(player);
        Debug.Log(json);
        StartCoroutine(PostPlayerData(json));
    }

    public IEnumerator PostPlayerData(string json)
    {
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            //Success
            Debug.Log($"Data Sent: {request.downloadHandler.text}");

            string newPlayerId = ExtractPlayerId(response);
            Debug.Log("New Player ID: " + newPlayerId);
            responseText.text = "Player Add Successfully!";
        }
        else
        {
            //Failed
            Debug.LogError($"Error sending data: {request.error}");
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
