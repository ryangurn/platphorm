using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHarvester : MonoBehaviour
{
    private bool isFull = false;

    public string myTeam;
    private Vector3 myOrePileLocation;

    void FixedUpdate()
    {
        MeshRenderer[] ChildrenMR = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in ChildrenMR)
        {
            if (mr.gameObject.name == "Payload" && isFull)
            {
                // Make the selected symbol mesh visible or not
                mr.enabled = true;
            }

            else if (mr.gameObject.name == "Payload")
            {
                mr.enabled = false;
            }
        }

        if (!isFull)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject bestOre = gameObject;
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < shortestDistance)
                {
                    shortestDistance = Vector3.Distance(ore.transform.position, transform.position);
                    bestOre = ore;
                }

                if (Vector3.Distance(ore.transform.position, transform.position) < 3)
                {
                    ore.GetComponent<OreRemaining>().OreContent -= 5f;
                    isFull = true;
                    
                }
            }

            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = bestOre.transform.position;

        }


        else
        {
            GameObject[] friendlies = GameObject.FindGameObjectsWithTag(myTeam);
            foreach (GameObject g in friendlies)
            {
                if (g.GetComponent<DepositOre>())
                {
                    GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g.transform.position;

                    if (Vector3.Distance(g.transform.position, transform.position) < 5)
                    {
                        isFull = false;
                        g.GetComponent<DepositOre>().Deposit();
                    }
                }

            }
        }




    }
}
