using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplayManager : MonoBehaviour {
    public static OrderDisplayManager instance { get; set; }
    public List<GameObject> displays;
    private List<GameObject> displayedOrders = new List<GameObject>();
    [SerializeField]
    GameObject orderPrefab;

    public void addOrderSet(List<Order> orders)
    {
        foreach(Order order in orders)
        {
            string name = order.GetType().Name;
            GameObject temp = Instantiate(orderPrefab);
            temp.transform.GetChild(0).GetComponent<Text>().text = name;
            temp.GetComponent<Button>().onClick.AddListener(delegate { onOrderClicked(order); });
            addToDisplay(temp);
        }
    }

    void addToDisplay(GameObject order)
    {
        order.transform.SetParent(displays[displayedOrders.Count % displays.Count].transform);
        displayedOrders.Add(order);
    }

    public void clearOrders()
    {
        foreach(GameObject order in displayedOrders)
        {
            Destroy(order);
        }
        displayedOrders.Clear();
    }

    public void onOrderClicked(Order order)
    {
        order.onClickedInUI();
    }

    void Start()
    {
        instance = this;
    }
}
