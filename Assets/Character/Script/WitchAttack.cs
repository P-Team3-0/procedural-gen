using UnityEngine;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;

public class WitchAttack : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public GameObject player;
    public string layer="Enemy";
    private Camera freeLookCamera;
    private GameObject spell;
    private GameObject golemSpell;
    int countEnemies;
    private float timeToFire = 0;

    void Start()
    {
        spell = vfx[0];
        golemSpell = vfx[1];
        freeLookCamera = FindObjectOfType<Camera>();
        if (freeLookCamera == null)
        {
            Debug.Log("FreeLook Camera non trovata nel tuo scenario!");
        }
        countEnemies = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / spell.GetComponent<ProjectileMove>().fireRate;
            Vector3 direction= AttackFunction();
            if (countEnemies != 1)
            {
                LayerMask layerMaskToSearch = LayerMask.NameToLayer(layer);
                GameObject[] objectsWithLayer = FindObjectsWithLayer(layerMaskToSearch);
                foreach (var obj in objectsWithLayer)
                {
                    LifeManager lifeManager = obj.GetComponent<LifeManager>();
                    if (lifeManager != null && lifeManager.health > 0)
                        countEnemies++; 
                }  
            }
            if (countEnemies == 1)
                spell = vfx[2];
            else
                countEnemies = 0;           
            // Istanzia l'effetto dal firePoint e applica la direzione
            StartCoroutine(DelaySX(direction));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 direction = AttackFunction();
            // Istanzia l'effetto dal firePoint e applica la direzione
            StartCoroutine(DelayDX(direction));
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

    Vector3 AttackFunction()
    {
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
        return direction;
    }
    void SpawnSpell(Vector3 direction)
    {
        GameObject vfx;
        if (firePoint != null)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            // Istanzia l'effetto dal firePoint e applica la rotazione
            vfx = Instantiate(spell, firePoint.transform.position, rotation);
        }
        else
        {
            Debug.Log("No fire point");
        }
    }
    void SpawnGolemSpell(Vector3 direction)
    {
        GameObject vfx;
        if (firePoint != null)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            // Istanzia l'effetto dal firePoint e applica la rotazione
            vfx = Instantiate(golemSpell, firePoint.transform.position, rotation);
        }
        else
        {
            Debug.Log("No fire point");
        }
    }
    IEnumerator DelaySX(Vector3 direction)
    {
        // Ritarda l'esecuzione 
        yield return new WaitForSeconds(0.3f);

        // Spawna l'effetto visivo
        SpawnSpell(direction);
    }
    IEnumerator DelayDX(Vector3 direction)
    {
        // Ritarda l'esecuzione 
        yield return new WaitForSeconds(0.3f);

        // Spawna l'effetto visivo
        SpawnGolemSpell(direction);
    }

    private GameObject[] FindObjectsWithLayer(LayerMask layer)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        var objectsWithLayer = new System.Collections.Generic.List<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.layer == layer)
            {
                objectsWithLayer.Add(obj);
            }
        }
        return objectsWithLayer.ToArray();
    }
}