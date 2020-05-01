using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterInventory : MonoBehaviour
{
    public bool isFull = false;

    void FixedUpdate()
    {
        MeshRenderer[] ChildrenMR = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in ChildrenMR)
        {
            if (mr.gameObject.name == "Payload" && isFull == true)
            {
                // Make the selected symbol mesh visible or not
                mr.enabled = true;
            }

            else if(mr.gameObject.name == "Payload")
            {
                mr.enabled = false;
            }
        }
    }
}
