using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int MinPuzzleRoom;
    [SerializeField] private int MaxPuzzleRoom;
    [SerializeField] private int MinRewardRoom;
    [SerializeField] private int MaxRewardRoom;
    private List<Room> GenericRoom;
    private List<Room> RewardRoom;
    private List<Room> PuzzleRoom;


    private Room StartRoom;
    private Room EndRoom;

    public void Start()
    {
        LoadNewBiome();
        Gen.Generate();
        //transform.localScale = new Vector3(10, 10, 10); //test scale
    }
    public void LoadNewBiome()
    {
        BiomeName = biome.biomeName;
        GenericRoom = biome.GenericRoom;
        RewardRoom = biome.RewardRoom;
        PuzzleRoom = biome.PuzzleRoom;
        StartRoom = biome.startRoom;
        EndRoom = biome.endRoom;
        Gen.AssignData(GridSize, MainPathLength, Width, Height, UnityEngine.Random.Range(MinPuzzleRoom, MaxPuzzleRoom), UnityEngine.Random.Range(MinRewardRoom, MaxRewardRoom), GenericRoom, RewardRoom, PuzzleRoom, StartRoom, EndRoom);
    }
}
