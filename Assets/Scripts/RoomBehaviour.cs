using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // down, up, right, left
    public bool[] statusWalls = new bool[4]; // down, up, right, left
    public GameObject[] doors; // down, up, right, left
    public bool[] statusDoors = new bool[4]; // down, up, right, left

    public GameObject[] entraces; // down, up, right, left

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

    public void updateRoom(bool[] status)
    {
        Debug.Log("Updating room");
        for (int i = 0; i < status.Length; i++)
        {
            statusDoors[i] = status[i];
            // doors[i].SetActive(status[i]);

            if (status[i])
            {
                entraces[i].GetComponent<Animator>().SetTrigger("OpenDoor");
            }
            else
            {
                entraces[i].GetComponent<Animator>().SetTrigger("CloseDoor");
            }
        }
    }
}
