using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    private Vector3 rallyPoint;  //this is the bridge they defend. In later builds, this will be public, and different enemy bases will have their own rally points we'll set from Unity
    private bool inPursuit = false; //we keep track of this so that we know if we're in a guarding or more offensive state
    private GameObject pursuing; //this is the immediate adversary of the enemy unit
    private float pursueDistance = 7;

    void Start()
    {
        rallyPoint = new Vector3(-42f, 5.5f, -.5f);
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
    }

    void FixedUpdate()
    {
        float distanceToPlayerUnit;

        if (pursuing == null && inPursuit) //if the player unit died, we don't need to pursue it anymore. Go back to the rally point
        {
            inPursuit = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
        }

        if (inPursuit) //if we're pursuing a player unit
        {
            distanceToPlayerUnit = Vector3.Distance(pursuing.transform.position, transform.position);

            if (distanceToPlayerUnit < 7 && distanceToPlayerUnit > 2) //adjust your position relative to it so that it's at a reasonable distance
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = pursuing.transform.position;
                return; //don't waste resources looking for other targets
            }
            else //it's outrun this unit, so return to guard
            {
                inPursuit = false;
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
            }
        }
        //if not in pursuit
        GameObject[] units = GameObject.FindGameObjectsWithTag("Player"); //find every player unit   
        float shortestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
      
        foreach (GameObject g in units) //for all player units
        {
            distanceToPlayerUnit = Vector3.Distance(g.transform.position, transform.position);

            if (distanceToPlayerUnit < pursueDistance && distanceToPlayerUnit < shortestDistance)
                closestUnit = g;

        }

        if (closestUnit != null) //if there's something within range, setup pursuit
        {

            inPursuit = true;
            pursuing = closestUnit;
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = pursuing.transform.position;
            return;
        }

        //if no player units are nearby, see if we're in a group

        units = GameObject.FindGameObjectsWithTag("Enemy");
        float distanceToOtherUnit;
        int nearbyCt = 0;

        foreach (GameObject g in units)
        {
            if (g.GetComponent<EnemyHarvester>()) //harvester doesn't count
                continue;

            distanceToOtherUnit = Vector3.Distance(g.transform.position, transform.position);

            if (distanceToOtherUnit < 7)
            {
                nearbyCt++;
            }
        }
        //if we are in a group, rally at the player's base and aggressively pursue units
        if (nearbyCt > 6)
        {
            rallyPoint = new Vector3(0f, 2f, 0f);
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
            pursueDistance = Mathf.Infinity;
        }

    }

    void OnTriggerEnter(Collider other) //prevents bumping around when there's a common target
    {

        if (Vector3.Distance(GetComponent<UnityEngine.AI.NavMeshAgent>().destination, transform.position) < 6f) //if you bump into a trigger, which are on all units, and you're close enough to your destination, stop trying to go there
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = transform.position;
        }


    }
}
