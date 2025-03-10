using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public TMP_Text playerName;

    void Start()
    {
        playerName.text = $"{PlayerDataManager.ScreenName}\n";
    }

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

