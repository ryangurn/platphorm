using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{

    public string MyEnemy;

    Vector3 positionBeforePursuit;

    bool inPursuit;

    void Start()
    {
        inPursuit = false;
        
    }

    void FixedUpdate()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyEnemy);

        foreach (GameObject g in units)
        {
            float distanceToEnemy = Vector3.Distance(g.transform.position, transform.position);

            if (distanceToEnemy < 7 && !inPursuit)
            {
                inPursuit = true;
                positionBeforePursuit = transform.position;
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g.transform.position;
                return;
            }
            else if (distanceToEnemy < 7 && distanceToEnemy > 2 && inPursuit)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = g.transform.position;
                return;
            }
        }

        if (inPursuit)
        {
            inPursuit = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = positionBeforePursuit;

        }

    }
}
