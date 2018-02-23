using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionMenuItem : MonoBehaviour {

    public RememberedDivision division;

    public void OnClick()
    {
        DivisionMenuManager.instance.DivisionMenuItemClicked(division);
    }
}
