using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputController : MonoBehaviour {
    public static InputController instance { get; set; }
    public Player player;
    public delegate void onClick(Vector3 mouseLoc);
    public List<onClick> onClickCallbacks = new List<onClick>();
    
    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            foreach(onClick callback in onClickCallbacks)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                callback(mousePos);
            }
            onClickCallbacks.Clear();
        }
        
	}

    public void registerOnClickCallBack(onClick callback)
    {
        onClickCallbacks.Add(callback);
    }
}
