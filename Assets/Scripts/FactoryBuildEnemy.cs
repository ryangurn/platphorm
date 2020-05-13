using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuildEnemy : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester;
    private GameObject enemySupplyInventory;
    private bool busy = false;

    void Start()
    {
        enemySupplyInventory = GameObject.FindGameObjectWithTag("EnemySupplyInventory");
    }

    void FixedUpdate()
    {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");

        bool haveHarvester = false;
        foreach (GameObject g in enemyUnits)
        {
            if (g.GetComponent<EnemyHarvester>() != null)
            {
                haveHarvester = true;
                break;
            }
        }

        if (!haveHarvester && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 500 && !busy)
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
            StartCoroutine(SpawnHarvester());
        }

        if (!busy && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 800)
        {
            if (Mathf.FloorToInt(Random.Range(0f, 100f)) % 2 == 0)
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
                StartCoroutine(SpawnAdvanced());
            }
            else
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
                StartCoroutine(SpawnBasic());
            }
        }
        else if (!busy && enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 300)
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
            StartCoroutine(SpawnBasic());
        }

    }



    public IEnumerator SpawnBasic()
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
