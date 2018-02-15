using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessage : Order {
    bool hasFoundTarget;
    List<Order> message;
    DivisionController target;
    public DivisionController finish;
    public SendMessage(DivisionController controller, DivisionController commanderSendingOrder, List<Order> message, DivisionController target)
    {
        this.commanderSendingOrder = commanderSendingOrder;
        this.controller = controller;
        this.finish = target;
        this.target = target;
        this.message = message;
    }

    public override void Proceed()
    {
        //if we have reached our target turn around and head back
        Vector3 currLoc = controller.transform.position;
        float distanceToFinish = (finish.transform.position - currLoc).magnitude;
        if (distanceToFinish < .1f && !hasFoundTarget)
        {
            //hand off message
            target.ReceiveOrders(message);
            finish = commanderSendingOrder;
            hasFoundTarget = true;
        }
        moveToTarget();
    }

    public void moveToTarget()
    {
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish.transform.position - currLoc).normalized;
        Vector3 moveVec = dir * controller.speed;
        //set start moving twords finish
        controller.GetComponent<Rigidbody>().velocity = moveVec;
    }

    public override bool TestIfFinished()
    {
        if (hasFoundTarget)
        {
            Vector3 currLoc = controller.transform.position;
            float distanceToFinish = (commanderSendingOrder.transform.position - currLoc).magnitude;
            if (distanceToFinish < .1f)
            {
                return true;
            }
        }
        return false;
    }

    public override void End()
    {
        //merge our troops back into the main dude
        commanderSendingOrder.AbsorbDivision(controller);
    }
}
