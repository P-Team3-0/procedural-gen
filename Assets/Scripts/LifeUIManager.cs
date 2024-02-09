using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    private GameObject[] lives = new GameObject[3];

    public GameObject pgGameObject;

    public int health;

    void Start()
    {
        // generate 3 hearts at the start of the game
        for (int i = 0; i < 3; i++)
        {
            GameObject newheartPrefab = Instantiate(heartPrefab, transform);
            newheartPrefab.transform.position += new Vector3((i * 50) + 45f, 15f, 0);
            lives[i] = newheartPrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pgGameObject)
        {
            pgGameObject = GameObject.FindWithTag("Player");
        }
        health = pgGameObject.GetComponent<LifeManager>().health;
        Debug.Log(health);
        if (health == 2)
        {
            Destroy(lives[2]);
        }
        else if (health == 1)
        {
            Destroy(lives[1]);
        }
        else if (health == 0)
        {
            Destroy(lives[0]);
        }
    }
}
