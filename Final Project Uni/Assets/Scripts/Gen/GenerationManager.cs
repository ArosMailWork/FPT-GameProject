using System;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] private RoomGen Gen;
    [SerializeField] private int GridSize = 1;
    [SerializeField] private int MainPathLength = 4;
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
    private List<Room> BossRoom;
    private bool haveBoss;
    private Room StartRoom;
    private Room ExitRoom;

    public void Start()
    {
        //Generate();
        //player.transform.position = Gen.s
        player = GameObject.Find("Player");

    }
    public void Generate()
    {
        Gen.Clear();
        AssignDataToRoomGen();
        Gen.Generate();
        player.transform.position = Gen.spawn;
    }


    public void AssignDataToRoomGen()
    {
        Gen.AssignData(GridSize, MainPathLength, UnityEngine.Random.Range(MinPuzzleRoom, MaxPuzzleRoom), UnityEngine.Random.Range(MinRewardRoom, MaxRewardRoom), GenericRoom, RewardRoom, PuzzleRoom, BossRoom, haveBoss, StartRoom, ExitRoom);
    }
    public void LoadBiomeData(Biome biome)
    {
        BiomeName = biome.biomeName;
        GridSize = biome.GridSize;
        GenericRoom = biome.GenericRoom;
        RewardRoom = biome.RewardRoom;
        PuzzleRoom = biome.PuzzleRoom;
        BossRoom = biome.BossRoom;
        StartRoom = biome.startRoom;
        ExitRoom = biome.exitRoom;

    }
    public void LoadFloorData(Floor newFloor)
    {

        MainPathLength = newFloor.MainPathLength;
        SideRoomChance = newFloor.SideRoomChance;
        MinPuzzleRoom = newFloor.MinPuzzleRoom;
        MaxPuzzleRoom = newFloor.MaxPuzzleRoom;
        MinRewardRoom = newFloor.MinRewardRoom;
        MaxRewardRoom = newFloor.MaxRewardRoom;
        haveBoss = newFloor.haveBoss;
        //StartRoom = newFloor.StartRoom;
        //EndRoom = newFloor.EndRoom;
    }
    public void LoadNewBiome()
    {
        BiomeName = biome.biomeName;
        GenericRoom = biome.GenericRoom;
        RewardRoom = biome.RewardRoom;
        PuzzleRoom = biome.PuzzleRoom;
        StartRoom = biome.startRoom;
        ExitRoom = biome.exitRoom;
    }


}
