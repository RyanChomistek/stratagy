using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; set; }
    public LocalPlayerController localPlayer;
    public List<AIController> AIs;
    public List<DivisionController> generals;
    public List<DivisionController> allDivisions;
    public float gameSpeed = 1;
    public bool isPaused = false;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshAllDivisons()
    {
        allDivisions = new List<DivisionController>(GameObject.FindObjectsOfType<DivisionController>());
     
        /*
        foreach (DivisionController general in generals)
        {
            allDivisions.AddRange(general.GetAllSubordinates());
        }
        */
    }


}
