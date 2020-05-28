using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    private bool hasOrePileSet = false;
    public Vector3 myOrePileLocation;
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

        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("PlayerBuilding"); //this is for the refinery
        foreach (GameObject g in friendlies)
        {
            if (g.GetComponent<DepositOre>())
            {
                refinery = g;
            }
        }
    }

    void FixedUpdate()
    {
        GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
        //first task is setting an ore pile location by proximity to harvester
        if (!hasOrePileSet) 
        {
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < 3)
                {
                    myOrePileLocation = ore.transform.position;
                    hasOrePileSet = true;
                    break;
                }
            }
        }
        else //if it is set, let's make sure there's still ore there, else we are setting it for false
        {
            hasOrePileSet = false; //checking to make sure there is ore still available at the currently-set ore pile
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, myOrePileLocation) < 7)
                {
                    hasOrePileSet = true;
                    break;
                }
            }
        }

        if (fullSymbol.enabled) //if full, always go to the refinery and deposit when close enough
        {

            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = refinery.transform.position;

            if (Vector3.Distance(refinery.transform.position, transform.position) < 5)
            {
                fullSymbol.enabled = false;
                refinery.GetComponent<DepositOre>().Deposit();
            }               
        }

        else //if you're not full, then you're eligible to pick up ore. So we'll do that when close enough
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = myOrePileLocation; //make sure you're heading to ore 

            foreach (GameObject ore in ores) //then deposit and stop when we've picked it up
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < 3.5)
                {
                    ore.GetComponent<OreRemaining>().OreContent -= 1;
                    fullSymbol.enabled = true;
                    return;
                }
            }                                   
        }
    }
}
