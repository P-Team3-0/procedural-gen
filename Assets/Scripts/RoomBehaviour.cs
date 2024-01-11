using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // down, up, right, left
    public GameObject[] doors;

    public void unlockForTheBoss()
    {
        doors[1].SetActive(true);
        walls[1].SetActive(false);
    }

    public void unlockForTheBoss_2()
    {
        doors[2].SetActive(true);
        walls[2].SetActive(false);
    }

    public void updateRoom(bool[] statusDoors)
    {
        for (int i = 0; i < statusDoors.Length; i++)
        {
            doors[i].SetActive(statusDoors[i]);
            walls[i].SetActive(!statusDoors[i]);
        }
    }

}