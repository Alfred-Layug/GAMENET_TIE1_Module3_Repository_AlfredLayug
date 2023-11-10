using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private float speed = 25;
    [SerializeField] private float flyingSpeed = 20;
    [SerializeField] private float rotationSpeed = 200;
    [SerializeField] private float currentSpeed = 0;
    private Vector3 moveDirection;

    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            Fly();
        }
    }

    private void Move()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        currentSpeed = translation;

        transform.Rotate(0, rotation, 0);
    }

    private void Fly()
    {
        moveDirection = Vector3.up * flyingSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, moveDirection.y, 0);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, -moveDirection.y, 0);
        }
    }
}
