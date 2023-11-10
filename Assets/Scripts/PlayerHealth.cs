using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public float startingHealth = 100;
    public Image healthBar;
    public GameObject healthUIObject;

    private float currentHealth;

    private void Start()
    {
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

    }
}