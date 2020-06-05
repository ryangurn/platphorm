using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to canvas
public class WinLossCheck : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;

    public FactoryController playerConstructionScript;

    void Start()
    {
        //Deleted broken mechanic.
    }
    //set the win active when conditions below are met
    void FixedUpdate()
    {
        if (!GameObject.FindGameObjectWithTag("EnemyHealth").GetComponentInChildren<UIHealth>().HasUnits)
        {
            Win.SetActive(true);
        }
        else if (GameObject.FindGameObjectWithTag("PlayerSupplyInventory").GetComponent<SupplyInventory>().Supplies < 300
            && !GameObject.FindGameObjectWithTag("PlayerHealth").GetComponentInChildren<UIHealth>().HasUnits
            && !playerConstructionScript.PendingUnits())
        {
            Lose.SetActive(true);
        }
    }
}
