using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionMenuManager : MonoBehaviour {
    public static DivisionMenuManager instance;
    public GameObject backButton;
    public GameObject divisionMenuItemPrefab;
    public DivisionController commander;
    public List<DivisionController> subordinates = new List<DivisionController>();
    public List<DivisionMenuItem> displayedItems = new List<DivisionMenuItem>();
    public GameObject divisionMenuPanel;

    void Awake()
    {
        instance = this;    
    }

    void Update()
    {
        if(subordinates.Count != commander.subordinates.Count)
        {
            RefreshMenuItems();
        }
    }

    void RefreshMenuItems()
    {
        foreach(DivisionMenuItem item in displayedItems)
        {
            Destroy(item.gameObject);
        }
        displayedItems.Clear();
        subordinates = commander.subordinates;
        foreach(DivisionController subordinate in subordinates)
        {
            GameObject item = Instantiate(divisionMenuItemPrefab);
            item.transform.parent = divisionMenuPanel.transform;
            //need to find division in remembered divisions
            RememberedDivision rememberedDivision = commander.rememberedDivisions.Find(x => x.divisionId == subordinate.divisionId);
            item.GetComponent<DivisionMenuItem>().division = rememberedDivision;
            displayedItems.Add(item.GetComponent<DivisionMenuItem>());
        }
    }

    void GoBack()
    {
        //go the the persons commander
        //disable back button if he doesent have a commander
    }

    public void DivisionMenuItemClicked(RememberedDivision division)
    {
        //bing up context menu for that division on the bottom right, but do different actions
        GameManager.instance.localPlayer.select(division);
        //need to send messengers to discover its location
        //refresh the division menu with his stuff
    }
}
