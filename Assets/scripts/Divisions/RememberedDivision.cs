using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RememberedDivision {
    public Vector3 position;
    public Vector3 velocity;
    public List<Order> orderQueue;
    public List<Order> possibleOrders;
    public List<Soldier> soldiers;
    public int divisionId;
    public float timeStamp;

    public RememberedDivision(Vector3 position, Vector3 velocity, List<Order> orders,
        List<Soldier> soldiers, int divisionId, float timeStamp, List<Order> possibleOrders)
    {
        this.position = position;
        this.velocity = velocity;
        this.orderQueue = orders;
        this.soldiers = soldiers;
        this.divisionId = divisionId;
        this.timeStamp = timeStamp;
        this.possibleOrders = possibleOrders;
    }

    public void Update(RememberedDivision division)
    {
        this.position = division.position;
        this.velocity = division.velocity;
        this.orderQueue = division.orderQueue;
        this.soldiers = division.soldiers;
        this.divisionId = division.divisionId;
        this.timeStamp = division.timeStamp;
        this.possibleOrders = division.possibleOrders;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        RememberedDivision division = obj as RememberedDivision;

        if (division == null)
            return false;

        return division.divisionId == divisionId;
    }
}
