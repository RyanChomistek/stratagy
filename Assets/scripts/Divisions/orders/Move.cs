using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Order {
    public Vector3 finish;

    public Move(DivisionController controller, Vector3 finish)
    {
        this.controller = controller;
        this.finish = finish;
    }

    public override void start()
    {
        Vector3 currLoc = controller.transform.position;
        Vector3 dir = (finish - currLoc).normalized;

        //set start moving twords finish
        controller.GetComponent<Rigidbody>().velocity = dir*controller.speed;
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
        Debug.Log(mousePos);
    }

    public override void proceed()
    {

    }

    public override bool testIfFinished()
    {
        return false;
    }
}
