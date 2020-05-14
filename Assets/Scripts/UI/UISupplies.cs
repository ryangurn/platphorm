using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attaches to child of canvas
public class UISupplies : MonoBehaviour
{
    public string MyTeam; //used for both teams, so this is necessary
    Text supplies;

    void Start()
    {
        supplies = GetComponent<Text>();
    }

    void Update()
    {
        supplies.text = MyTeam + " Supplies: " + GameObject.FindGameObjectWithTag(MyTeam + "SupplyInventory").GetComponent<SupplyInventory>().Supplies;
    }
}
