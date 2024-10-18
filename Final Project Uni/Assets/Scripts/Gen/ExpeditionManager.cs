using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public GenerationManager gen;
    public List<World> worlds;
    public List<Floor> floors;
    public World currentWorld;
    public Biome currentBiome;
    public int currentWorldNumber = 0;
    public int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetWorld(0);
    }


    void SetWorld(int worldIndex)
    {
        currentWorld = worlds[worldIndex];
        currentWorldNumber = worldIndex;
        currentLevel = 0;
    }
    void SetRandomBiome()
    {
        List<Biome> biomes = currentWorld.biomePool;
        currentBiome = biomes[UnityEngine.Random.Range(0, biomes.Count)];
    }
    void GenerateFloor(World world, Floor floor)
    {
        gen.LoadNewBiome(currentBiome);
        //floo
    }


}
