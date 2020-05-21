using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script attaches to the canvas
public class Construction : MonoBehaviour
{
  private FactoryBuildPlayer fbp; //this holds the factory for the player
  private bool queueFull = false; //can't build if we're busy, so we keep track
  private bool sufficientFunds = true;
  private GameObject playerSupplyInventory;
  private List<string> buildQueue = new List<string>();
  private GameObject[] slots;
  private bool busy = false;


  //this starting function locates the player factory
  void Start()
  {
    // get slots
    slots = GameObject.FindGameObjectsWithTag("PlayerSlots");


    // get buildings
    playerSupplyInventory = GameObject.FindGameObjectWithTag("PlayerSupplyInventory");
    GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerBuilding");

    foreach (GameObject g in playerUnits)
    {
      if (g.GetComponent<FactoryBuildPlayer>())
      {
        fbp = g.GetComponent<FactoryBuildPlayer>();
      }
    }
  }

  void FixedUpdate() //basic handler to check for work and update queue Count/size
  {
    queueFull = buildQueue.Count > 4; //if it's 5, it's full
    string currentWork;


    if (busy || buildQueue.Count == 0) //if we're busy, nothing else we can do
    return;

    currentWork = buildQueue[0]; //there's work to do
    busy = true;

    if (currentWork == "Basic")
    {
      StartCoroutine(BuildBasic());
    }
    else if (currentWork == "Advanced")
    {
      StartCoroutine(BuildAdvanced());
    }
    else
    {
      StartCoroutine(BuildHarvester());
    }

  }

  void Update()
  {

    int i = 0; //we're sharing the iterator through two loops
    for (; i < buildQueue.Count; i++) //go up to the places in the queue that are actually in use, and update the slots accordingly
    {

      if(buildQueue[i] == "Advanced")
      {
        slots[i].transform.GetChild(0).gameObject.SetActive(true);
        slots[i].transform.GetChild(1).gameObject.SetActive(false);
        slots[i].transform.GetChild(2).gameObject.SetActive(false);
      }
      else if (buildQueue[i] == "Basic")
      {
        slots[i].transform.GetChild(0).gameObject.SetActive(false);
        slots[i].transform.GetChild(1).gameObject.SetActive(true);
        slots[i].transform.GetChild(2).gameObject.SetActive(false);
      }
      else //if (buildQueue[i] == "Harvester")
      {
        slots[i].transform.GetChild(0).gameObject.SetActive(false);
        slots[i].transform.GetChild(1).gameObject.SetActive(false);
        slots[i].transform.GetChild(2).gameObject.SetActive(true);
      }

    }

    for (; i < 5; i++) //for the rest of the slots, make them blank
    {
        slots[i].transform.GetChild(0).gameObject.SetActive(false);
        slots[i].transform.GetChild(1).gameObject.SetActive(false);
        slots[i].transform.GetChild(2).gameObject.SetActive(false);
    }

  }

  private void HaveSufficientFunds(string unitType) //this enqueues, as well as determines sufficient funding
  {
    if (unitType == "Basic")
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= 300)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
        buildQueue.Add("Basic");
      }
      else
      StartCoroutine(NotSufficientFunds());
    }
    else if (unitType == "Advanced")
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= 800)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
        buildQueue.Add("Advanced");

      }
      else
      StartCoroutine(NotSufficientFunds());
    }
    else //Harvester
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= 500)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
        buildQueue.Add("Harvester");

      }
      else
      StartCoroutine(NotSufficientFunds());
    }
  }


  //all of these functions check if you have the right amount of money, and "build" the units if you do, set the NSF flag is you're out of funding
  private IEnumerator BuildBasic()
  {

    yield return new WaitForSeconds(3);
    fbp.SpawnBasic();
    buildQueue.RemoveAt(0);

    busy = false;
  }

  private IEnumerator BuildAdvanced()
  {

    yield return new WaitForSeconds(10);
    fbp.SpawnAdvanced();
    buildQueue.RemoveAt(0);
    busy = false;
  }

  private IEnumerator BuildHarvester()
  {

    yield return new WaitForSeconds(5);
    fbp.SpawnHarvester();
    buildQueue.RemoveAt(0);
    busy = false;
  }

  private IEnumerator NotSufficientFunds()
  {
    sufficientFunds = false;
    yield return new WaitForSeconds(2);
    sufficientFunds = true;
  }

  void OnGUI() //GUI objects
  {
    //don't paint the building gui if factory isn't selected

    if (!fbp.isSelected)
    return;

    if (queueFull && sufficientFunds)
    {
      GUILayout.BeginArea(new Rect(Screen.width / 2 - 110,
      Screen.height - 70,
      220,
      25), "Please Wait. Queue Full.", "box");

      GUILayout.EndArea();
    }

    else if (!queueFull && !sufficientFunds)
    {
      GUILayout.BeginArea(new Rect(Screen.width / 2 - 85,
      Screen.height - 70,
      170,
      25), "Insufficient Funds.", "box");

      GUILayout.EndArea();
    }

    else
    {

      GUILayout.BeginArea(new Rect(Screen.width / 2 - 260,
      Screen.height - 70,
      170,
      30), "", "box");

      if (GUILayout.Button("Construct Basic Unit-$300"))
      {

        HaveSufficientFunds("Basic");
      }

      GUILayout.EndArea();

      GUILayout.BeginArea(new Rect(Screen.width / 2 - 75,
      Screen.height - 70,
      200,
      30), "", "box");

      if (GUILayout.Button("Construct Advanced Unit-$800"))
      {

        HaveSufficientFunds("Advanced");
      }

      GUILayout.EndArea();

      GUILayout.BeginArea(new Rect(Screen.width / 2 + 135,
      Screen.height - 70,
      170,
      30), "", "box");

      if (GUILayout.Button("Construct Harvester-$500"))
      {
        HaveSufficientFunds("Harvester");
      }

      GUILayout.EndArea();
    }

  }
}
