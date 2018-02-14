using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDivisionController : DivisionController
{
    public DivisionController generalDivision;

    public void select()
    {
        GameManager.instance.localPlayer.select(this);
    }

    void Start()
    {
        ongoingOrder = new EmptyOrder();
        init();
    }

    void Update()
    {
        doOrders();
    }

    public void setCommanders(DivisionController commander, DivisionController general)
    {
        base.init(commander);
        generalDivision = general;
    }

    public override void init()
    {
        possibleOrders.Add(new Move(this, generalDivision, new Vector3()));
    }

    public override DivisionController createChild(List<Soldier> soldiersForChild)
    {
        playerDivisionController child = (playerDivisionController) base.createChild(soldiersForChild);
        child.setCommanders(this, generalDivision);

        return child;
    }
}
