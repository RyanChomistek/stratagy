using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessage : Move {
    bool hasFoundTarget;
    List<Order> message;
    DivisionController target;
    public SendMessage(DivisionController controller, DivisionController commanderSendingOrder, Vector3 finish, List<Order> message, DivisionController target)
        : base(controller, commanderSendingOrder, finish)
    {
        this.target = target;
        this.message = message;
    }

    public override void proceed()
    {
        //if we have reached our target turn around and head back
        Vector3 currLoc = controller.transform.position;
        float distanceToFinish = (finish - currLoc).magnitude;
        if (distanceToFinish < .1f && !hasFoundTarget)
        {
            //hand off message
            target.receiveOrders(message);
            finish = commanderSendingOrder.transform.position;
            hasFoundTarget = true;
        }
        moveToTarget();
    }

    public override bool testIfFinished()
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
}
