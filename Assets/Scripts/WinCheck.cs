using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCheck : MonoBehaviour
{
    public GameObject Win;

    void FixedUpdate()
    {

        if (GameObject.FindGameObjectWithTag("EnemySupplyInventory").GetComponent<SupplyInventory>().Supplies < 300
            && !gameObject.GetComponentInChildren<UIEnemyHealth>().HasUnits)
        {
            Win.SetActive(true);
        }
    }
}
