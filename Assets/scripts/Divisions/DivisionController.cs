using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionController : MonoBehaviour {
    public DivisionController commander;
    public List<DivisionController> subordinates;
    public List<Soldier> soldiers { get; set; }
    public float speed { get; set; }
    public List<Order> possibleOrders;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void initOrders()
    {
        possibleOrders.Add(new Move(null,new Vector3()));

    }
}
