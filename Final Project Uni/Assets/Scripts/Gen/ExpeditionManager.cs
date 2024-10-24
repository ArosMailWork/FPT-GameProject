using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public GenerationManager gen;
    public List<World> worlds;
    public List<Floor> floors;
    public World currentWorld;
    public Biome currentBiome;
    [FoldoutGroup("Expedition Number")]
    public int currentWorldNumber = 0;
    [FoldoutGroup("Expedition Number")]
    public int currentFloor = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetWorld(0);
        GenerateRandomFloor();
    }
    [Button]
    void GenerateRandomFloor()
    {
        ChooseRandomBiome();
        SetFloorData(floors[Random.Range(0, floors.Count)]);
        gen.Generate();
    }
    [Button]
    void RegenerateFloor()
    {
        ChooseRandomBiome();
        SetFloorData(floors[currentFloor]);
        gen.Generate();
    }

    void SetWorld(int worldIndex)
    {
        currentWorld = worlds[worldIndex];
        currentWorldNumber = worldIndex;
        floors = currentWorld.floors;
        currentFloor = 0;
    }
    void ChooseRandomBiome()
    {
        List<Biome> biomes = currentWorld.biomePool;
        currentBiome = biomes[UnityEngine.Random.Range(0, biomes.Count)];
        gen.LoadBiomeData(currentBiome);
    }
    void SetFloorData(Floor floor)
    {
        gen.LoadFloorData(floor);
    }


}
