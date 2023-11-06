using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] rcSelectablePlayers;
    public GameObject[] drSelectablePlayers;

    public int playerSelectionNumber;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        playerSelectionNumber = 0;

        ActivatePlayer(playerSelectionNumber);
    }

    private void ActivatePlayer(int x)
    {
        if (networkManager.GameMode != null)
        {
            if (networkManager.GameMode == "rc")
            {
                foreach (GameObject go in rcSelectablePlayers)
                {
                    go.SetActive(false);
                }
                rcSelectablePlayers[x].SetActive(true);
            }
            else if (networkManager.GameMode == "dr")
            {
                foreach (GameObject go in drSelectablePlayers)
                {
                    go.SetActive(false);
                }
                drSelectablePlayers[x].SetActive(true);
            }
        }

        // Setting the player selection for the vehicle
        ExitGames.Client.Photon.Hashtable playerSelectionProperties = new ExitGames.Client.Photon.Hashtable() { {Constants.PLAYER_SELECTION_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperties);
    }

    public void GoToNextPlayer()
    {
        playerSelectionNumber++;

        if (playerSelectionNumber >= rcSelectablePlayers.Length)
        {
            playerSelectionNumber = 0;
        }

        ActivatePlayer(playerSelectionNumber);
    }

    public void GoToPrevPlayer()
    {
        playerSelectionNumber--;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = rcSelectablePlayers.Length - 1;
        }

        ActivatePlayer(playerSelectionNumber);
    }
}
