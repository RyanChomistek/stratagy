using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour {
    public static LocalPlayerController instance { get; set; }
    [SerializeField]
    private DivisionController selected;

    public DivisionController generalDivision;

    bool UIwaitingForSelection = false;
    public delegate void responseToUI(DivisionController division);
    responseToUI UIResponse;
    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void select(DivisionController divisionController)
    {
        if (!UIwaitingForSelection)
        {
            selected = divisionController;
            //bing up order ui
            OrderDisplayManager.instance.clearOrders();
            OrderDisplayManager.instance.addOrderSet(selected.possibleOrders);
        }
        else
        {
            UIResponse(divisionController);
            //responseToUI(divisionController);
        }

    }
}
