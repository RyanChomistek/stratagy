using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDivision : Order
{
    bool hasFoundTarget;
    DivisionController target;
    public RememberedDivision finish;
    public FindDivision(DivisionController controller, DivisionController commanderSendingOrder, RememberedDivision finish)
    {
        this.commanderSendingOrder = commanderSendingOrder;
        this.controller = controller;
        this.finish = finish;
        this.target = null;
    }

    public override void Proceed()
    {
        Vector3 currLoc = controller.transform.position;
        float distanceToFinish = (finish.position - currLoc).magnitude;

        //if target is null look for it in the visible divisions
        if (target == null)
        {
            controller.FindVisibleDivision(finish.divisionId, out target);
        }

        //if it isnt null go find em
        if (target != null)
        {
            distanceToFinish = (target.transform.position - currLoc).magnitude;
        }

        //when this is true then we have caught up to our target
        if (distanceToFinish < .1f && !hasFoundTarget)
        {
            hasFoundTarget = true;
            target = null;
        }

        moveToTarget();
    }

    public void moveToTarget()
    {
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish.position - currLoc).normalized;
        Vector3 moveVec = dir * controller.speed;
        controller.GetComponent<Rigidbody>().velocity = moveVec;
    }

    public override bool TestIfFinished()
    {
        return hasFoundTarget;
    }

    public override void End()
    {
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
