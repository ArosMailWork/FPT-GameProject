using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class RoomGen : MonoBehaviour
{
    // The size of our world in grid cells.
    private int GridSize = 1;
    private int Size = 100;
    private int MainPathLength = 4;
    private int Width;
    private int Height;
    private int SideRoomChance = 5;
    private List<Room> MainPath;
    private List<Room> SidePath;
    private Room StartRoom;
    private Room EndRoom;
    // A 2D array that will store our collapsed tiles so we can reference them later.
    private Node[,] _grid;
    //List containing all possible nodes
    private List<Node> Nodes = new List<Node>();
    private List<GameObject> allRoom = new List<GameObject>();
    private Node origin = new Node();

    //An array of offset to make it easier to check neighbours
    // without duplicating code.
    private List<Vector2Int> sideOffsets = new List<Vector2Int>{
        new Vector2Int(0,1), //Top
        new Vector2Int(0,-1), //Bottom
        new Vector2Int(1,0), //Right
        new Vector2Int(-1,0) //Left
    };
    private List<Vector2Int> mainOffsets = new List<Vector2Int>{
        new Vector2Int(0,1), //Top
        new Vector2Int(1,0), //Right
        //new Vector2Int(-1,0) //Left
    };
    public void AssignData(int GridSize, int MainPathLength, int Width, int Height, List<Room> mainPath, List<Room> sidePath, Room StartRoom, Room EndRoom)
    {
        this.GridSize = GridSize;
        this.MainPathLength = MainPathLength;
        this.Width = Width;
        this.Height = Height;
        this.MainPath = mainPath;
        this.SidePath = sidePath;
        this.StartRoom = StartRoom;
        this.EndRoom = EndRoom;

    }


    public void Generate()
    {
        _grid = new Node[Width, Height];
        //First Pass 
        GenerateMainPath();
        //Instantiate the origin point

        GenerateSidePath();
        origin.ConnectedPos = CheckConnectedDirection(origin.currentPos, sideOffsets);
        Debug.Log(origin.ConnectedPos.Count);
        GameObject o = GameObject.Instantiate(origin.room.Prefab, new Vector3(origin.currentPos.x * GridSize, 0f, origin.currentPos.y * GridSize), Quaternion.identity);
        o.GetComponent<RoomController>().SetConnector(origin.ConnectedPos);
        //o.transform.localScale = new Vector3(Size, Size, Size);
        o.transform.SetParent(this.transform);
        allRoom.Add(o);
        //Second Pass

        //Instantiate the main path and the side path
        foreach (Node n in origin.nodes)
        {
            n.ConnectedPos = CheckConnectedDirection(n.currentPos, sideOffsets);
            GameObject g = GameObject.Instantiate(n.room.Prefab, new Vector3(n.currentPos.x * GridSize, 0f, n.currentPos.y * GridSize), Quaternion.identity);
            g.GetComponent<RoomController>().SetConnector(n.ConnectedPos);
            g.transform.SetParent(this.transform);
            //g.transform.localScale = new Vector3(Size, Size, Size);
            allRoom.Add(g);
            if (n.nodes.Count > 0)
            {
                foreach (Node s in n.nodes)
                {
                    s.ConnectedPos = CheckConnectedDirection(s.currentPos, sideOffsets);
                    GameObject g1 = GameObject.Instantiate(s.room.Prefab, new Vector3(s.currentPos.x * GridSize, 0f, s.currentPos.y * GridSize), Quaternion.identity);
                    g1.GetComponent<RoomController>().SetConnector(s.ConnectedPos);
                    //g1.transform.localScale = new Vector3(Size, 1, Size);
                    g1.transform.SetParent(this.transform);
                    allRoom.Add(g1);
                }
            }
        }

    }
    public void Regenerate()
    {
        Clear();
        _grid = new Node[Width, Height];
        //First Pass 
        GenerateMainPath();
        //Instantiate the origin point
        allRoom.Add(GameObject.Instantiate(origin.room.Prefab, new Vector3(origin.currentPos.x * GridSize, 0f, origin.currentPos.y * GridSize), Quaternion.identity));
        //Second Pass
        GenerateSidePath();
        //Instantiate the main path and the side path
        foreach (Node n in origin.nodes)
        {
            CheckConnectedDirection(n.currentPos, mainOffsets);
            allRoom.Add(GameObject.Instantiate(n.room.Prefab, new Vector3(n.currentPos.x * GridSize, 0f, n.currentPos.y * GridSize), Quaternion.identity));
            if (n.nodes.Count > 0)
            {
                foreach (Node s in n.nodes)
                {
                    allRoom.Add(GameObject.Instantiate(s.room.Prefab, new Vector3(s.currentPos.x * GridSize, 0f, s.currentPos.y * GridSize), Quaternion.identity));
                }
            }
        }
    }
    public void Clear()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int y = 0; i < _grid.GetLength(1); y++)
            {
                _grid[i, y] = null;
            }
        }
        for (int i = 0; i < allRoom.Count; i++)
        {
            Destroy(allRoom[i]);
        }
        origin = null;

    }
    private void GenerateMainPath()
    {
        origin.nodes.Clear();
        Vector2Int pointer = new Vector2Int(Width / 2, Height / 2);
        //Vector2Int pointer = new Vector2Int(0, 0);
        origin.currentPos = pointer;
        origin.room = StartRoom;
        _grid[pointer.x, pointer.y] = origin;
        for (int i = 1; i <= MainPathLength; i++)
        {
            List<Vector2Int> ValidDir = CheckAvailableDirection(pointer, mainOffsets);
            if (ValidDir.Count > 0)
            {
                Vector2Int rV = ValidDir[Random.Range(0, ValidDir.Count)];
                pointer.x += rV.x;
                pointer.y += rV.y;
                Room p = MainPath[Random.Range(0, MainPath.Count)];
                if (i == MainPathLength)
                {
                    p = EndRoom;
                }
                Node n = new Node()
                {
                    currentPos = pointer,
                    room = p
                };
                _grid[pointer.x, pointer.y] = n;
                origin.nodes.Add(n);

            }

        }
    }

    public void GenerateSidePath()
    {
        foreach (Node n in origin.nodes)
        {
            int roll = Random.Range(2, 11);
            //Debug.Log("Roll :" + roll);
            int chances = SideRoomChance;
            Vector2Int pointer = n.currentPos;
            //Debug.Log("Chances :" + chances);
            while (chances >= roll)
            {
                List<Vector2Int> ValidDir = CheckAvailableDirection(pointer, sideOffsets);
                //Debug.Log("aaaa" + offsets.Count);
                //Debug.Log(ValidDir.Count);
                //n.ConnectedPos = ValidDir;
                if (ValidDir.Count > 0)
                {
                    Vector2Int rV = ValidDir[Random.Range(0, ValidDir.Count)];
                    pointer.x += rV.x;
                    pointer.y += rV.y;
                    Node s = new Node()
                    {
                        currentPos = pointer,
                        room = SidePath[Random.Range(0, SidePath.Count)]


                        //Prefab = SidePrefab
                    };
                    _grid[pointer.x, pointer.y] = s;
                    n.nodes.Add(s);
                    //Debug.Log("Chances :" + chances);
                }
                else
                {
                    break;
                }
                chances /= 2;

            }

        }
    }
    private void GenerateConnector()
    {

    }
    private bool IsInsideGrid(Vector2Int v2int)
    {
        if (v2int.x > -1 && v2int.x < Width && v2int.y > -1 && v2int.y < Height)
        {
            return true;
        }
        return false;
    }
    private List<Vector2Int> CheckConnectedDirection(Vector2Int v2int, List<Vector2Int> offsets)
    {
        List<Vector2Int> ValidDir = offsets.ToList();


        for (int i = ValidDir.Count - 1; i >= 0; i--)
        {
            Vector2Int neighbour = new Vector2Int(v2int.x + ValidDir[i].x, v2int.y + ValidDir[i].y);
            if (IsInsideGrid(neighbour) && _grid[neighbour.x, neighbour.y] == null)
            {
                ValidDir.RemoveAt(i);
            }
            // if (!IsInsideGrid(neighbour) || _grid[neighbour.x, neighbour.y] == null)
            // {
            //     ValidDir.RemoveAt(i);
            // }
        }
        //Debug.Log(ValidDir.Count);
        return ValidDir;
    }
    private List<Vector2Int> CheckAvailableDirection(Vector2Int v2int, List<Vector2Int> offsets)
    {
        List<Vector2Int> ValidDir = offsets.ToList();


        for (int i = ValidDir.Count - 1; i >= 0; i--)
        {
            Vector2Int neighbour = new Vector2Int(v2int.x + ValidDir[i].x, v2int.y + ValidDir[i].y);
            if (!IsInsideGrid(neighbour) || _grid[neighbour.x, neighbour.y] != null)
            {
                ValidDir.RemoveAt(i);
            }
        }
        //Debug.Log(ValidDir.Count);
        return ValidDir;
    }
    // private List<Vector2Int> CheckConnectedRoom(Vector2Int v2int)
    // {
    //     List<Vector2Int> ValidDir = offsets.ToList();
    //     for (int i = ValidDir.Count - 1; i >= 0; i--)
    //     {
    //         Vector2Int neighbour = new Vector2Int(v2int.x + ValidDir[i].x, v2int.y + ValidDir[i].y);
    //         if (!IsInsideGrid(neighbour) || _grid[neighbour.x, neighbour.y] == null)
    //         {
    //             ValidDir.RemoveAt(i);
    //         }
    //     }
    //     return ValidDir;
    // }
}
