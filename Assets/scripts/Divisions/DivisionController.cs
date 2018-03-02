using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DivisionController : MonoBehaviour {
    protected static int divisionCounter = 0;
    public int divisionId;
    public RememberedDivision commander;
    public List<RememberedDivision> subordinates = new List<RememberedDivision>();
    public List<Soldier> soldiers;
    public float speed = 10;
    public List<Order> possibleOrders;
    public float maxSightDistance = 0;
    public List<Order> orderQueue = new List<Order>();
    public Order ongoingOrder = null;
    public GameObject divisonPrefab;
    public GameObject display;
    public Dictionary<int, DivisionController> visibleDivisions = new Dictionary<int, DivisionController>();
    public Dictionary<int, RememberedDivision> rememberedDivisions = new Dictionary<int, RememberedDivision>();

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
        UpdateDisplay();
    }

    public virtual void DoOrders()
    {
        if(GameManager.instance.isPaused)
        {
            ongoingOrder.Pause();
        }
        else if (ongoingOrder.GetType() != typeof(EmptyOrder))
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
            ongoingOrder = orderQueue[0];
            orderQueue.RemoveAt(0);
            ongoingOrder.Start();
            ContinueOrder();
        }
    }

    void CheckFOW()
    {
        List<DivisionController> allDivisions = GameManager.instance.allDivisions;
        
        visibleDivisions = new Dictionary<int, DivisionController>();
        //fing the largest sight distance
        foreach (Soldier soldier in soldiers)
        {
            if(maxSightDistance < soldier.sightDistance)
            {
                maxSightDistance = soldier.sightDistance;
            }
        }

        //find all of the divisions inside our sight distance and update their remembered position
        foreach(DivisionController division in allDivisions)
        {
            Vector3 end = division.transform.position;
            Vector3 start = transform.position;
            float dis = (end - start).magnitude;
            if(dis < maxSightDistance)
            {
                visibleDivisions[division.divisionId] = division;
                RememberedDivision rememberedDivision;
                bool hasRemembered = rememberedDivisions.TryGetValue(division.divisionId, out rememberedDivision);
                if (!hasRemembered)
                {
                    rememberedDivision = division.GenerateRememberedDivision();
                    rememberedDivisions.Add(rememberedDivision.divisionId, rememberedDivision);
                }
                else
                {
                    rememberedDivision = division.GenerateRememberedDivision();
                    rememberedDivisions[division.divisionId] = rememberedDivision;
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

    void UpdateDisplay()
    {
        display.GetComponentInChildren<Text>().text = name;
    }

    public void Init(RememberedDivision commander)
    {
        this.commander = commander;
        possibleOrders = new List<Order>();
        name = "division " + divisionId;
    }

    public virtual void Init()
    {
        possibleOrders.Add(new Move(this,null,new Vector3()));
        ongoingOrder = new EmptyOrder();
        GameManager.instance.RefreshAllDivisons();
        divisionId = divisionCounter++;
        name = "division : " + divisionId;
    }
    
    public void SendOrderTo(RememberedDivision to, Order order)
    {
        //follow commander tree to get there
        List<RememberedDivision> pathToDivision = FindDivisionInSubordinates(GenerateRememberedDivision(), to, new List<RememberedDivision>());
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

    public void SendMessenger(RememberedDivision to, Order order)
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
        messenger.ReceiveOrder(new FindDivision(messenger, this, to));
        messenger.ReceiveOrder(new SendMessage(messenger, this, orders, to));
        //messenger.ReceiveOrder(new FindDivision(messenger, this, messenger));
       // messenger.ReceiveOrder
    }

    public virtual DivisionController CreateChild(List<Soldier> soldiersForChild)
    {
        GameObject newDivision = Instantiate(divisonPrefab);
        DivisionController newController = newDivision.GetComponent<DivisionController>();
        newController.Init(GenerateRememberedDivision());
        newController.soldiers = soldiersForChild;
        newDivision.transform.position = this.transform.position;
        newDivision.transform.rotation = this.transform.rotation;
        subordinates.Add(newController.GenerateRememberedDivision());
        GameManager.instance.RefreshAllDivisons();
        return newController;
    }

    public List<RememberedDivision> FindDivisionInSubordinates(RememberedDivision start, RememberedDivision end, List<RememberedDivision> prev_)
    {
        List<RememberedDivision> prev = new List<RememberedDivision>(prev_);
        //RememberedDivision rememberedStart = start.GenerateRememberedDivision();
        prev.Add(start);
        if (start.divisionId == end.divisionId)
        {
            return prev;
        }

        foreach(RememberedDivision division in start.subordinates)
        {
            List<RememberedDivision> temp = FindDivisionInSubordinates(division, end, prev);
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

    public List<RememberedDivision> GetAllSubordinates()
    {
        List<RememberedDivision> allSubordinates = new List<RememberedDivision>();
        allSubordinates.Add(GenerateRememberedDivision());
        foreach(RememberedDivision division in subordinates)
        {
            allSubordinates.AddRange(division.GetAllSubordinates());
        }

        return allSubordinates;
    }

    public void AbsorbDivision(DivisionController other)
    {
        TransferSoldiers(other.soldiers);
        //kick him out of his commanders subordinate list
        RememberedDivision parent = other.commander;
        
        if(parent != null)
        {
            parent.RemoveSubordinate(other.GenerateRememberedDivision());
        }
        GameObject.Destroy(other.gameObject);
        GameManager.instance.RefreshAllDivisons();
    }

    public void RemoveSubordinate(RememberedDivision division)
    {
        subordinates.Remove(division);
    }

    public RememberedDivision GenerateRememberedDivision()
    {
        List<RememberedDivision> subordinatesDeep = new List<RememberedDivision>();
        foreach(RememberedDivision subordinate in subordinates)
        {

            //subordinatesDeep.Add()
        }

        RememberedDivision rememberedDivision =
            new RememberedDivision(
                transform.position, GetComponent<Rigidbody>().velocity, orderQueue, soldiers, divisionId, Time.time, possibleOrders, subordinatesDeep, commander);

        return rememberedDivision;
    }

    public bool FindVisibleDivision(int divisionID, out DivisionController division)
    {
        division = null;
        return visibleDivisions.TryGetValue(divisionID, out division);
    }
}
