using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendPlayerData : MonoBehaviour
{
    public TMP_InputField screenName;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField dateStarted;
    public TMP_InputField score;
    public PostData post;

    public void SendData()
    {
        if (screenName.text != "" && firstName.text != "" && lastName.text != "" && dateStarted.text != "" && score.text != "")
        {
            int scoreData = int.Parse(score.text);
            post.SetupPlayerData(screenName.text, firstName.text, lastName.text, dateStarted.text, scoreData);
        }
    }
    
}
