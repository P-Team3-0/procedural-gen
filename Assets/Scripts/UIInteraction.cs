using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    public GameObject[] life;

    public GameObject gameover;

    public bool keepingDamage = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // for from the last
        for (int i = life.Length - 1; i >= 0; i--)
        {
            if (life[i].activeSelf == true && keepingDamage == true)
            {
                life[i].SetActive(false);
                keepingDamage = false;
            }
        }

        // if all life is gone, game over
        if (life[0].activeSelf == false && life[1].activeSelf == false && life[2].activeSelf == false)
        {
            Debug.Log("Game Over");
            gameover.SetActive(true);
        }

    }
}
