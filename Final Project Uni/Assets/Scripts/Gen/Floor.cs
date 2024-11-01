using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "Gen/Floor")]
[System.Serializable]
public class Floor : ScriptableObject
{
    public string floorName;
    public int MainPathLength;
    public int SideRoomChance;
    public int MinPuzzleRoom;
    public int MaxPuzzleRoom;
    public int MinRewardRoom;
    public int MaxRewardRoom;
    public Room StartRoom;
    public Room EndRoom;
}
