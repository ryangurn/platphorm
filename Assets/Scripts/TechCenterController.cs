using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TechCenterController : MonoBehaviour
{

  public GameObject PlayerResources;
  public GameObject PlayerRefinery;
  public GameObject Funds;
  public GameObject Upgraded;
  public GameObject Completed;

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

  public GameObject RangeText;
  public GameObject SpeedText;
  public GameObject StrengthText;
  public GameObject HarvesterText;

  private GameObject[] units;
  private List<GameObject> harvesters = new List<GameObject>();
  private List<GameObject> attackUnits = new List<GameObject>();

  void Start()
  {
    UpdateText();
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

  void UpdateText()
  {
    RangeText.GetComponent<Text>().text = "+ RANGE OF ATTACK ("+RangePerCost+")";
    SpeedText.GetComponent<Text>().text = "+ SPEED OF UNITS ("+SpeedPerCost+")";
    StrengthText.GetComponent<Text>().text = "+ STRENGTH OF UNITS ("+StrengthPerCost+")";
    HarvesterText.GetComponent<Text>().text = "+ 1 SLOT IN HARVESTER ("+HarvestPerCost+")";
  }


  void FixedUpdate()
  {
    UpdateText();
  }

  public void ChangeRangeOfAttack()
  {
    if (RangeCounter >= RangeLimit)
    {
      StartCoroutine(UpgradedAlready());
      return;
    }
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
    StartCoroutine(CompletedAlready());
    return;
  }

  public void ChangeSpeedOfUnits()
  {
    if (SpeedCounter >= SpeedLimit)
    {
      StartCoroutine(UpgradedAlready());
      return;
    }
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
    StartCoroutine(CompletedAlready());
    return;
  }

  public void ChangeStrengthOfUnits()
  {
    if (StrengthCounter >= StrengthLimit)
    {
      StartCoroutine(UpgradedAlready());
      return;
    }
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
    StartCoroutine(CompletedAlready());
    return;
  }

  public void AddMultiplierToHarvester()
  {
    if (HarvestCounter >= HarvestLimit)
    {
      StartCoroutine(UpgradedAlready());
      return;
    }
    if (PlayerResources.GetComponent<SupplyInventory>().Supplies < HarvestPerCost)
    {
      StartCoroutine(InsufficientFunds());
      return;
    }

    PlayerResources.GetComponent<SupplyInventory>().Supplies -= HarvestPerCost;
    HarvestCounter++;

    PlayerRefinery.GetComponent<DepositOre>().HarvesterMultiplier += HarvestDelta;
    StartCoroutine(CompletedAlready());
    return;
  }

  IEnumerator InsufficientFunds()
  {
    Upgraded.SetActive(false);
    Completed.SetActive(false);
    Funds.SetActive(true);
    yield return new WaitForSeconds(2);
    Funds.SetActive(false);
  }

  IEnumerator UpgradedAlready()
  {
    Funds.SetActive(false);
    Completed.SetActive(false);
    Upgraded.SetActive(true);
    yield return new WaitForSeconds(1);
    Upgraded.SetActive(false);
  }

  IEnumerator CompletedAlready()
  {
    Funds.SetActive(false);
    Upgraded.SetActive(false);
    Completed.SetActive(true);
    yield return new WaitForSeconds(1);
    Completed.SetActive(false);
  }
}
