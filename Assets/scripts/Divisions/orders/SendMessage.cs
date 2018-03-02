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
        if(controller.FindVisibleDivision(finish.divisionId, out target))
        {
            target.ReceiveOrders(message);
            hasFoundTarget = true;
        }
    }

    public override bool TestIfFinished()
    {
        return hasFoundTarget;
    }

    public override void End()
    {
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RememberedDivision commander = controller.rememberedDivisions[commanderSendingOrder.divisionId];
        controller.ReceiveOrder(new FindDivision(controller, target, commander));
        controller.ReceiveOrder(new MergeDivisions(controller, commander));
    }
}
