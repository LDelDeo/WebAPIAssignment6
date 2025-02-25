using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class UpdateData : MonoBehaviour
{
    string serverURL = "http://localhost:3000/updatePlayer";
    public TMP_InputField idInputField;
    public TMP_InputField screenNameInputField;
    public TMP_InputField firstNameInputField;
    public TMP_InputField lastNameInputField;
    public TMP_InputField dateStartedInputField;
    public TMP_InputField scoreInputField;
    public TMP_Text responseText;

    public void OnUpdateButtonClick()
    {
        string playerid = idInputField.text;
        string screenName = screenNameInputField.text;
        string firstName = firstNameInputField.text;
        string lastName = lastNameInputField.text;
        string dateStarted = dateStartedInputField.text;
        string scoreText = scoreInputField.text;

        if (string.IsNullOrEmpty(playerid) || string.IsNullOrEmpty(screenName) || string.IsNullOrEmpty(firstName) ||
            string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(dateStarted) ||
            string.IsNullOrEmpty(scoreText))
        {
            responseText.text = "Please fill in all fields!";
            return;
        }

        if (int.TryParse(scoreText, out int score))
        {
            UpdatePlayerData(playerid, screenName, firstName, lastName, dateStarted, score);
        }
        else
        {
            responseText.text = "Invalid score value!";
        }
    }

    public void UpdatePlayerData(string playerid, string screenName, string firstName, string lastName, string dateStarted, int score)
    {
        Player updatedPlayer = new Player
        {
            playerid = playerid,
            screenName = screenName,
            firstName = firstName,
            lastName = lastName,
            dateStarted = dateStarted,
            score = score
        };

        string json = JsonUtility.ToJson(updatedPlayer);
        StartCoroutine(SendUpdateRequest(json));
    }

    public IEnumerator SendUpdateRequest(string json)
    {
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseText.text = "Player Updated Successfully!";
        }
        else
        {
            responseText.text = $"Error updating player: {request.error}";
        }
    }
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
