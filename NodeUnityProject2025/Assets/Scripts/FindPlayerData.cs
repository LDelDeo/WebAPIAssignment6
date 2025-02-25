using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class FindPlayerData : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_InputField playerid;
    public FetchData fetch;

 public void SearchForPlayer()
 {
     if (!string.IsNullOrEmpty(name.text) && !string.IsNullOrEmpty(playerid.text))
     {
         fetch.SetupPlayerSearchData(name.text, playerid.text);
     }
     else
     {
         Debug.Log("Please fill both name and player ID.");
     }
 }


}
