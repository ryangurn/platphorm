using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to enemy factory. This is more fully-featured than the player version of the script since th eplayer one gets "help" from the Construction.cs attached to the camera
public class FactoryBuildEnemy : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester; //these are links to prefab objects
    private SupplyInventory enemySupplyScript; 
    public bool busy = false;
    private AudioSource finishedSound;
    private List<FactoryBuildEnemy> enemyFactoryScripts = new List<FactoryBuildEnemy>(); //other enemy factories, but not this one
    public bool MyTurn = false;

    void Start()
    {
        finishedSound = gameObject.GetComponent<AudioSource>();
        enemySupplyScript = GameObject.FindGameObjectWithTag("EnemySupplyInventory").GetComponent<SupplyInventory>();

        GameObject[] enemyBuildings = GameObject.FindGameObjectsWithTag("EnemyBuilding");

        foreach (GameObject building in enemyBuildings)
        {
            if (building != gameObject && building.GetComponent<FactoryBuildEnemy>()) //if the building isn't the current building, but has a script for enemy construction, add that script to the list
                enemyFactoryScripts.Add(building.GetComponent<FactoryBuildEnemy>());  
        }
    }

    void FixedUpdate()
    {
        if (!MyTurn || busy) //give way to the other factory/ies if it's not our turn. Also, stop if we're busy
            return;

        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy"); //make sure we have an enemy harvester

        int nearbyHarvesterCount = 0;
        int randomNum = Random.Range(0, 100); //for odds of what we'll try to make

        foreach (GameObject g in enemyUnits)
        {
            if (g.GetComponent<EnemyHarvester>() != null && Vector3.Distance(g.transform.position, gameObject.transform.position) < 40)
            {
                nearbyHarvesterCount++;
            }
        }

        if (nearbyHarvesterCount < 1                       //if we don't have at least one harvester, we need to build one
            || nearbyHarvesterCount < 3 && randomNum%10 == 0) //or if we have fewer than 3, 10% chance we build another
        {
            StartCoroutine(SpawnHarvester());
        }

        else if (randomNum < 60) //build advanced unit 60% chance if we have 3 harvesters, 54% if we have 1-2, no chance if no harvester
        {      
            StartCoroutine(SpawnAdvanced());
        }
        else //build basic unit 40% chance if we have 3 harvesters, 36% if we have 1-2, no chance if no harvester
        { 
            StartCoroutine(SpawnBasic());
        }
        
    }


    public IEnumerator SpawnBasic() //this is used for creating a delay before making a unit
    {
        busy = true;
        while (enemySupplyScript.Supplies < 300) //save up until we can get a basic unit   
            yield return new WaitForSeconds(1); 
        
        enemySupplyScript.Supplies -= 300;

        int randomNum = Random.Range(0, enemyFactoryScripts.Count); //hand off construction to other factory/ies
        MyTurn = false;
        enemyFactoryScripts[randomNum].MyTurn = true;

        //start building basic
        yield return new WaitForSeconds(5);

        FinishedSound(); //done
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
        while (enemySupplyScript.Supplies < 800) //save up until we can get an advanced unit   
            yield return new WaitForSeconds(1);

        enemySupplyScript.Supplies -= 800;

        int randomNum = Random.Range(0, enemyFactoryScripts.Count); //hand off construction to other factory/ies
        MyTurn = false;
        enemyFactoryScripts[randomNum].MyTurn = true;

        //start work on advanced
        yield return new WaitForSeconds(10);

        FinishedSound(); //done
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
        while (enemySupplyScript.Supplies < 500) //save up until we can get a harvester  
            yield return new WaitForSeconds(1);

        enemySupplyScript.Supplies -= 500;

        int randomNum = Random.Range(0, enemyFactoryScripts.Count); //hand off construction to other factory/ies
        MyTurn = false;
        enemyFactoryScripts[randomNum].MyTurn = true;

        //start work on harvester
        yield return new WaitForSeconds(3);


        FinishedSound(); //done
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
