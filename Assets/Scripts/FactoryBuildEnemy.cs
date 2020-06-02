using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to enemy factory. This is more fully-featured than the player version of the script since th eplayer one gets "help" from the Construction.cs attached to the camera
public class FactoryBuildEnemy : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester; //these are links to prefab objects
    private GameObject enemySupplyInventory; 
    public bool busy = false;
    private AudioSource finishedSound;

    void Start()
    {
        finishedSound = gameObject.GetComponent<AudioSource>();
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
        if (busy) //can't do anything it we're busy
            return;

        if (nearbyHarvesterCount < 1) //if we don't have at least one harvester..
        {
            if (enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 500) //make one if we can
            {
                enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
                StartCoroutine(SpawnHarvester());
            }
            else //or save up
                return;
        }

        int randomNum = Mathf.FloorToInt(Random.Range(0f, 100f)); //pick a random integer

        if (enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 800
            && randomNum < 40) //if we can afford an advanced unit, there's a chance we make on, or a harvester
        {           
 
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
            StartCoroutine(SpawnAdvanced());       

        }
        else if (enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 500
            && nearbyHarvesterCount < 3
            && randomNum < 60 && randomNum > 40)
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
            StartCoroutine(SpawnHarvester());
        }

        else if (enemySupplyInventory.GetComponent<SupplyInventory>().Supplies >= 300
            && randomNum < 80 && randomNum > 60) 
        {
            enemySupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
            StartCoroutine(SpawnBasic());
        }
    }


    public IEnumerator SpawnBasic() //this is used for creating a delay before making a unit
    {
        
        busy = true;
        yield return new WaitForSeconds(5);
        FinishedSound();
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
        FinishedSound();
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
        FinishedSound();
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;

        Instantiate(Harvester, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        busy = false;
    }

    private void FinishedSound()
    {
        if (!finishedSound.isPlaying)
        {
            finishedSound.Play();
        }
    }
}
