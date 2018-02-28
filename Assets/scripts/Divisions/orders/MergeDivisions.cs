using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeDivisions : Order {

    RememberedDivision target;
    bool finishedMerging;
    public MergeDivisions(DivisionController controller, RememberedDivision target)
    {
        this.controller = controller;
        this.target = target;
        this.finishedMerging = false;
    }

    public override void Start() { }
    public override void Pause() { }
    public override void End() { }
    public override void OnClickedInUI() { }
    public override void Proceed()
    {
        DivisionController divisionToMergeWith;
        if(controller.FindVisibleDivision(target.divisionId, out divisionToMergeWith))
        {
            divisionToMergeWith.AbsorbDivision(controller);
            finishedMerging = true;
        }
    }
    public override bool TestIfFinished() { return finishedMerging; }
}
