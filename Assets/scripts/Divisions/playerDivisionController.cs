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
        initOrders();
    }

    void Update()
    {
        doOrders();
    }

    public void init(DivisionController commander, DivisionController general)
    {
        base.init(commander);
        generalDivision = general;
    }

    public override void initOrders()
    {
        possibleOrders.Add(new Move(this, generalDivision, new Vector3()));
    }

    public override DivisionController createChild()
    {
        playerDivisionController child = (playerDivisionController) base.createChild();
        child.init(this, generalDivision);

        return child;
    }
}
