using UnityEngine;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;

public class WitchAttack : MonoBehaviour
{
    public GameObject firePoint;
    private Camera freeLookCamera;
    public List<GameObject> vfx = new List<GameObject>();
    public GameObject player;

    private GameObject effectToSpawn;
    private float timeToFire = 0;

    void Start()
    {
        effectToSpawn = vfx[0];
        freeLookCamera = FindObjectOfType<Camera>();

        if (freeLookCamera == null)
        {
            Debug.Log("FreeLook Camera non trovata nel tuo scenario!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            Vector3 firePointPosition = firePoint.transform.position;
            // Calcola il centro della visuale in spazio mondo
            Vector3 centerOfViewport = new Vector3(0.5f, 0.5f, 0);
            Vector3 targetPosition = freeLookCamera.GetComponent<Camera>().ViewportToWorldPoint(centerOfViewport);

            // Calcola la direzione dalla posizione del fuoco al centro della visuale
            Vector3 direction = targetPosition - firePointPosition;
            direction = new Vector3(direction.x, -direction.y, direction.z);
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction);
            Quaternion invertedRotation = rotation * Quaternion.Euler(0, 180, 0);
            player.transform.localRotation = Quaternion.Lerp(player.transform.rotation, invertedRotation, 1);
            // Istanzia l'effetto dal firePoint e applica la direzione
            StartCoroutine(SpawnVFXWithDelay(invertedRotation));
        }
    }

    void SpawnVFX(Quaternion rotation)
    {
        GameObject vfx;
        if (firePoint != null)
        {


            // Istanzia l'effetto dal firePoint e applica la rotazione
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, rotation);


        }
        else
        {
            Debug.Log("No fire point");
        }
    }
    IEnumerator SpawnVFXWithDelay(Quaternion rotation)
    {
        // Ritarda l'esecuzione 
        yield return new WaitForSeconds(0.3f);

        // Spawna l'effetto visivo
        SpawnVFX(rotation);
    }
}
