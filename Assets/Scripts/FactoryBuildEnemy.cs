using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to enemy factory. This is more fully-featured than the player version of the script since th eplayer one gets "help" from the Construction.cs attached to the camera
public class FactoryBuildEnemy : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester; //these are links to prefab objects
    private GameObject enemySupplyInventory; 
    private bool busy = false;

    void Start()
    {
        enemySupplyInventory = GameObject.FindGameObjectWithTag("EnemySupplyInventory");
    }

    void FixedUpdate()
    {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy"); //make sure we have an enemy harvester

        int nearbyHarvesterCount = 0;

        foreach (GameObject g in enemyUnits)
        {
            if (g.GetComponent<EnemyHarvester>() != null && Vector3.Distance(g.transform.position, gameObject.transform.position) < 40)
            {
                nearbyHarvesterCount++;
            }
        }

        if (nearbyHarvesterCount < 1 && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 500 && !busy) //if we don't and we can afford it, make one
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
            StartCoroutine(SpawnHarvester());
        }

        if (!busy && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 800) //if we can afford an advanced unit, 50/50 chance we make one (or we make a basic unit)
        {
            int randomNum = Mathf.FloorToInt(Random.Range(0f, 100f));

            if (randomNum < 50)
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
                StartCoroutine(SpawnAdvanced());
            }
            else if (nearbyHarvesterCount < 3)
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
                StartCoroutine(SpawnHarvester());              
            }
            else
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
                StartCoroutine(SpawnBasic());
            }
        }
        else if (!busy && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 300) //couldn't afford advanced, so make basic
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
            StartCoroutine(SpawnBasic());
        }

    }



    public IEnumerator SpawnBasic() //this is used for creating a delay before making a unit
    {
        busy = true;
        yield return new WaitForSeconds(5);
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        
        Instantiate(BasicUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        busy = false;
    }



    private IEnumerator SpawnAdvanced()
    {
        busy = true;
        yield return new WaitForSeconds(10);
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;

        Instantiate(AdvancedUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        busy = false;
    }

    private IEnumerator SpawnHarvester()
    {
        busy = true;
        yield return new WaitForSeconds(3);
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;

        Instantiate(Harvester, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        busy = false;
    }
}
