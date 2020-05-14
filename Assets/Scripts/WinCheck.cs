using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to canvas
public class WinCheck : MonoBehaviour
{
    public GameObject Win;
    //set the win active when conditions below are met
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("EnemySupplyInventory").GetComponent<SupplyInventory>().Supplies < 300
            && !GameObject.FindGameObjectWithTag("EnemyHealth").GetComponentInChildren<UIHealth>().HasUnits)
        {
            Win.SetActive(true);
        }
    }
}
