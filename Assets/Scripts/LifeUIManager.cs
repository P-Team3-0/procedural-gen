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
            newheartPrefab.transform.position += new Vector3((i * 100), 0, 0);
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

        // if the player has no health, restart the scene after 5 seconds
        if (health == 0)
        {
            StartCoroutine(RestartScene());
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(7);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
