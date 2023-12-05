using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBeheviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;

    public bool[] statusDoors;

    public void Start()
    {
        updateRoom(statusDoors);
    }

    void updateRoom(bool[] statusDoors)
    {
        for (int i = 0; i < statusDoors.Length; i++)
        {
            doors[i].SetActive(statusDoors[i]);
            walls[i].SetActive(!statusDoors[i]);
        }
    }

}
