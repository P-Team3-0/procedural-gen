using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public GameObject room;
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public GameObject roomPrefab;
    public GameObject bossRoomPrefab;
    public Vector2 offsetBossRoom;

    public GameObject playerPrefab;
    public GameObject cameraPlayerPrefab;
    public CinemachineFreeLook freeLookCameraPrefab;

    public GameObject bossPrefab;
    public GameObject[] enemiesPrefabs = new GameObject[3];

    public int[] randomEnemyPositions = new int[3] { 10, -10, 0 };

    public Vector2Int size;
    public int startPos = 0;
    public Vector2 offset;

    private GameObject roomParent;

    public int seed = 42;

    List<Cell> board;

    void Start()
    {
        // Random.InitState(seed);
        MazeGenerator();
    }

    void Update()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentRoom = board[i + j * size.x];

                if (currentRoom.visited)
                {
                    // check if the player is inside a room, inside a batch of space
                    // if the player is inside a room, update the room status

                    roomParent = GameObject.Find("Room " + (i + j * size.x));
                    // get a child of the roomParent by name
                    GameObject floor = roomParent.transform.Find("Floor").gameObject;

                    if (floor == null)
                    {
                        Debug.Log("Floor is null");
                    }

                    BoxCollider collider = floor.GetComponent<BoxCollider>();

                    GameObject player = GameObject.FindWithTag("Player");


                    if (collider.bounds.Contains(player.transform.position))
                    {
                        Debug.Log("Player is inside the room " + (i + j * size.x));

                        int countEnemies = 0;

                        foreach (Transform child in roomParent.transform)
                        {

                            if (child.tag == "Enemy" || child.tag == "FlyingEnemy")
                            {
                                countEnemies++;
                            }
                        }
                        if (countEnemies == 0)
                        {
                            roomParent.GetComponent<RoomBehaviour>().updateRoom(currentRoom.status);

                            if (board[i + (size.y - 1) * size.x].status[0] && !board[i + (size.y - 1) * size.x].status[1] & !board[i + (size.y - 1) * size.x].status[2] && !board[i + (size.y - 1) * size.x].status[3])
                            {
                                GameObject room = GameObject.Find("Room " + ((size.x * size.y) - 1));
                                room.GetComponent<RoomBehaviour>().updateRoom(new bool[4] { true, true, false, false });
                            }
                            else if (!board[i + (size.y - 1) * size.x].status[0] && !board[i + (size.y - 1) * size.x].status[1] && !board[i + (size.y - 1) * size.x].status[2] && board[i + (size.y - 1) * size.x].status[3])
                            {
                                GameObject room = GameObject.Find("Room " + ((size.x * size.y) - 1));
                                room.GetComponent<RoomBehaviour>().updateRoom(new bool[4] { false, false, true, true });
                            }
                        }
                        else
                        {
                            Debug.Log("There are " + countEnemies + " enemies in the room");
                            roomParent.GetComponent<RoomBehaviour>().updateRoom(new bool[4] { false, false, false, false });
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentRoom = board[i + j * size.x];

                if (currentRoom.visited)
                {
                    // instantiate a gameobject (player) in the first room
                    Vector3 pos = new Vector3(-i * offset.x, 0, j * offset.y);
                    var newRoom = Instantiate(currentRoom.room, pos, Quaternion.identity);
                    newRoom.name = "Room " + (i + j * size.x);
                    newRoom.GetComponent<RoomBehaviour>().updateRoom(currentRoom.status);

                    if (i == 0 && j == 0)
                    {
                        Instantiate(playerPrefab, pos, Quaternion.identity);

                        GameObject player = GameObject.FindWithTag("Player");

                        //setting the freelookcamera parameters
                        freeLookCameraPrefab.Follow = player.transform;
                        freeLookCameraPrefab.LookAt = player.transform;
                        freeLookCameraPrefab.GetRig(0).LookAt = player.transform;
                        freeLookCameraPrefab.GetRig(1).LookAt = player.transform;
                        freeLookCameraPrefab.GetRig(2).LookAt=player.transform;

                        //instantiate camera and freelookCamera
                        Instantiate(cameraPlayerPrefab, pos, Quaternion.identity);
                        Instantiate(freeLookCameraPrefab, pos, Quaternion.identity);
                    }
                    else
                    {
                        int numberOfEnemies = Random.Range(0, 5);

                        for (int k = 0; k < numberOfEnemies; k++)
                        {
                            int randomEnemy = Random.Range(0, enemiesPrefabs.Length);
                            // if the tag is FlyingEnemy, the enemy will be spawned in the air
                            if (enemiesPrefabs[randomEnemy].tag == "FlyingEnemy")
                            {
                                Vector3 enemyPos = new Vector3(-i * offset.x + randomEnemyPositions[randomEnemy], 4.45f, j * offset.y + randomEnemyPositions[randomEnemy]);
                                // instantiate the enemy in the corrispective room as a child of the room
                                roomParent = GameObject.Find("Room " + (i + j * size.x));
                                Instantiate(enemiesPrefabs[randomEnemy], enemyPos, Quaternion.identity, roomParent.transform);
                            }
                            else
                            {
                                Vector3 enemyPos = new Vector3(-i * offset.x + randomEnemyPositions[randomEnemy], 0, j * offset.y + randomEnemyPositions[randomEnemy]);
                                // instantiate the enemy in the corrispective room as a child of the room
                                roomParent = GameObject.Find("Room " + (i + j * size.x));
                                Instantiate(enemiesPrefabs[randomEnemy], enemyPos, Quaternion.identity, roomParent.transform);
                            }
                        }
                    }
                }
            }
        }

        // search a room that has the south door open, only this opened
        // and instantiate the boss room in front of it (north)
        // or on the right of it (if the door on the left is opened)
        for (int i = 0; i < size.x; i++)
        {
            if (board[i + (size.y - 1) * size.x].status[0] && !board[i + (size.y - 1) * size.x].status[1] & !board[i + (size.y - 1) * size.x].status[2] && !board[i + (size.y - 1) * size.x].status[3])
            {
                Debug.Log("Room " + ((size.x * size.y) - 1) + " has the north door open");

                // search the Room + (size.x * size.y) - 1 gameobject and unlock the door
                GameObject room = GameObject.Find("Room " + ((size.x * size.y) - 1));
                room.GetComponent<RoomBehaviour>().unlockForTheBoss();

                Vector3 pos = new Vector3(-i * offset.x - offsetBossRoom.x, 0, (size.y - 1) * offset.y + offsetBossRoom.y);
                // rotate of 180° the room
                var newRoom = Instantiate(bossRoomPrefab, pos, Quaternion.Euler(0, 180, 0));

                //instatiate the boss
                // Instantiate(bossPrefab, pos, Quaternion.identity);

                break;
            }
            else if (!board[i + (size.y - 1) * size.x].status[0] && !board[i + (size.y - 1) * size.x].status[1] && !board[i + (size.y - 1) * size.x].status[2] && board[i + (size.y - 1) * size.x].status[3])
            {
                Debug.Log("Room " + ((size.x * size.y) - 1) + " has the right door open");

                // search the Room + (size.x * size.y) - 1 gameobject and unlock the door
                GameObject room = GameObject.Find("Room " + ((size.x * size.y) - 1));
                room.GetComponent<RoomBehaviour>().unlockForTheBoss_2();

                Vector3 pos = new Vector3(-i * offset.x - 96.05f, 0, (size.y - 1) * offset.y + 0.35f);
                // rotate of 180° the room
                var newRoom = Instantiate(bossRoomPrefab, pos, Quaternion.Euler(0, -270, 0));
                newRoom.name = "Boss Room";

                //instatiate the boss
                // Instantiate(bossPrefab, pos, Quaternion.identity);

                break;
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        // Debug.Log(board.Count);

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].room = roomPrefab;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }

            }

        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (cell - size.x >= 0 && !board[(cell - size.x)].visited)
        {
            neighbors.Add((cell - size.x));
        }

        //check down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbors.Add((cell + size.x));
        }

        //check right neighbor
        if ((cell + 1) % size.x != 0 && !board[(cell + 1)].visited)
        {
            neighbors.Add((cell + 1));
        }

        //check left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbors.Add((cell - 1));
        }

        return neighbors;
    }
}