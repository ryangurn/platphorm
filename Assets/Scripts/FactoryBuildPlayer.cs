using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuildPlayer : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester;




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
        float angle = Random.Range(0.0f, 90.0f);
        Instantiate(AdvancedUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));
        
    }

    public void SpawnHarvester()
    {


        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        float angle = Random.Range(0.0f, 90.0f);
        Instantiate(Harvester, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));

    }
}
