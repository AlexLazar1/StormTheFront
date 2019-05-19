using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell 
{
    public Boolean walkable { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public GameObject go { get; set; }
    public GameObject troop { get; set; }
    public Boolean selected { get; set; }
    public string troopName { get; set; } = "";
    public Boolean isPlayerTroop { get; set; } = false;
    public int numberOfTroops { get; set; } = 0;

    public void Update()
    {
        Debug.Log("upd" + x);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(x+"-"+y);
        }
    }
}
