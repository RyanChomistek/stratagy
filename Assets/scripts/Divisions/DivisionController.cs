using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DivisionController : MonoBehaviour {
    protected static int divisionCounter = 0;
    public int divisionId;
    public DivisionController commander;
    public List<DivisionController> subordinates = new List<DivisionController>();
    public List<Soldier> soldiers;
    public float speed = 10;
    public List<Order> possibleOrders;

    public List<Order> orderQueue = new List<Order>();
    public Order ongoingOrder = null;
    public GameObject divisonPrefab;
    public GameObject display;
    public List<DivisionController> visibleDivisions = new List<DivisionController>();
    //warning anyone who touches this may have to deal with nulls
    public List<RememberedDivision> rememberedDivisions = new List<RememberedDivision>();

    // Use this for initialization
    void Start () {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        UpdateSpeed();
        CheckFOW();
        DoOrders();
    }

    public virtual void DoOrders()
    {
        //Debug.Log(ongoingOrder.GetType());
        if (ongoingOrder.GetType() != typeof(EmptyOrder))
        {
            //if we are finished stop
            if (ongoingOrder.TestIfFinished())
            {
                ongoingOrder.End();
                ongoingOrder = new EmptyOrder();
            }
            else
            {
                ContinueOrder();
            }
        }
        //grab a new order
        else if (orderQueue.Count > 0)
        {
            Debug.Log("starting new order");
            ongoingOrder = orderQueue[0];
            orderQueue.RemoveAt(0);
            ongoingOrder.Start();
            ContinueOrder();
        }
    }

    void CheckFOW()
    {
        List<DivisionController> allDivisions = GameManager.instance.allDivisions;
        float maxSightDistance = 0;
        visibleDivisions = new List<DivisionController>();
        //fing the largest sight distance
        foreach (Soldier soldier in soldiers)
        {
            if(maxSightDistance < soldier.sightDistance)
            {
                maxSightDistance = soldier.sightDistance;
            }
        }

        //find all of the divisions inside our sight distance
        foreach(DivisionController division in allDivisions)
        {
            Vector3 end = division.transform.position;
            Vector3 start = transform.position;
            float dis = (end - start).magnitude;
            if(dis < maxSightDistance)
            {
                //Debug.Log(division.name);
                visibleDivisions.Add(division);
                //RememberedDivision rememberedDivision = division.GenerateRememberedDivision();

                int rememberedDivsionIndex = 
                    rememberedDivisions.FindIndex(x => x.divisionId == division.divisionId);

                //RememberedDivision rememberedDivisionResult = rememberedDivisions[rememberedDivsionIndex];

                if (rememberedDivsionIndex == -1)
                {
                    RememberedDivision rememberedDivisionResult = division.GenerateRememberedDivision();
                    rememberedDivisions.Add(rememberedDivisionResult);
                }
                else
                {
                    RememberedDivision rememberedDivisionResult = rememberedDivisions[rememberedDivsionIndex];
                    Debug.Log(rememberedDivisionResult);
                    rememberedDivisions[rememberedDivsionIndex] = division.GenerateRememberedDivision();
                    //rememberedDivisionResult.Update(rememberedDivisionResult);

                }
            }
        }
    }

    void UpdateSpeed()
    {
        if (soldiers.Count == 0)
        {
            speed = 0;
            return;
        }
        float sum = 0;
        foreach(Soldier soldier in soldiers)
        {
            sum += soldier.speed;
        }
        speed = sum / soldiers.Count;
    }

    void ContinueOrder()
    {
        ongoingOrder.Proceed();
    }

    public void Init(DivisionController commander)
    {
        this.commander = commander;
        //subordinates = new List<DivisionController>();
        possibleOrders = new List<Order>();
    }

    public virtual void Init()
    {
        possibleOrders.Add(new Move(this,null,new Vector3()));
        ongoingOrder = new EmptyOrder();
        GameManager.instance.RefreshAllDivisons();
        divisionId = divisionCounter++;
    }
    
    public void SendOrderTo(DivisionController to, Order order)
    {
        //follow commander tree to get there
        List<DivisionController> pathToDivision = FindDivision(this, to, new List<DivisionController>());
        //if path is only size one, were at where the order needs to go
        if(pathToDivision.Count == 1)
        {
            Debug.Log(order);
            ReceiveOrder(order);
            return;
        }

        //send order to the next commander
        SendMessenger(pathToDivision[1], order);
    }

    public void SendMessenger(DivisionController to, Order order)
    {
        //create a new division
        //todo make the messenger descesion smart
        List<Soldier> soldiersToGive = new List<Soldier>();
        soldiersToGive.Add(PopSoldier());
        DivisionController messenger = CreateChild(soldiersToGive);

        //give it a move order to go to the division
        List<Order> orders = new List<Order>();
        orders.Add(order);
        //todo discover location of to
        messenger.ReceiveOrder(new SendMessage(messenger, this, orders, to));
    }

    public virtual DivisionController CreateChild(List<Soldier> soldiersForChild)
    {
        GameObject newDivision = Instantiate(divisonPrefab);
        DivisionController newController = newDivision.GetComponent<DivisionController>();
        newController.Init(this);
        newController.soldiers = soldiersForChild;
        newDivision.transform.position = this.transform.position;
        newDivision.transform.rotation = this.transform.rotation;
        subordinates.Add(newController);
        GameManager.instance.RefreshAllDivisons();
        return newController;
    }

    public List<DivisionController> FindDivision(DivisionController start, DivisionController end, List<DivisionController> prev_)
    {
        List<DivisionController> prev = new List<DivisionController>(prev_);
        prev.Add(start);
        if (start == end)
        {
            return prev;
        }

        foreach(DivisionController division in start.subordinates)
        {
            List<DivisionController> temp = FindDivision(division, end, prev);
            if(temp != null)
            {
                return temp;
            }
        }

        return null;
    }

    public void ReceiveOrder(Order order)
    {
        orderQueue.Add(order);
    }

    public void ReceiveOrders(List<Order> orders)
    {
        foreach (Order order in orders)
        {
            orderQueue.Add(order);
        }
    }

    protected Soldier PopSoldier()
    {
        Soldier soldier = soldiers[soldiers.Count - 1];
        soldiers.Remove(soldier);
        return soldier;
    }

    public void TransferSoldiers(List<Soldier> troops)
    {
        soldiers.AddRange(troops);
        troops.Clear();
    }

    public List<DivisionController> GetAllSubordinates()
    {
        List<DivisionController> allSubordinates = new List<DivisionController>();
        allSubordinates.Add(this);
        foreach(DivisionController division in subordinates)
        {
            allSubordinates.AddRange(division.GetAllSubordinates());
        }

        return allSubordinates;
    }

    public void AbsorbDivision(DivisionController other)
    {
        TransferSoldiers(other.soldiers);
        //kick him out of his commanders subordinate list
        DivisionController parent = other.commander;
        
        if(parent != null)
        {
            parent.RemoveSubordinate(other);
        }
        GameObject.Destroy(other.gameObject);
        GameManager.instance.RefreshAllDivisons();
    }

    public void RemoveSubordinate(DivisionController division)
    {
        subordinates.Remove(division);
    }

    public RememberedDivision GenerateRememberedDivision()
    {
        RememberedDivision rememberedDivision =
            new RememberedDivision(
                transform.position, GetComponent<Rigidbody>().velocity, orderQueue, soldiers, divisionId, Time.time);

        return rememberedDivision;
    }
}
