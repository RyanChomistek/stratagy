using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour {

    public DivisionController playerDivision;
    public GameObject fowTileParent;
    public GameObject FOWTilePrefab;
    public List<List<GameObject>> FOWTiles;
    public GameObject fogOfWarQuad;
    // Use this for initialization
    void Awake () {
        //InitFOWTiles(width, height, scale);
	}
	
	// Update is called once per frame
	void Update () {
        /*
        //set displays for every division depending on whether it should be seen
        foreach(DivisionController division in GameManager.instance.allDivisions)
        {
            DivisionController dummy;
            division.display.SetActive(playerDivision.FindVisibleDivision(division.divisionId, out dummy));
        }
        for(int i = 0; i < height * scale; i++)
        {
            for(int j = 0; j < width * scale; j++)
            {
                float dis = (FOWTiles[i][j].transform.position - playerDivision.transform.position).magnitude;
                FOWTiles[i][j].SetActive(dis > playerDivision.maxSightDistance);
            }
        }
        */
        Vector3 playerPositionScreen = Camera.main.WorldToScreenPoint(playerDivision.transform.position);
        Vector3 sightRangeScreen = Camera.main.WorldToScreenPoint(playerDivision.transform.position + new Vector3(playerDivision.maxSightDistance, 0,0));
        float sightRadiusScreen = (sightRangeScreen - playerPositionScreen).magnitude;
        //Debug.Log(sightRadiusScreen / playerDivision.maxSightDistance);
        fogOfWarQuad.GetComponent<MeshRenderer>().material.SetVector("_PlayerPos",new Vector4(playerPositionScreen.x, playerPositionScreen.y, 0,0));
        fogOfWarQuad.GetComponent<MeshRenderer>().material.SetFloat("_SightRange", sightRadiusScreen);
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
                temp.transform.parent = fowTileParent.transform;
                temp.transform.position = new Vector3(i/ (float)s, j/ (float)s);
                temp.transform.localScale = new Vector3(1/(float)s,1/(float)s,1);
                
                rowOfTiles.Add(temp);
            }
        }
    }
}
