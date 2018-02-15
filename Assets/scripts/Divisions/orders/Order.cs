using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order {
    public DivisionController controller;
    public DivisionController commanderSendingOrder;

    public virtual void Start() { }
    public virtual void Pause() { }
    public virtual void End() { }
    public virtual void OnClickedInUI() { }
    public virtual void Proceed() { }
    public virtual bool TestIfFinished() { return false; }

}
