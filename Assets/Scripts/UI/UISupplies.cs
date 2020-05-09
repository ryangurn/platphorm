using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISupplies : MonoBehaviour
{
    public string MyTeam;
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
