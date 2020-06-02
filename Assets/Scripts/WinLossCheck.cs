using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to canvas
public class WinLossCheck : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;

    public FactoryController playerConstructionScript;
    private List<FactoryBuildEnemy> enemyFactoryScripts = new List<FactoryBuildEnemy>();

    void Start()
    {
        GameObject[] enemyBuildings = GameObject.FindGameObjectsWithTag("EnemyBuilding");

        foreach (GameObject building in enemyBuildings)
        {
            if (building.GetComponent<FactoryBuildEnemy>())
                enemyFactoryScripts.Add(building.GetComponent<FactoryBuildEnemy>());
        }

    }
    //set the win active when conditions below are met
    void FixedUpdate()
    {
        bool enemyBusy = false;

        foreach (FactoryBuildEnemy script in enemyFactoryScripts)
        {
            if (script.busy)
            {
                enemyBusy = true;
                break;
            }
        }

        if (GameObject.FindGameObjectWithTag("EnemySupplyInventory").GetComponent<SupplyInventory>().Supplies < 300
            && !GameObject.FindGameObjectWithTag("EnemyHealth").GetComponentInChildren<UIHealth>().HasUnits
            && !enemyBusy)
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
