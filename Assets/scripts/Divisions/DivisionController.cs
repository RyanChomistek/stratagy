using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionController : MonoBehaviour {

    public DivisionController commander;
    public List<DivisionController> subordinates;
    public List<Soldier> soldiers;
    public float speed = 10;
    public List<Order> possibleOrders;

    public List<Order> orderQueue = new List<Order>();
    public Order ongoingOrder = null;
    public GameObject divisonPrefab;

    // Use this for initialization
    void Start () {
        ongoingOrder = new EmptyOrder();
    }

    // Update is called once per frame
    void Update()
    {
        doOrders();
    }

    public virtual void doOrders()
    {
        updateSpeed();
        //Debug.Log(ongoingOrder.GetType());
        if (ongoingOrder.GetType() != typeof(EmptyOrder))
        {
            //if we are finished stop
            if (ongoingOrder.testIfFinished())
            {
                ongoingOrder.end();
                ongoingOrder = new EmptyOrder();
            }
            else
            {
                continueOrder();
            }
        }
        //grab a new order
        else if (orderQueue.Count > 0)
        {
            Debug.Log("starting new order");
            ongoingOrder = orderQueue[0];
            orderQueue.RemoveAt(0);
            ongoingOrder.start();
            continueOrder();
        }
    }

    void updateSpeed()
    {
        float sum = 0;
        foreach(Soldier soldier in soldiers)
        {
            sum += soldier.speed;
        }
        speed = sum / soldiers.Count;
    }

    void continueOrder()
    {
        ongoingOrder.proceed();
    }

    public void init(DivisionController commander)
    {
        this.commander = commander;
        subordinates = new List<DivisionController>();
        possibleOrders = new List<Order>();
    }

    public virtual void init()
    {
        possibleOrders.Add(new Move(this,null,new Vector3()));
    }
    
    public void sendOrderTo(DivisionController to, Order order)
    {
        //follow commander tree to get there
        List<DivisionController> pathToDivision = findDivision(this, to, new List<DivisionController>());
        Debug.Log(this);
        //if path is only size one, were at where the order needs to go
        if(pathToDivision.Count == 1)
        {
            Debug.Log(order);
            receiveOrder(order);
            return;
        }

        //send order to the next commander
        sendMessenger(pathToDivision[1], order);
    }

    public void sendMessenger(DivisionController to, Order order)
    {
        //create a new division
        //todo make the messenger descesion smart
        List<Soldier> soldiersToGive = new List<Soldier>();
        soldiersToGive.Add(popSoldier());
        DivisionController messenger = createChild(soldiersToGive);

        //give it a move order to go to the division
        List<Order> orders = new List<Order>();
        orders.Add(order);
        //todo discover location of to
        messenger.receiveOrder(new SendMessage(messenger, this, to.transform.position, orders, to));
    }

    public virtual DivisionController createChild(List<Soldier> soldiersForChild)
    {
        GameObject newDivision = Instantiate(divisonPrefab);
        DivisionController newController = newDivision.GetComponent<DivisionController>();
        newController.init(this);
        newController.soldiers = soldiersForChild;
        newDivision.transform.position = this.transform.position;
        newDivision.transform.rotation = this.transform.rotation;
        subordinates.Add(newController);
        return newController;
    }

    public List<DivisionController> findDivision(DivisionController start, DivisionController end, List<DivisionController> prev_)
    {
        List<DivisionController> prev = new List<DivisionController>(prev_);
        prev.Add(start);
        if (start == end)
        {
            return prev;
        }

        foreach(DivisionController division in start.subordinates)
        {
            List<DivisionController> temp = findDivision(division, end, prev);
            if(temp != null)
            {
                return temp;
            }
        }

        return null;
    }

    public void receiveOrder(Order order)
    {
        orderQueue.Add(order);
    }

    public void receiveOrders(List<Order> orders)
    {
        foreach (Order order in orders)
        {
            orderQueue.Add(order);
        }
    }

    protected Soldier popSoldier()
    {
        Soldier soldier = soldiers[soldiers.Count - 1];
        soldiers.Remove(soldier);
        return soldier;
    }
}
