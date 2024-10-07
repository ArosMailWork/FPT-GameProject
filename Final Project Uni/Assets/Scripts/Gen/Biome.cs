using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Gen/Biome")]
[System.Serializable]
public class Biome : ScriptableObject
{
    public String biomeName;
    public List<Room> GenericRoom;
    public List<Room> RewardRoom;
    public List<Room> PuzzleRoom;
    public Room startRoom;
    public Room endRoom;
}


