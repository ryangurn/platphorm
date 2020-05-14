using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is analagous to health, but for pieces of ore
public class OreRemaining : MonoBehaviour
{
    public int OreContent = 20; //scaled-down ore content (there's a random multiplier when depositing from refinery

    void FixedUpdate()
    {
        TextMesh tm = GetComponentInChildren<TextMesh>();

        if (Mathf.FloorToInt(OreContent) <= 0) //it's depleated, remove the game object
        {
            Destroy(gameObject);
        }
    }

}
