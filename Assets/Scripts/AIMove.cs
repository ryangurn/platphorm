using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{

    public string MyEnemy = "Player";

    private Vector3 rallyPoint;

    private bool inPursuit = false;
    private GameObject pursuing;


    void Start()
    {
        rallyPoint = new Vector3(-42f, 5.5f, -.5f);
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
    }

    void FixedUpdate()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyEnemy);

        float distanceToPlayerUnit;

        if (inPursuit && pursuing != null)
        {
            distanceToPlayerUnit = Vector3.Distance(pursuing.transform.position, transform.position);

            if (distanceToPlayerUnit < 7 && distanceToPlayerUnit > 2) //continue pursuing the unit
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = pursuing.transform.position;
                return;
            }
            else
            {
                inPursuit = false;
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = rallyPoint;
            }
        }
        //if not in pursuit
        foreach (GameObject g in units) //for all player units
        {
            distanceToPlayerUnit = Vector3.Distance(g.transform.position, transform.position);

            if (distanceToPlayerUnit < 7) //if it's near a player unit, get into pursuit mode and chase them
            {
                inPursuit = true;
                pursuing = g;
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g.transform.position;
                return;
            }
        }

    }

    void OnTriggerEnter(Collider other) //prevents bumping around
    {

        if (Vector3.Distance(GetComponent<UnityEngine.AI.NavMeshAgent>().destination, transform.position) < 6f)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = transform.position;
        }


    }
}
