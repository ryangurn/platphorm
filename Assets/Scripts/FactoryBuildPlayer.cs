using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this whole script is an offload of functions for Construction.cs to call from the main camera (which handles construction, but only for the player)
public class FactoryBuildPlayer : MonoBehaviour
{
    public GameObject BasicUnit, AdvancedUnit, Harvester;
    public bool isSelected;

    public int BasicAttackRange;
    public float BasicSpeed;
    public float BasicHealthLevel;
    public int AdvancedAttackRange;
    public float AdvancedSpeed;
    public float AdvancedHealthLevel;
    public float HarvesterSpeed;
    public float HarvesterHealthLevel;

    private AudioSource finishedSound;

    void Start()
    {
        finishedSound = gameObject.GetComponent<AudioSource>();
        BasicAttackRange = BasicUnit.GetComponent<Attack>().AttackRange;
      BasicSpeed = BasicUnit.GetComponent<NavMeshAgent>().speed;
      BasicHealthLevel = BasicUnit.GetComponent<Health>().HealthLevel;
      AdvancedAttackRange = AdvancedUnit.GetComponent<Attack>().AttackRange;
      AdvancedSpeed = AdvancedUnit.GetComponent<NavMeshAgent>().speed;
      AdvancedHealthLevel = AdvancedUnit.GetComponent<Health>().HealthLevel;
      HarvesterSpeed = Harvester.GetComponent<NavMeshAgent>().speed;
      HarvesterHealthLevel = Harvester.GetComponent<Health>().HealthLevel;
    }

    public void SpawnBasic()
    {
        FinishedSound();
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;
        float angle = Random.Range(0.0f, 90.0f);
        GameObject n = Instantiate(BasicUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, angle, 0.0f));
        n.GetComponent<Attack>().AttackRange = BasicAttackRange;
        n.GetComponent<NavMeshAgent>().speed = BasicSpeed;
        n.GetComponent<Health>().HealthLevel = BasicHealthLevel;
    }

    public void SpawnAdvanced()
    {
        FinishedSound();
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;

        GameObject n = Instantiate(AdvancedUnit, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        n.GetComponent<Attack>().AttackRange = AdvancedAttackRange;
        n.GetComponent<NavMeshAgent>().speed = AdvancedSpeed;
        n.GetComponent<Health>().HealthLevel = AdvancedHealthLevel;
    }

    public void SpawnHarvester()
    {
        FinishedSound();
        Vector3 pos = transform.position;
        float x = pos.x - Random.Range(.5f, 5f);
        float y = pos.y;
        float z = pos.z;

        GameObject n = Instantiate(Harvester, new Vector3(x, y, z), Quaternion.Euler(0.0f, 0f, 0.0f));
        n.GetComponent<NavMeshAgent>().speed = HarvesterSpeed;
        n.GetComponent<Health>().HealthLevel = HarvesterHealthLevel;
    }

    private void FinishedSound()
    {
        if (!finishedSound.isPlaying)
        {
            finishedSound.Play();
        }
    }
}
