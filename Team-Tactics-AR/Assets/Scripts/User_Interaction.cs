using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject whiteTeam = null;
    [SerializeField] private GameObject playerPositions = null;
    [SerializeField] private GameObject goalkeeperPositions = null;
    [SerializeField] private GameObject openMenu = null;

    public void OpenSelectedMenu()
    {
        if (whiteTeam.activeSelf)
        {
            goalkeeperPositions.SetActive(true);
        }
        else
        {
            playerPositions.SetActive(true);
        }
    }

    public void SwitchTeam()
    {
        if (!openMenu.activeSelf)
        {
            if (whiteTeam.activeSelf)
            {
                goalkeeperPositions.SetActive(true);
                playerPositions.SetActive(false);
            }
            else
            {
                goalkeeperPositions.SetActive(false);
                playerPositions.SetActive(true);
            }
        }
    }
}
