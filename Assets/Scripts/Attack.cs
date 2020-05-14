using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string MyEnemy; //this component is shared, so we need to specify who were're trying to attack and damage
 
    void FixedUpdate()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyEnemy); //look for all enemy units

        foreach (GameObject g in units)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < 3) //if they're nearby, reduce their health and run their damage "animation"
            {
                g.GetComponent<Health>().HealthLevel -= .015f;
                g.GetComponent<Health>().RunDamageAnimation();
            }
        }

    }
}
