using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private RoomGen Gen;
    [SerializeField] private int GridSize = 1;
    [SerializeField] private int MainPathLength = 4;
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int SideRoomChance;
    [SerializeField] private Biome biome;
    [SerializeField] private String BiomeName;
    private List<Room> MainPath;
    private List<Room> SidePath;
    private Room StartRoom;
    private Room EndRoom;

    public void Start()
    {
        LoadNewBiome();
        Gen.Generate();

    }
    public void LoadNewBiome()
    {
        BiomeName = biome.biomeName;
        MainPath = biome.mainPath;
        SidePath = biome.sidePath;
        StartRoom = biome.startRoom;
        EndRoom = biome.endRoom;
        Gen.AssignData(GridSize, MainPathLength, Width, Height, MainPath, SidePath, StartRoom, EndRoom);
    }
}
