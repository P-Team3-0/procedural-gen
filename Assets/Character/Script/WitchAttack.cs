using UnityEngine;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;

public class WitchAttack : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public GameObject player;

    private Camera freeLookCamera;
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
            Vector3 targetPosition = CalculateTargetPosition();

            // Calcola la direzione dalla posizione del fuoco al centro della visuale
            Vector3 direction = targetPosition - firePointPosition;
            direction.Normalize();

            // Applica la direzione al player
            player.transform.forward = direction;
            Vector3 euler = player.transform.rotation.eulerAngles;
            euler.x = 0f;
            player.transform.rotation = Quaternion.Euler(euler);

            // Istanzia l'effetto dal firePoint e applica la direzione
            StartCoroutine(SpawnVFXWithDelay(direction));
        }
    }

    Vector3 CalculateTargetPosition()
    {
        // Ottieni la posizione della telecamera
        Vector3 cameraPosition = freeLookCamera.transform.position;

        // Ottieni la direzione in cui la telecamera sta guardando
        Vector3 cameraDirection = freeLookCamera.transform.forward;

        // Aggiungi un offset alla coordinata Y (puoi regolare questo valore)
        float yOffset = 10.0f;  // Regola questo valore a seconda di quanto vuoi alzare la traiettoria
        Vector3 targetPosition = cameraPosition + cameraDirection * 100f + new Vector3(0, yOffset, 0);

        return targetPosition;
    }
    void SpawnVFX(Vector3 direction)
    {
        GameObject vfx;
        if (firePoint != null)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            // Istanzia l'effetto dal firePoint e applica la rotazione
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, rotation);
        }
        else
        {
            Debug.Log("No fire point");
        }
    }
    IEnumerator SpawnVFXWithDelay(Vector3 direction)
    {
        // Ritarda l'esecuzione 
        yield return new WaitForSeconds(0.3f);

        // Spawna l'effetto visivo
        SpawnVFX(direction);
    }
}