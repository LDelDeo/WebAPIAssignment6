using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class DeletePlayer : MonoBehaviour
{
    public TMP_InputField playerIDInput;  // Reference to the input field where the user types the player ID
    public TMP_Text responseText;
    //private string serverURL = "http://localhost:3000/player/";  // Replace with your actual server URL
    private string serverURL = "https://webapiassignment6.onrender.com/player/";

    public void DeleteUser()
    {
        string playerID = playerIDInput.text.Trim();
        if (string.IsNullOrEmpty(playerID))
        {
            Debug.LogError("Player ID cannot be empty!");
            return;
        }

        StartCoroutine(DeletePlayerFromDB(playerID));
    }

    IEnumerator DeletePlayerFromDB(string playerID)
    {
        string deleteURL = serverURL + playerID;
        UnityWebRequest request = UnityWebRequest.Delete(deleteURL);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Player deleted successfully: " + request.downloadHandler.text);
            responseText.text = "Player Deleted Successfully!";
            responseText.color = Color.green;
            playerIDInput.text = "";
        }
        else
        {
            //Debug.LogError("Error deleting player: " + request.error);
            responseText.text = "Error Deleting Player/Player Not Found";
            responseText.color = Color.red;
        }
    }
}
