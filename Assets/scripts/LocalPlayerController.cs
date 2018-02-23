using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour {
    public static LocalPlayerController instance { get; set; }
    [SerializeField]
    private DivisionController selected;

    public DivisionController generalDivision;

    [SerializeField]
    bool UIwaitingForSelection = false;
    public delegate void responseToUI(DivisionController division);
    responseToUI UIResponse;

    public delegate void responseToUIRemembered(RememberedDivision division);
    responseToUIRemembered UIResponseRememberedDivision;

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
            OrderDisplayManager.instance.ClearOrders();
            OrderDisplayManager.instance.AddOrderSet(selected.possibleOrders);
        }
        else
        {
            UIResponse(divisionController);
        }
    }

    public void select(RememberedDivision divisionController)
    {
        if (!UIwaitingForSelection)
        {
            //bing up order ui
            OrderDisplayManager.instance.ClearOrders();
            OrderDisplayManager.instance.AddOrderSet(divisionController.possibleOrders);
        }
        else
        {
            UIResponseRememberedDivision(divisionController);
        }
    }
}
