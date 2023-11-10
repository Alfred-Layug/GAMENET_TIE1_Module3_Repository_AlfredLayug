using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleShooting : MonoBehaviourPunCallbacks
{
    public GameObject projectilePrefab;
    public Transform nozzlePosition;
    public DeathRacingPlayer deathRacingPlayer;
    public Camera camera;
    public LineRenderer lineRenderer;

    private float laserFireRate;
    private float projectileFireRate;
    private bool canShootLaser;
    private bool canShootProjectile;

    private void Start()
    {
        this.camera = transform.Find("Camera").GetComponent<Camera>();
        laserFireRate = deathRacingPlayer.laserFireRate;
        projectileFireRate = deathRacingPlayer.projectileFireRate;
        canShootLaser = true;
        canShootProjectile = true;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.Mouse0) && canShootLaser)   //Left click to shoot laser
            {
                photonView.RPC("ShootLaser", RpcTarget.All);
                canShootLaser = false;
                StartCoroutine(LaserTimer());
            }
            else if (Input.GetKey(KeyCode.Mouse1) && canShootProjectile)  //Right click to shoot projectiles
            {
                photonView.RPC("ShootProjectile", RpcTarget.All);
                canShootProjectile = false;
                StartCoroutine(ProjectileTimer());
            }
        }
    }

    [PunRPC]
    public void ShootLaser()
    {
        RaycastHit raycastHit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.6f));

        if (Physics.Raycast(ray, out raycastHit, 100))
        {
            if (lineRenderer.enabled == false)
            {
                lineRenderer.enabled = true;
            }

            lineRenderer.SetPosition(0, nozzlePosition.position);
            lineRenderer.SetPosition(1, raycastHit.point);

            if (raycastHit.collider.GetComponent<PlayerHealth>() != null && raycastHit.collider.GetComponent<PhotonView>().IsMine)
            {
                raycastHit.collider.GetComponent<PhotonView>().RPC("ReceiveDamage", RpcTarget.AllBuffered, deathRacingPlayer.laserDamage);
            }

            StartCoroutine(LaserDisappearTimer());
        }
    }

    [PunRPC]
    public void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, nozzlePosition.position, Quaternion.identity);
        projectileGO.GetComponent<Projectile>().SetStats(nozzlePosition.forward, deathRacingPlayer.projectileSpeed, deathRacingPlayer.projectileDamage);
    }

    private IEnumerator LaserTimer()
    {
        yield return new WaitForSeconds(laserFireRate);
        canShootLaser = true;
    }

    private IEnumerator LaserDisappearTimer()
    {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.enabled = false;
    }

    private IEnumerator ProjectileTimer()
    {
        yield return new WaitForSeconds(projectileFireRate);
        canShootProjectile = true;
    }
}
