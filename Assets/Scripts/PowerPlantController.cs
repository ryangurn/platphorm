using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPlantController : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject PlayerResources;
    public GameObject Funds;

    public GameObject SpeedText;
    public GameObject EfficiencyText;
    public GameObject TaskText;

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
      EfficiencyText.GetComponent<Text>().text = "+ FACTORY SPEED ("+EfficiencyPerCost+")";
      TaskText.GetComponent<Text>().text = "FACTORY MULTITASK ("+TaskPerCost+")";
    }

    void Start()
    {
      UpdateText();
    }

    void FixedUpdate()
    {
      UpdateText();
    }

    public void ChangeSpeedOfFactory()
    {
      if (SpeedCounter >= SpeedLimit) return;
      if (PlayerResources.GetComponent<SupplyInventory>().Supplies < SpeedPerCost)
      {
        StartCoroutine(InsufficientFunds());
        return;
      }

      PlayerResources.GetComponent<SupplyInventory>().Supplies -= SpeedPerCost;
      SpeedCounter++;


      Canvas.GetComponent<Construction>().ConstructionSpeedMulti += SpeedDelta;
    }

    public void ChangeEfficiencyOfFactory()
    {
      if (EfficiencyCounter >= EfficiencyLimit) return;
      if (PlayerResources.GetComponent<SupplyInventory>().Supplies < EfficiencyPerCost)
      {
        StartCoroutine(InsufficientFunds());
        return;
      }

      PlayerResources.GetComponent<SupplyInventory>().Supplies -= EfficiencyPerCost;
      EfficiencyCounter++;


      Canvas.GetComponent<Construction>().ConstructionEfficiencyMulti += EfficiencyDelta;
    }

    public void ChangeMultitaskingOfFactory()
    {

    }

    IEnumerator InsufficientFunds()
    {
      Funds.SetActive(true);
      yield return new WaitForSeconds(2);
      Funds.SetActive(false);
    }
}
