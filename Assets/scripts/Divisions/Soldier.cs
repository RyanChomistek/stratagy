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

    public Soldier(float speed, float hitStrength, float range, float sightDistance, SoldierType type)
    {
        this.speed = speed;
        this.hitStrength = hitStrength;
        this.range = range;
        this.sightDistance = sightDistance;
        this.type = type;
    }
}
