using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionMenuManager : MonoBehaviour {
    public static DivisionMenuManager instance;
    public GameObject backButton;
    public GameObject divisionMenuItemPrefab;
    public RememberedDivision commander;
    public DivisionController playerDivision;
    public List<RememberedDivision> subordinates = new List<RememberedDivision>();
    public List<DivisionMenuItem> displayedItems = new List<DivisionMenuItem>();
    public GameObject divisionMenuPanel;
    private Coroutine menuCycler;
    
    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        playerDivision = GameManager.instance.localPlayer.generalDivision;
    }

    void Update()
    {
        if(subordinates.Count != commander.subordinates.Count)
        {
            if(menuCycler != null)
            {
                StopCoroutine(menuCycler);
            }

            menuCycler = StartCoroutine(CycleMenuItems());
        }
        else
        {
            UpdateMenuItems();
        }
    }

    //make sure that the enu items are created properly
    IEnumerator CycleMenuItems()
    {
        RegenerateMenuItems();
        yield return new WaitForSeconds(.5f);
        yield return CycleMenuItems();
    }

    void RegenerateMenuItems()
    {
        foreach(DivisionMenuItem item in displayedItems)
        {
            Destroy(item.gameObject);
        }
        displayedItems.Clear();
        subordinates = commander.subordinates;
        foreach(RememberedDivision subordinate in subordinates)
        {
            RememberedDivision rememberedDivision;
            
            bool foundDivision = 
                playerDivision.rememberedDivisions.TryGetValue(subordinate.divisionId, out rememberedDivision);
            if (!foundDivision)
            {
                continue;
            }

            GameObject item = Instantiate(divisionMenuItemPrefab);
            item.transform.SetParent(divisionMenuPanel.transform);
            item.GetComponent<DivisionMenuItem>().division = rememberedDivision;
            displayedItems.Add(item.GetComponent<DivisionMenuItem>());
        }
    }

    void UpdateMenuItems()
    {

    }

    void GoBack()
    {
        //go the the persons commander
        //disable back button if he doesent have a commander
    }

    public void DivisionMenuItemClicked(RememberedDivision division)
    {
        commander = division;
        RegenerateMenuItems();
        //bing up context menu for that division on the bottom right, but do different actions
        GameManager.instance.localPlayer.select(division);
        //need to send messengers to discover its location
        //refresh the division menu with his stuff
    }
}
