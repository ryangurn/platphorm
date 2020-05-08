using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositOre : MonoBehaviour
{
    public string MyTeam;
    public void Deposit()
    {
        GameObject.FindGameObjectWithTag(MyTeam + "SupplyInventory").GetComponent<SupplyInventory>().Supplies += 100;
    }
}
