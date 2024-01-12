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

    public GameObject bossPrefab;
    public GameObject[] enemiesPrefabs = new GameObject[3];

    public int[] randomEnemyPositions = new int[3] { 10, -10, 0 };

    public Vector2Int size;
    public int startPos = 0;
    public Vector2 offset;

    public int seed = 42;

    List<Cell> board;

    void Start()
    {
        // Random.InitState(seed);
        MazeGenerator();
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
                        Instantiate(cameraPlayerPrefab, pos, Quaternion.identity);
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
                                Instantiate(enemiesPrefabs[randomEnemy], enemyPos, Quaternion.identity);
                            }
                            else
                            {
                                Vector3 enemyPos = new Vector3(-i * offset.x + randomEnemyPositions[randomEnemy], 0, j * offset.y + randomEnemyPositions[randomEnemy]);
                                Instantiate(enemiesPrefabs[randomEnemy], enemyPos, Quaternion.identity);
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
                newRoom.name = "Boss Room";
                newRoom.GetComponent<RoomBehaviour>().updateRoom(new bool[] { true, false, false, false });

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

                Vector3 pos = new Vector3(-i * offset.x - 80.25f, 0, (size.y - 1) * offset.y + 3f);
                // rotate of 180° the room
                var newRoom = Instantiate(bossRoomPrefab, pos, Quaternion.Euler(0, -270, 0));
                newRoom.name = "Boss Room";
                //TODO: fix the doors, now the boss room has only one door
                newRoom.GetComponent<RoomBehaviour>().updateRoom(new bool[] { true, false, false, false });

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