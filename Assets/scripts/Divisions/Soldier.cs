using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoldierType
{
    Melee, Ranged, Seige
}

[System.Serializable]
public class Soldier {
    public float speed = 1;
    public float hitStrength = 1;
    public float range = 1;
    public float sightDistance = 1;
    public SoldierType type = SoldierType.Melee;
}
