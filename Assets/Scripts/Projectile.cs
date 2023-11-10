using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float projectileDamage;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.GetComponent<PlayerHealth>() != null && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("ReceiveDamage", RpcTarget.AllBuffered, projectileDamage);
        }
    }

    public void SetStats(Vector3 direction, float projectileSpeed, float damage)
    {
        projectileDamage = damage;
        transform.forward = direction;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * projectileSpeed * 2;
    }
}
