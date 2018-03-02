using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDivisionController : DivisionController
{
    public RememberedDivision generalDivision;

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

    public void SetCommanders(RememberedDivision commander, RememberedDivision general)
    {
        base.Init(commander);
        generalDivision = general;
    }

    public override void Init()
    {
        possibleOrders.Add(new Move(this, LocalPlayerController.instance.generalDivision, new Vector3()));
        possibleOrders.Add(new SplitDivision(this, null));
        GameManager.instance.RefreshAllDivisons();
        ongoingOrder = new EmptyOrder();
        divisionId = divisionCounter++;
        name = "division : " + divisionId;
    }

    public override DivisionController CreateChild(List<Soldier> soldiersForChild)
    {
        PlayerDivisionController child = (PlayerDivisionController)base.CreateChild(soldiersForChild);
        child.SetCommanders(GenerateRememberedDivision(), generalDivision);

        return child;
    }
}
