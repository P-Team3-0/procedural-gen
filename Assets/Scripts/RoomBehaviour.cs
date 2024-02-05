using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // down, up, right, left
    public GameObject[] doors; // down, up, right, left
    public GameObject[] lineWalls; // down, up, right, left

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

    public void updateRoom(bool[] statusDoors)
    {
        for (int i = 0; i < statusDoors.Length; i++)
        {
            if (statusDoors[i])
            {
                entraces[i].GetComponent<Animator>().SetTrigger("OpenDoor");
            }
            else
            {
                entraces[i].GetComponent<Animator>().SetTrigger("CloseDoor");
            }
        }
    }

    public void removeWalls(string direction)
    {
        switch (direction)
        {
            case "down":
                lineWalls[0].SetActive(false);
                break;
            case "up":
                lineWalls[1].SetActive(false);
                break;
            case "right":
                lineWalls[2].SetActive(false);
                break;
            case "left":
                lineWalls[3].SetActive(false);
                break;
        }
    }

}
