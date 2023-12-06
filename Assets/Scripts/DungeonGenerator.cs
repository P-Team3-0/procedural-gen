using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int dungeonWidth = 5;
    public int dungeonHeight = 5;

    public int offsetBetweenRoom = 18;

    public GameObject dungeonParent;
    private Transform dungeonParentTransform;
    public GameObject[] roomPrefabs;

    public float chanceToSpawnRoom = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        dungeonParentTransform = dungeonParent.transform;

        // Generate random dungeon layout
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                if (Random.value < chanceToSpawnRoom)
                {
                    int randomRoomIndex = Random.Range(0, roomPrefabs.Length);
                    GameObject roomPrefab = roomPrefabs[randomRoomIndex];

                    Vector3 roomPosition = new Vector3(x * offsetBetweenRoom, 0, y * offsetBetweenRoom);
                    Instantiate(roomPrefab, roomPosition, Quaternion.identity, dungeonParentTransform);
                }
            }
        }
    }

}
