using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    private bool hasOrePileSet = false;
    private Vector3 myOrePileLocation;
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

        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("Player"); //this is for the refinery
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
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < 3.5)
                {
                    ore.GetComponent<OreRemaining>().OreContent -= 1;
                    fullSymbol.enabled = true;
                    break;
                }
            }          
        }

        if (!hasOrePileSet) //if we don't have an ore pile set, we better get one. Once we're close to ore, this is set here
        {
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
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

        else if (!fullSymbol.enabled) //if we do know where to go for ore, head there, but only if we're empty
        {
            hasOrePileSet = false; //checking to make sure there is ore still available at the currently-set ore pile
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, myOrePileLocation) < 7)
                {
                    hasOrePileSet = true;
                    break;
                }
            }        
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = myOrePileLocation;
        }


    }
}
