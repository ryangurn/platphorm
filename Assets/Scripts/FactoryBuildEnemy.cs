using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuildEnemy : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester;

    void Start()
    {
        StartCoroutine(SpawnBasic());
        StartCoroutine(SpawnAdvanced());
    }

    private IEnumerator SpawnBasic()
    {
        while (true)
        {
            yield return new WaitForSeconds(11);
            Vector3 pos = transform.position;
            float x = pos.x - Random.Range(.5f, 5f);
            float y = pos.y;
            float z = pos.z;
            float angle = Random.Range(0.0f, 90.0f);
            Instantiate(BasicUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));
        }
    }



    private IEnumerator SpawnAdvanced()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            Vector3 pos = transform.position;
            float x = pos.x - Random.Range(.5f, 5f);
            float y = pos.y;
            float z = pos.z;
            float angle = Random.Range(0.0f, 90.0f);
            Instantiate(AdvancedUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));
        }
    }
}
