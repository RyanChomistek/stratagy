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

    public override void Start()
    {
        moveToTarget();
    }

    public void moveToTarget()
    {
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish - currLoc).normalized;
        Vector3 moveVec = dir * controller.speed;
        //set start moving twords finish
        controller.GetComponent<Rigidbody>().velocity = moveVec * GameManager.instance.gameSpeed;
    }

    public override void Pause()
    {
        controller.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    public override void End()
    {
        controller.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    public override void OnClickedInUI()
    {
        InputController.instance.registerOnClickCallBack(onClickReturn);
    }

    public void onClickReturn(Vector3 mousePos)
    {
        finish = new Vector3(mousePos.x, mousePos.y);
        //clear ui
        OrderDisplayManager.instance.ClearOrders();
        //need to get 
        commanderSendingOrder.SendOrderTo(controller.GenerateRememberedDivision(), new Move(controller, commanderSendingOrder, finish));
    }

    public override void Proceed()
    {

    }

    public override bool TestIfFinished()
    {
        Vector3 currLoc = controller.transform.position;
        float distanceToFinish = (finish - currLoc).magnitude;
        if (distanceToFinish < .1f)
        {
            return true;
        }
        return false;
    }
}
