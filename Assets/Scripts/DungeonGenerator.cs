using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public Vector2 size = new Vector2(10, 10);
    public int startPos = 0;

    public GameObject room;
    public Vector2 offset;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        GridGenerator();
        drawDungeon();
    }

    void drawDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; i++)
            {
                // GameObject newRoom = Instantiate(room, new Vector3(i * offset.x, 0, j * offset.y), Quaternion.identity).getComponent<RoomBehaviour>();
                // newRoom.updateRoom(board[Mathf.FloorToInt(i * size.x + j)].status);
            }
        }
    }

    void GridGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; i++)
            {
                Cell cell = new Cell();
                board.Add(cell);
            }
        }

        int currentPos = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;
        while (k < 1000)
        {
            k++;

            board[currentPos].visited = true;

            List<int> neighbors = CheckNeighbors(currentPos);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentPos = path.Pop();
                }

            }
            else
            {
                path.Push(currentPos);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentPos)
                {
                    board[currentPos].status[1] = true;
                    currentPos = newCell;
                    board[newCell].status[3] = true;
                }
                else if (newCell < currentPos)
                {
                    board[currentPos].status[3] = true;
                    board[newCell].status[1] = true;
                }
                else if (newCell == currentPos + 1)
                {
                    board[currentPos].status[2] = true;
                    board[newCell].status[0] = true;
                }
                else if (newCell == currentPos - 1)
                {
                    board[currentPos].status[0] = true;
                    board[newCell].status[2] = true;
                }
            }
        }

        List<int> CheckNeighbors(int cell)
        {
            List<int> neighbors = new List<int>();

            //check up neighbor
            if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
            {
                neighbors.Add(Mathf.FloorToInt(cell - size.x));
            }

            //check down neighbor
            if (cell + size.x < size.x * size.y && !board[Mathf.FloorToInt(cell + size.x)].visited)
            {
                neighbors.Add(Mathf.FloorToInt(cell + size.x));
            }

            //check right neighbor
            if (cell + 1 < size.x * size.y && !board[Mathf.FloorToInt(cell + 1)].visited && (cell + 1) % size.x != 0)
            {
                neighbors.Add(Mathf.FloorToInt(cell + 1));
            }

            //check left neighbor
            if (cell - 1 >= 0 && !board[Mathf.FloorToInt(cell - 1)].visited && (cell - 1) % size.x != size.x - 1)
            {
                neighbors.Add(Mathf.FloorToInt(cell - 1));
            }

            return neighbors;
        }
    }
}
