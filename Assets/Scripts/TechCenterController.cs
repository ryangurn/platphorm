using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TechCenterController : MonoBehaviour
{

  public GameObject PlayerResources;
  public GameObject PlayerRefinery;
  public GameObject Funds;

  public int RangeDelta = 1;
  public int RangeLimit = 2;
  public int RangePerCost = 300;

  public float SpeedDelta = 1.5f;
  public int SpeedLimit = 2;
  public int SpeedPerCost = 300;

  public float StrengthDelta = 2.0f;
  public int StrengthLimit = 2;
  public int StrengthPerCost = 800;

  public int HarvestDelta = 1;
  public int HarvestLimit = 2;
  public int HarvestPerCost = 250;

  private int RangeCounter = 0;
  private int SpeedCounter = 0;
  private int StrengthCounter = 0;
  private int HarvestCounter = 0;


  private GameObject[] units;
  private List<GameObject> harvesters = new List<GameObject>();
  private List<GameObject> attackUnits = new List<GameObject>();

  void Start()
  {

    units = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject unit in units)
    {
      if (unit.name == "Harvester") {
        harvesters.Add(unit);
      }
      else
      {
        attackUnits.Add(unit);
      }

    }
  }

  public void ChangeRangeOfAttack()
  {
    if (RangeCounter >= RangeLimit) return;
    if (PlayerResources.GetComponent<SupplyInventory>().Supplies < RangePerCost)
    {
        StartCoroutine(InsufficientFunds());
        return;
    }

    PlayerResources.GetComponent<SupplyInventory>().Supplies -= RangePerCost;
    RangeCounter++;

    foreach (GameObject attack in attackUnits)
    {
      attack.GetComponent<Attack>().AttackRange = attack.GetComponent<Attack>().AttackRange + RangeDelta;
    }
    return;
  }

  public void ChangeSpeedOfUnits()
  {
    if (SpeedCounter >= SpeedLimit) return;
    if (PlayerResources.GetComponent<SupplyInventory>().Supplies < SpeedPerCost)
    {
      StartCoroutine(InsufficientFunds());
      return;
    }

    PlayerResources.GetComponent<SupplyInventory>().Supplies -= SpeedPerCost;
    SpeedCounter++;

    foreach (GameObject unit in units)
    {
      unit.GetComponent<NavMeshAgent>().speed = unit.GetComponent<NavMeshAgent>().speed + SpeedDelta;
    }
    return;
  }

  public void ChangeStrengthOfUnits()
  {
    if (StrengthCounter >= StrengthLimit) return;
    if (PlayerResources.GetComponent<SupplyInventory>().Supplies < StrengthPerCost)
    {
      StartCoroutine(InsufficientFunds());
      return;
    }

    PlayerResources.GetComponent<SupplyInventory>().Supplies -= StrengthPerCost;
    StrengthCounter++;

    foreach (GameObject unit in units)
    {
      unit.GetComponent<Health>().HealthLevel = unit.GetComponent<Health>().HealthLevel + StrengthDelta;
    }
    return;
  }

  public void AddMultiplierToHarvester()
  {
    if (HarvestCounter >= HarvestLimit) return;
    if (PlayerResources.GetComponent<SupplyInventory>().Supplies < HarvestPerCost)
    {
      StartCoroutine(InsufficientFunds());
      return;
    }

    PlayerResources.GetComponent<SupplyInventory>().Supplies -= HarvestPerCost;
    HarvestCounter++;

    PlayerRefinery.GetComponent<DepositOre>().HarvesterMultiplier += HarvestDelta;

    return;
  }

  IEnumerator InsufficientFunds()
  {
    Funds.SetActive(true);
    yield return new WaitForSeconds(2);
    Funds.SetActive(false);
  }
}
