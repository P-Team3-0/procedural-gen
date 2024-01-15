using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firePoint;

    public List<GameObject> vfx = new List<GameObject>();
    public RotateToMouse rotateToMouse;

    private GameObject effectToSpawn;
    private float timeToFire = 0;

    int AttackHash;
    void Start()
    {
        effectToSpawn = vfx[0];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            // Crea una coroutine e ritarda l'esecuzione di SpawnVFX per 2 secondi
            StartCoroutine(SpawnVFXWithDelay());
        }


    }

    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            if (rotateToMouse != null)
            {
                vfx.transform.localRotation = rotateToMouse.GetRotation();
            }
        }
        else
        {
            Debug.Log("no fire point");
        }
    }
    IEnumerator SpawnVFXWithDelay()
    {
        // Ritarda l'esecuzione per 2 secondi
        yield return new WaitForSeconds(0.3f);

        // Spawna l'effetto visivo
        SpawnVFX();
    }
}
