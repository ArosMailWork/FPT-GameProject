using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private RoomGen Gen;
    [SerializeField] private int GridSize = 1;
    [SerializeField] private int MapScale_test = 15;
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
    private Room StartRoom;
    private Room EndRoom;

    public void Start()
    {
        GenerateFloor();
        //player.transform.position = Gen.s

    }
    public void GenerateFloor()
    {
        //LoadNewBiome();
        AssignDataToRoomGen();
        Gen.Generate();

    }
    public void SetScale()
    {
        Debug.Log("adaojsjp");
        transform.localScale = new Vector3(MapScale_test, MapScale_test, MapScale_test); //test scale   
    }

    public void AssignDataToRoomGen()
    {
        Gen.AssignData(GridSize, MainPathLength, UnityEngine.Random.Range(MinPuzzleRoom, MaxPuzzleRoom), UnityEngine.Random.Range(MinRewardRoom, MaxRewardRoom), GenericRoom, RewardRoom, PuzzleRoom, StartRoom, EndRoom);
    }
    public void LoadNewFloor(Floor newFloor)
    {

        MainPathLength = newFloor.MainPathLength;
        SideRoomChance = newFloor.SideRoomChance;
        MinPuzzleRoom = newFloor.MinPuzzleRoom;
        MaxPuzzleRoom = newFloor.MaxPuzzleRoom;
        MinRewardRoom = newFloor.MinRewardRoom;
        MaxRewardRoom = newFloor.MaxRewardRoom;
        StartRoom = newFloor.StartRoom;
        EndRoom = newFloor.EndRoom;
    }
    public void LoadNewBiome(Biome newBiome)
    {
        biome = newBiome;
        BiomeName = biome.biomeName;
        GenericRoom = biome.GenericRoom;
        RewardRoom = biome.RewardRoom;
        PuzzleRoom = biome.PuzzleRoom;
        StartRoom = biome.startRoom;
        EndRoom = biome.endRoom;

    }
}
