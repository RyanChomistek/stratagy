using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionMenuManager : MonoBehaviour {
    public GameObject backButton;
    public GameObject divisionMenuItem;
    public DivisionController commander;
    List<DivisionController> subordinates;
    List<DivisionMenuItem> displayedItems;
    void Update()
    {
        if(subordinates.Count != commander.subordinates.Count)
        {
            subordinates = commander.subordinates;
        }
    }

    void RefreshMenuItems()
    {

    }

    void GoBack()
    {
        //go the the persons commander
        //disable back button if he doesent have a commander
    }

    public void DivisionMenuItemClicked(RememberedDivision division)
    {
        //bing up context menu for that division on the bottom right, but do different actions
        //need to send messengers to discover its location
        //refresh the division menu with his stuff
    }
}
