using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour {

    public DivisionController playerDivision;

    public GameObject FOWTilePrefab;
    public List<List<GameObject>> FOWTiles;
    private int height = 10;
    private int width = 10;
    int scale = 10;
    // Use this for initialization
    void Start () {
        InitFOWTiles(width, height, scale);
	}
	
	// Update is called once per frame
	void Update () {
        //set displays for every division depending on whether it should be seen
        foreach(DivisionController division in GameManager.instance.allDivisions)
        {
            division.display.SetActive(playerDivision.visibleDivisions.Contains(division));
        }
        for(int i = 0; i < height * scale; i++)
        {
            for(int j = 0; j < width * scale; j++)
            {
                float dis = (FOWTiles[i][j].transform.position - playerDivision.transform.position).magnitude;
                FOWTiles[i][j].SetActive(dis > playerDivision.maxSightDistance);
            }
        }
    }

    public void InitFOWTiles(int h, int w, int s)
    {
        FOWTiles = new List<List<GameObject>>();
        
        for(int i = 0; i < h * s; i++)
        {
            List<GameObject> rowOfTiles = new List<GameObject>();
            FOWTiles.Add(rowOfTiles);
            for (int j = 0; j < w * s; j++)
            {
                GameObject temp = Instantiate(FOWTilePrefab);
                temp.transform.position = new Vector3(i/ (float)s, j/ (float)s);
                temp.transform.localScale = new Vector3(1/(float)s,1/(float)s,1);
                rowOfTiles.Add(temp);
            }
        }
    }
}
