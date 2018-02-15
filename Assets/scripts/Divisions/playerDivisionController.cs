using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDivisionController : DivisionController
{
    public DivisionController generalDivision;

    public void Select()
    {
        GameManager.instance.localPlayer.select(this);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        OnUpdate();
    }

    public void SetCommanders(DivisionController commander, DivisionController general)
    {
        base.Init(commander);
        generalDivision = general;
    }

    public override void Init()
    {
        possibleOrders.Add(new Move(this, generalDivision, new Vector3()));
        GameManager.instance.RefreshAllDivisons();
        ongoingOrder = new EmptyOrder();
        divisionId = divisionCounter++;
    }

    public override DivisionController CreateChild(List<Soldier> soldiersForChild)
    {
        playerDivisionController child = (playerDivisionController) base.CreateChild(soldiersForChild);
        child.SetCommanders(this, generalDivision);

        return child;
    }
}
