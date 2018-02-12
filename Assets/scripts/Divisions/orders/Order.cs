using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Order {
    protected DivisionController controller;

    public abstract void start();
    public abstract void pause();
    public abstract void proceed();
    public abstract bool testIfFinished();
}
