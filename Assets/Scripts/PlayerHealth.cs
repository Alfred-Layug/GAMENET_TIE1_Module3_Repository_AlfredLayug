using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public float startingHealth = 100;
    public Image healthBar;
    public GameObject healthUIObject;
    public GameObject[] visibleGameObjects;

    private float currentHealth;

    public enum RaiseEventsCode
    {
        WhoFinishedEventCode = 0
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnWinEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnWinEvent;
    }

    void OnWinEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoFinishedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfWinner = (string)data[0];
            int viewId = (int)data[1];

            GameObject winnerUiText = DeathRacingGameManager.instance.winnerTextUi;
            winnerUiText.SetActive(true);
            winnerUiText.GetComponent<TextMeshProUGUI>().text = nickNameOfWinner + " IS THE WINNER!";
        }
    }

    private void Start()
    {
        DeathRacingGameManager.instance.players.Add(this.gameObject);

        currentHealth = startingHealth;

        healthBar.fillAmount = currentHealth / startingHealth;

        if (photonView.IsMine)
        {
            healthUIObject.SetActive(false);
        }
    }

    [PunRPC]
    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / startingHealth;

        if (currentHealth <= 0)
        {
            DoDeath();
        }
    }

    private void DoDeath()
    {
        foreach (GameObject go in visibleGameObjects)
        {
            go.SetActive(false);
        }
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<VehicleShooting>().enabled = false;
        DeathRacingGameManager.instance.EliminatePlayer(this.gameObject);
    }

    public void EndMatch()
    {
        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;

        // event data
        object[] data = new object[] { nickName, viewId };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoFinishedEventCode, data, raiseEventOptions, sendOptions);
    }
}