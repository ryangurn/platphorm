using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantController : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject PlayerResources;

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


    public void ChangeSpeedOfFactory()
    {
      if (SpeedCounter >= SpeedLimit) return;
      if (PlayerResources.GetComponent<SupplyInventory>().Supplies < SpeedPerCost) return;

      PlayerResources.GetComponent<SupplyInventory>().Supplies -= SpeedPerCost;
      SpeedCounter++;


      Canvas.GetComponent<Construction>().ConstructionSpeedMulti += SpeedDelta;
    }

    public void ChangeEfficiencyOfFactory()
    {
      if (EfficiencyCounter >= EfficiencyLimit) return;
      if (PlayerResources.GetComponent<SupplyInventory>().Supplies < EfficiencyPerCost) return;

      PlayerResources.GetComponent<SupplyInventory>().Supplies -= EfficiencyPerCost;
      EfficiencyCounter++;


      Canvas.GetComponent<Construction>().ConstructionEfficiencyMulti += EfficiencyDelta;
    }

    public void ChangeMultitaskingOfFactory()
    {

    }
}
