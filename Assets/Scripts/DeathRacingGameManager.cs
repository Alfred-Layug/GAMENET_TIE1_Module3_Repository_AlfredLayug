using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathRacingGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] vehiclePrefabs;
    public Transform[] startingPositions;
    public List<GameObject> players;
    public GameObject winnerTextUi;

    public static DeathRacingGameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log((int)playerSelectionNumber);

                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = startingPositions[actorNumber - 1].position;
                PhotonNetwork.Instantiate(vehiclePrefabs[(int)playerSelectionNumber].name, instantiatePosition, startingPositions[actorNumber - 1].rotation);
            }
        }
    }

    public void EliminatePlayer(GameObject player)
    {
        players.Remove(player);

        if (players.Count == 1)
        {
            players[0].GetComponent<PlayerHealth>().EndMatch();
        }
    }
}
