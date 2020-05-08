using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    private bool isFull = false;
    private bool hasOrePileSet = false;

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

            else if(mr.gameObject.name == "Payload")
            {
                mr.enabled = false;
            }
        }

        if (hasOrePileSet && !isFull)
        {
            hasOrePileSet = false;
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

        if (!hasOrePileSet)
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

        if (!isFull)
        {
            GameObject[] ores = GameObject.FindGameObjectsWithTag("Ore");
            foreach (GameObject ore in ores)
            {
                if (Vector3.Distance(ore.transform.position, transform.position) < 3.5)
                {
                    ore.GetComponent<OreRemaining>().OreContent -= 5f;
                    isFull = true;
                    break;
                }
            }
        }

        else
        {
            GameObject[] friendlies = GameObject.FindGameObjectsWithTag(myTeam);
            foreach (GameObject g in friendlies)
            {
                if (g.GetComponent<DepositOre>())
                {
                    GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g.transform.position;

                    if (Vector3.Distance(g.transform.position, transform.position) < 4)
                    {
                        isFull = false;
                        g.GetComponent<DepositOre>().Deposit();
                    }
                }

                
                
            }
        }




    }
}
