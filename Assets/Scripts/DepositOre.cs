using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this attaches to the refineries
public class DepositOre : MonoBehaviour
{
    public string MyTeam; //shared enemy/player script

    private SupplyInventory MySupplyInventory;

    void Start()
    {
        MySupplyInventory = GameObject.FindGameObjectWithTag(MyTeam + "SupplyInventory").GetComponent<SupplyInventory>();
    }
    //called by harvester
    public void Deposit()
    {   //look for refinery's team's supply inventory and put the supplies in the inventory. There's an element of randomness here to keep the game more interensting
        MySupplyInventory.Supplies += Mathf.FloorToInt(Random.Range(50f, 500f));
    }
}
