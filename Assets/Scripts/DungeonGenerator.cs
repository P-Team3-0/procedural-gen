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

    public GameObject player;
    public Vector2 offsetBossRoom;

    public Vector2Int size;
    public int startPos = 0;
    public Vector2 offset;

    public int seed = 42;

    List<Cell> board;

    void Start()
    {
        Random.InitState(seed);
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
                        Instantiate(player, pos, Quaternion.identity);
                    }
                }
            }
        }

        // search a room that has the south door open, only this opened
        // and instantiate the boss room there
        // break the all loops when the room is found
        // TODO: fai attaccare la stanza del boss anche a room finali a giù destra e sinistra
        for (int i = 0; i < size.x; i++)
        {
            if (board[i + (size.y - 1) * size.x].status[0] && !board[i + (size.y - 1) * size.x].status[1] && !board[i + (size.y - 1) * size.x].status[2] && !board[i + (size.y - 1) * size.x].status[3])
            {
                // search the Room + (size.x * size.y) - 1 gameobject and unlock the door
                GameObject room = GameObject.Find("Room " + ((size.x * size.y) - 1));
                room.GetComponent<RoomBehaviour>().unlockForTheBoss();


                Vector3 pos = new Vector3(-i * offset.x - offsetBossRoom.x, 0, (size.y - 1) * offset.y + offsetBossRoom.y);
                var newRoom = Instantiate(bossRoomPrefab, pos, Quaternion.identity);
                newRoom.name = "Boss Room";
                newRoom.GetComponent<RoomBehaviour>().updateRoom(new bool[] { false, true, false, false });
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