using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Order {
    public Vector3 finish;
    
    public Move(DivisionController controller, DivisionController commanderSendingOrder, Vector3 finish)
    {
        this.commanderSendingOrder = commanderSendingOrder;
        this.controller = controller;
        this.finish = finish;
    }

    public override void start()
    {
        Debug.Log("starting move");
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish - currLoc).normalized;
        Vector3 moveVec = dir * controller.speed;
        Debug.Log(moveVec);
        //set start moving twords finish
        controller.GetComponent<Rigidbody>().velocity = moveVec;
    }

    public override void pause()
    {
        
    }

    public override void onClickedInUI()
    {
        Debug.Log(InputController.instance);
        InputController.instance.registerOnClickCallBack(onClickReturn);
    }

    public void onClickReturn(Vector3 mousePos)
    {
        finish = mousePos;
        //clear ui
        OrderDisplayManager.instance.clearOrders();
        commanderSendingOrder.sendOrderTo(controller, this);
    }

    public override void proceed()
    {

    }

    public override bool testIfFinished()
    {
        return false;
    }
}
