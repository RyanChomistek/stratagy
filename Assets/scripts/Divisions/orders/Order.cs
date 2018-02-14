using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order {
    public DivisionController controller;
    public DivisionController commanderSendingOrder;

    public virtual void start() { }
    public virtual void pause() { }
    public virtual void end() { }
    public virtual void onClickedInUI() { }
    public virtual void proceed() { }
    public virtual bool testIfFinished() { return false; }

}
