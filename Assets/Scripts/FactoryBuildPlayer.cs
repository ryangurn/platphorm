using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this whole script is an offload of functions for Construction.cs to call from the main camera (which handles construction, but only for the player)
public class FactoryBuildPlayer : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester;
    public bool isSelected;

    public void SpawnBasic()
    {
        
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        float angle = Random.Range(0.0f, 90.0f);
        Instantiate(BasicUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));
        
    }

    public void SpawnAdvanced()
    {
       
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        
        Instantiate(AdvancedUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        
    }

    public void SpawnHarvester()
    {

        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        
        Instantiate(Harvester, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));

    }
}
