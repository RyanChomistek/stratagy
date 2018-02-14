using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionController : MonoBehaviour {

    public DivisionController commander;
    public List<DivisionController> subordinates;
    public List<Soldier> soldiers { get; set; }
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
        //Debug.Log(ongoingOrder.GetType());
        if (ongoingOrder.GetType() != typeof(EmptyOrder))
        {
            //if we are finished stop
            if (ongoingOrder.testIfFinished())
            {
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

    public virtual void initOrders()
    {
        possibleOrders.Add(new Move(this,null,new Vector3()));
    }
    
    public void sendOrderTo(DivisionController other, Order order)
    {
        //follow commander tree to get there
        List<DivisionController> pathToDivision = findDivision(this, other, new List<DivisionController>());
        Debug.Log(this);
        //if path is only size one, were at where the order needs to go
        if(pathToDivision.Count == 1)
        {
            Debug.Log(order);
            receiveOrder(order);
            return;
        }

        //send order to the next commander
        sendMessenger(pathToDivision[0], order);
    }

    public void sendMessenger(DivisionController to, Order order)
    {
        //create a new division
        DivisionController messenger = createChild();

        //give it a move order to go to the division

        //todo discover location of to
        messenger.receiveOrder(new Move(to, this, to.transform.position));
    }

    public virtual DivisionController createChild()
    {
        GameObject newDivision = Instantiate(divisonPrefab);
        DivisionController newController = newDivision.GetComponent<DivisionController>();
        newController.init(this);
        newDivision.transform.position = this.transform.position;
        newDivision.transform.rotation = this.transform.rotation;

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
}
