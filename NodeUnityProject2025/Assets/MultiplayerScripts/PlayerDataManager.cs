using UnityEngine;

public static class PlayerDataManager
{
    public static string PlayerID;
    public static string ScreenName;
    public static string FirstName;
    public static string LastName;
    public static string DateStarted;
    public static int Score;

    // Method to store data
    public static void SetPlayerData(PlayerSearch.Player player)
    {
        PlayerID = player.playerid;
        ScreenName = player.screenName;
        FirstName = player.firstName;
        LastName = player.lastName;
        DateStarted = player.dateStarted;
        Score = player.score;
    }
}
