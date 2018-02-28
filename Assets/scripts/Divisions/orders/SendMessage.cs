using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessage : Order {
    bool hasFoundTarget;
    List<Order> message;
    DivisionController target;
    public RememberedDivision finish;
    public SendMessage(DivisionController controller, DivisionController commanderSendingOrder, List<Order> message, RememberedDivision target)
    {
        this.commanderSendingOrder = commanderSendingOrder;
        this.controller = controller;
        this.finish = target;
        this.target = null;
        this.message = message;
    }

    public override void Proceed()
    {
        //if we have reached our target turn around and head back
        Vector3 currLoc = controller.transform.position;
        float distanceToFinish = (finish.position - currLoc).magnitude;
        if(target == null)
        {
            //List<DivisionController> visibleDivisions = controller.visibleDivisions;
            //target = visibleDivisions.Find(x => finish.divisionId == x.divisionId);
            controller.FindVisibleDivision(finish.divisionId, out target);
        }

        if(target != null)
        {
            distanceToFinish = (target.transform.position - currLoc).magnitude;
        }
        
        if (distanceToFinish < .1f && !hasFoundTarget)
        {
            //hand off message
            target.ReceiveOrders(message); //todo need to get the actual target
            //finish = commanderSendingOrder.GenerateRememberedDivision(); //todo need to use reference from remembered
            hasFoundTarget = true;
            //target = null;
        }
        
        moveToTarget();
    }

    public void moveToTarget()
    {
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish.position - currLoc).normalized;
        Vector3 moveVec = dir * controller.speed;
        //set start moving twords finish
        controller.GetComponent<Rigidbody>().velocity = moveVec;
    }

    public override bool TestIfFinished()
    {
        /*
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
        */
        return hasFoundTarget;
    }

    public override void End()
    {
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RememberedDivision commander = controller.rememberedDivisions[commanderSendingOrder.divisionId];
        controller.ReceiveOrder(new FindDivision(controller, target, commander));
        controller.ReceiveOrder(new MergeDivisions(controller, commander));
        //merge our troops back into the main dude
        //commanderSendingOrder.AbsorbDivision(controller);
    }
}
