using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    // The size of our world in grid cells.
    [SerializeField] private int GridSize = 1;
    [SerializeField] private int MainPathLength = 4;
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    private Node[,] _grid;
    private List<Vector2Int> offsets = new List<Vector2Int>{
        new Vector2Int(0,1), //Top
        new Vector2Int(0,-1), //Bottom
        new Vector2Int(1,0), //Right
        new Vector2Int(-1,0) //Left
    };
    // Start is called before the first frame update
    void Start()
    {
        _grid = new Node[Width, Height];
        //First Pass 
        GenerateMainPath();
    }
    private void GenerateMainPath()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
