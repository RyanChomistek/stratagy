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
    public List<RememberedDivision> subordinates;
    public RememberedDivision commander;
    public int divisionId;
    public float timeStamp;

    public RememberedDivision(Vector3 position, Vector3 velocity, List<Order> orders,
        List<Soldier> soldiers, int divisionId, float timeStamp, 
        List<Order> possibleOrders, List<RememberedDivision> subordinates, RememberedDivision commander)
    {
        this.position = position;
        this.velocity = velocity;
        this.orderQueue = orders;
        this.soldiers = soldiers;
        this.divisionId = divisionId;
        this.timeStamp = timeStamp;
        this.possibleOrders = possibleOrders;
        this.subordinates = subordinates;
        this.commander = commander;
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
        this.subordinates = division.subordinates;
        this.commander = division.commander;
    }

    public List<RememberedDivision> GetAllSubordinates()
    {
        List<RememberedDivision> allSubordinates = new List<RememberedDivision>();
        allSubordinates.Add(this);
        foreach (RememberedDivision division in subordinates)
        {
            allSubordinates.AddRange(division.GetAllSubordinates());
        }

        return allSubordinates;
    }

    public void RemoveSubordinate(RememberedDivision division)
    {
        subordinates.Remove(division);
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
