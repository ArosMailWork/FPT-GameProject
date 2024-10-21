using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Gen/World")]
[System.Serializable]

public class World : ScriptableObject
{
    public string worldName;
    public int numberOfFloor = 3;
    public int difficulty;
    public List<Biome> biomePool;
    public List<Floor> floors;
}
