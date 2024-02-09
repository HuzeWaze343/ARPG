using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
    { 
    public class Cell
        {
        public bool visited = false;
        public bool[] status = new bool[4];// 0 = up, 1 = down, 2 = right, 3 = left
        }

    public GameObject[] rooms;

    public Vector2 size;
    public int startPos = 0;
    public int maxRooms;
    public int totalRooms;
    public Vector2 offset;

    List<Cell> board;

    private void Start()
        {
        MazeGenerator();
        }
    void GenerateDungeon() //generates the dungeon in world based on a previously generated maze
        {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                {
                Cell currentCell = board[Mathf.FloorToInt(x + y * size.x)];
                if(currentCell.visited)
                    GenerateRoom(x, y);
                }
        }
    void GenerateRoom(int x, int y) //generates a matching room in world based on a cells connections and coordinates
        {
        Cell c = board[Mathf.FloorToInt(x + y * size.x)];
        
        string roomName = "";
        if (c.status[0]) roomName += "N";
        if (c.status[2]) roomName += "E";
        if (c.status[1]) roomName += "S";
        if (c.status[3]) roomName += "W";

        for (int i = 0; i < rooms.Length; i++)
            {
            if (rooms[i].name == roomName)
                {
                GameObject room = Instantiate(rooms[i], new Vector2(x * offset.x, -y * offset.y), Quaternion.identity, transform);
                room.name = $"Room {x}-{y}";

                //destroy the room generator in the starting room so that it doesnt spawn enemies
                if (x == 0 && y == 0)
                    {
                    RoomGenerator roomGen = room.GetComponentInChildren<RoomGenerator>();
                    Destroy(roomGen.gameObject);
                    }
                break;
                }
            }
        }
    void MazeGenerator() //generates a maze of cells and their connections to eachother
        {
        //initialize the board with blank cells
        board = new List<Cell>();
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                board.Add(new Cell());

        
        totalRooms = 0;
        int currentCell = startPos;
        Stack<int> path = new Stack<int>(); //the history of the path we have taken to get to the current cell
        int k = 0; //how many times we have looped

        while (k < 1000)
            {
            k++;

            board[currentCell].visited = true;

            if (currentCell == board.Count - 1 || totalRooms >= maxRooms - 1) //stop generating more cells if the dungeon is already large enough or has reached the opposite side of the board
                break;

            List<int> neighbours = CheckNeighbours(currentCell); //check if the neighbouring cells are invalid or already visited

            if (neighbours.Count == 0)
                {
                if (path.Count == 0) //if theres no neighbours and no pathway, stop trying to generate cells
                    break;
                else
                    currentCell = path.Pop(); //if theres no neighbours but still cells on the pathway, backtrack along the path to continue generating
                }
            else
                {
                path.Push(currentCell); //push the currentcell onto the pathway

                int newCell = neighbours[Random.Range(0, neighbours.Count)]; //pick a random, valid, neighbouring cell

                if(newCell>currentCell) //new cell is right or down
                    {
                    if(newCell - 1 == currentCell) //new cell is right
                        {
                        board[currentCell].status[2] = true; //right path is open
                        currentCell = newCell;
                        board[currentCell].status[3] = true; //new cells left path is open
                        }
                    else //new cell is down
                        {
                        board[currentCell].status[1] = true; //down path is open
                        currentCell = newCell;
                        board[currentCell].status[0] = true; //new cells up path is open
                        }
                    }
                else //new cell is up or left
                    {
                    if (newCell + 1 == currentCell) //new cell is left
                        {
                        board[currentCell].status[3] = true; //left path is open
                        currentCell = newCell;
                        board[currentCell].status[2] = true; //new cells right path is open
                        }
                    else //new cell is up
                        {
                        board[currentCell].status[0] = true; //up path is open
                        currentCell = newCell;
                        board[currentCell].status[1] = true; //new cells down path is open
                        }
                    }

                }
            totalRooms++;
            }
        GenerateDungeon();//once were done generating the maze, turn it into a dungeon in the game
        }
    List<int> CheckNeighbours(int cell) //returns a list of the all neighbouring cells that are within the bounds of the board and havent already been visited
        {
        List<int> neighbours = new List<int>();


        //--MATRIX FOR CELL POSITIONS
        //              cell - size.x
        //cell - 1      cell | (x+y*size.x)     cell + 1
        //              cell + size.x

        //check all neighbouring cells to see if they are within the bounds of the board and that they havent already been visited
        //check north neighbour
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
            neighbours.Add(Mathf.FloorToInt(cell - size.x));

        //check south neighbour
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
            neighbours.Add(Mathf.FloorToInt(cell + size.x));

        //check east neighbour
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
            neighbours.Add(Mathf.FloorToInt(cell + 1));

        //check west neighbour
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
            neighbours.Add(Mathf.FloorToInt(cell - 1));


        return neighbours;
        }

    }
