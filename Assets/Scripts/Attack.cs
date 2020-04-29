using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string MyEnemy;
 
    void FixedUpdate()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyEnemy);

        foreach (GameObject g in units)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < 3)
            {
                g.GetComponent<Health>().HealthLevel -= .015f;
                g.GetComponent<Health>().RunDamageAnimation();
            }
        }



    }
}
