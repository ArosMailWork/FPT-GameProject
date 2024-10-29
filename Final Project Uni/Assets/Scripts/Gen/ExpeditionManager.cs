using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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
    public int currentFloorNumber = 0;
    [FoldoutGroup("Event")]
    public UnityEvent OnExit, LoadNextFloor;

    #region Event

    //Load next floor after event 
    [Button]
    public void NextFloor()
    {
        Debug.Log("World : " + currentWorldNumber + " - " + " Floor :" + currentFloorNumber);
        GenerateFloor(currentFloorNumber);
        LoadNextFloor.Invoke();
    }

    //setup number pre-event to prepare for next floor load
    [Button]
    public void ExitFloor()
    {

        if (currentFloorNumber < floors.Count)
        {
            currentFloorNumber += 1;
            OnExit.Invoke();
        }
        else if (currentWorldNumber < worlds.Count)
        {
            currentWorldNumber += 1;
            currentFloorNumber = 0;
            SetWorld(currentWorldNumber);
            OnExit.Invoke();
        }
        else
        {
            ExpeditionComplete();
        }
    }
    public void ExpeditionStart()
    {
        SetWorld(currentWorldNumber);
        GenerateFloor(currentFloorNumber);
    }
    public void ExpeditionComplete()
    {
        currentWorldNumber = 0;
        currentFloorNumber = 0;
    }

    #endregion

    #region GenerationManager
    // Start is called before the first frame update
    void Start()
    {
        ExpeditionStart();
    }
    [Button]
    void GenerateRandomFloor()
    {
        SetFloorData(Random.Range(0, floors.Count));
        gen.Generate();
    }
    void GenerateFloor(int floor)
    {
        SetFloorData(floor);
        gen.Generate();
    }

    void SetWorld(int worldIndex)
    {
        currentWorld = worlds[worldIndex];
        currentWorldNumber = worldIndex;
        ChooseRandomBiome();
        floors = currentWorld.floors;
        currentFloorNumber = 0;
    }
    void ChooseRandomBiome()
    {
        List<Biome> biomes = currentWorld.biomePool;
        currentBiome = biomes[UnityEngine.Random.Range(0, biomes.Count)];
        gen.LoadBiomeData(currentBiome);
    }
    void SetFloorData(int floor)
    {
        Floor currFloor = floors[floor];
        gen.LoadFloorData(currFloor);
    }
    #endregion

    #region EventManager


    #endregion
}
