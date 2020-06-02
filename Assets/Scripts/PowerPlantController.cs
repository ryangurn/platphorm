using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPlantController : MonoBehaviour
{
    public GameObject ConstructionQueue;
    public GameObject PlayerResources;
    public GameObject Funds;
    public GameObject Upgraded;
    public GameObject Completed;

    public GameObject SpeedText;
    public GameObject EfficiencyText;
    public GameObject TaskText;

    private AudioSource upgradeSound;

    public int SpeedDelta = 1;
    public int SpeedLimit = 2;
    public int SpeedPerCost = 300;
    private int SpeedCounter = 0;

    public int EfficiencyDelta = 1;
    public int EfficiencyLimit = 2;
    public int EfficiencyPerCost = 300;
    private int EfficiencyCounter = 0;

    public int TaskDelta = 1;
    public int TaskLimit = 2;
    public int TaskPerCost = 300;
    private int TaskCounter = 0;

    void UpdateText()
    {
      SpeedText.GetComponent<Text>().text = "+ FACTORY SPEED ("+SpeedPerCost+")";
      EfficiencyText.GetComponent<Text>().text = "+ FACTORY EFFICIENCY ("+EfficiencyPerCost+")";
      TaskText.GetComponent<Text>().text = "+ FACTORY MULTITASK ("+TaskPerCost+")";
    }

    void Start()
    {
      UpdateText();
      upgradeSound = gameObject.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
      UpdateText();
    }

    public void ChangeSpeedOfFactory()
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


      ConstructionQueue.GetComponent<FactoryController>().ConstructionSpeedMulti += SpeedDelta;
      StartCoroutine(CompletedAlready());
      return;
    }

    public void ChangeEfficiencyOfFactory()
    {
      if (EfficiencyCounter >= EfficiencyLimit)
      {
        StartCoroutine(UpgradedAlready());
        return;
      }
      if (PlayerResources.GetComponent<SupplyInventory>().Supplies < EfficiencyPerCost)
      {
        StartCoroutine(InsufficientFunds());
        return;
      }


    PlayerResources.GetComponent<SupplyInventory>().Supplies -= EfficiencyPerCost;
      EfficiencyCounter++;


      ConstructionQueue.GetComponent<FactoryController>().ConstructionEfficiencyMulti += EfficiencyDelta;
      StartCoroutine(CompletedAlready());
      return;
    }

    public void ChangeMultitaskingOfFactory()
    {
        if (TaskCounter >= TaskLimit)
        {
            StartCoroutine(UpgradedAlready());
            return;
        }
        if (PlayerResources.GetComponent<SupplyInventory>().Supplies < TaskPerCost)
        {
            StartCoroutine(InsufficientFunds());
            return;
        }


        PlayerResources.GetComponent<SupplyInventory>().Supplies -= TaskPerCost;
        TaskCounter++;

        ConstructionQueue.GetComponent<FactoryController>().AddTask();
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
    if (!upgradeSound.isPlaying)
    {
        upgradeSound.Play();
    }
    Funds.SetActive(false);
      Upgraded.SetActive(false);
      Completed.SetActive(true);
      yield return new WaitForSeconds(1);
      Completed.SetActive(false);
    }
}
