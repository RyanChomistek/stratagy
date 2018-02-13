using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDivisionController : DivisionController
{
    public void select()
    {
        GameManager.instance.localPlayer.select(this);
    }

    void Start()
    {
        base.initOrders();
    }
}
