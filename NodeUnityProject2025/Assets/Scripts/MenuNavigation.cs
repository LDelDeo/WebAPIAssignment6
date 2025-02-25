using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playerList;
    public GameObject addPlayer;
    public GameObject updatePlayer;
    public GameObject deletePlayer;
    public GameObject findPlayer;

    public void OpenList()
    {
        mainMenu.SetActive(false);
        playerList.SetActive(true);
    }

    public void CloseList()
    {
        mainMenu.SetActive(true);
        playerList.SetActive(false);
    }

    public void OpenAdd()
    {
        mainMenu.SetActive(false);
        addPlayer.SetActive(true);
    }

    public void CloseAdd()
    {
        mainMenu.SetActive(true);
        addPlayer.SetActive(false);
    }

    public void OpenUpdate()
    {
        mainMenu.SetActive(false);
        updatePlayer.SetActive(true);
    }

    public void CloseUpdate()
    {
        mainMenu.SetActive(true);
        updatePlayer.SetActive(false);
    }

    public void OpenDelete()
    {
        mainMenu.SetActive(false);
        deletePlayer.SetActive(true);
    }

    public void CloseDelete()
    {
        mainMenu.SetActive(true);
        deletePlayer.SetActive(false);
    }

    public void OpenFind()
    {
        mainMenu.SetActive(false);
        findPlayer.SetActive(true);
    }

    public void CloseFind()
    {
        mainMenu.SetActive(true);
        findPlayer.SetActive(false);
    }
}
