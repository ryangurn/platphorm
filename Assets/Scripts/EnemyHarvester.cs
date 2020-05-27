using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHarvester : MonoBehaviour
{
    private MeshRenderer fullSymbol;
    private GameObject refinery;

    void Awake() //locate the unit's fullSymbol and refinery when it's instantiated
    {
        MeshRenderer[] ChildrenMR = GetComponentsInChildren<MeshRenderer>(); //this is for the unit's own full symbol
        foreach (MeshRenderer mr in ChildrenMR)
        {
            if (mr.gameObject.name == "Payload")
            {
                fullSymbol = mr;
            }
        }

        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("EnemyBuilding"); //this is to find the refinery
        float closestDistance = Mathf.Infinity;
        foreach (GameObject g in friendlies)
        {
           if (g.GetComponent<DepositOre>() && Vector3.Distance(gameObject.transform.position, g.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(gameObject.transform.position, g.transform.position);
                refinery = g;
            }
        }

    }

    void FixedUpdate()
    {    

        if (!fullSymbol.enabled) //if harvester is empty,
        {
            float shortestDistance = Mathf.Infinity;
            GameObject bestOre = gameObject;
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
            foreach (GameObject ore in ores) //get the best piece of ore available. I.e. the shortest distance to one
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < shortestDistance)
                {
                    shortestDistance = Vector3.Distance(ore.transform.position, transform.position);
                    bestOre = ore;
                }

                if (Vector3.Distance(ore.transform.position, transform.position) < 3) //pick ore up if you're close enough
                {
                    ore.GetComponent<OreRemaining>().OreContent -= 1;
                    fullSymbol.enabled = true;
                    
                }
            }

            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = bestOre.transform.position;
        }

        //if full
        else
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = refinery.transform.position;

            if (Vector3.Distance(refinery.transform.position, transform.position) < 5)
            {
                fullSymbol.enabled = false;
                refinery.GetComponent<DepositOre>().Deposit();
            }
        }


    }
}
