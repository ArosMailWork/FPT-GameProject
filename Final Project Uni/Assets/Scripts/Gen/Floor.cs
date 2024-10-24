using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "Gen/Floor")]
[System.Serializable]
public class Floor : ScriptableObject
{
    public string floorName;
    [FoldoutGroup("Room Setup")]
    public int MainPathLength, SideRoomChance;
    [FoldoutGroup("Room Setup")]
    public bool haveBoss;
    [FoldoutGroup("Special Room")]
    public int MinPuzzleRoom, MaxPuzzleRoom, MinRewardRoom, MaxRewardRoom;

}
