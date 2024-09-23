using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    //Light = Odd , Heavy = Even
    //Ground 1->8
    //Infantry 1->6
    //Mounted 7->8
    //Air 9->10

    Generic = 0,
    LightMelee = 1,
    HeavyMelee = 2,
    LightRange = 3,
    HeavyRange = 4,
    LightMagic = 5,
    HeavyMagic = 6,
    LightMounted = 7,
    HeavyMounted = 8,
    LightAir = 9,
    HeavyAir = 10,

}
