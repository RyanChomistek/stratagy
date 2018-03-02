using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitDivision : Order {

    List<Tuple<SoldierType, int>> soldiersToSplit;
    public bool isFinishedSpliting;
    public SplitDivision(DivisionController divisionToSplit, List<Tuple<SoldierType, int>> soldiersToSplit)
    {
        this.controller = divisionToSplit;
        this.soldiersToSplit = soldiersToSplit;
        this.isFinishedSpliting = false;
    }

    public override void Start()
    {
        List<Soldier> soldiers = new List<Soldier>();
        //find soldiers
        foreach(Soldier soldier in controller.soldiers)
        {
            foreach(var soldierTypeWanted in soldiersToSplit)
            {
                if(soldier.type == soldierTypeWanted.first)
                {
                    soldierTypeWanted.second--;
                    soldiers.Add(soldier);
                    continue;
                }
            }
        }

        controller.CreateChild(soldiers);
        isFinishedSpliting = true;
    }
    public override void Pause() { }
    public override void End() { }
    public override void OnClickedInUI()
    {
        //bing up ui to split choose what units to split
        GameObject splitMenu = OrderPrefabManager.Instantiate(OrderPrefabManager.instance.prefabs["DivisionSplitMenu"]);
        splitMenu.transform.SetParent(OrderPrefabManager.instance.mainCanvas.transform, false);
        splitMenu.GetComponent<DivisionSplitMenu>().Setup(controller);
        //regester a func as a callback
        //send the order
    }
    public override void Proceed() { }
    public override bool TestIfFinished() { return isFinishedSpliting; }
}
