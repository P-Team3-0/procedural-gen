using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    private GameObject[] lives;

    public GameObject pgGameObject;

    void Start()
    {
        // generate 3 hearts at the start of the game
        for (int i = 0; i < 3; i++)
        {
            GameObject newheartPrefab = Instantiate(heartPrefab, transform);
            newheartPrefab.transform.position += new Vector3(i * 60.5f, 0, 0);
            lives[i] = newheartPrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pgGameObject.GetComponent<LifeManager>().health == 2)
        {
            Destroy(lives[2]);
        }
        else if (pgGameObject.GetComponent<LifeManager>().health == 1)
        {
            Destroy(lives[1]);
        }
        else if (pgGameObject.GetComponent<LifeManager>().health == 0)
        {
            Destroy(lives[0]);
        }
    }
}
