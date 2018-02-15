using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour {

    public DivisionController playerDivision;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //set displays for every division depending on whether it should be seen
        foreach(DivisionController division in GameManager.instance.allDivisions)
        {
            division.display.SetActive(playerDivision.visibleDivisions.Contains(division));
        }
    }
}
