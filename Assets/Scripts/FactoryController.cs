using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script attaches to the canvas
public class FactoryController : MonoBehaviour
{
  public GameObject Funds;
  public GameObject Completed;

  private int constant = 1;
  public float ConstructionSpeedMulti = 1;
  public float ConstructionEfficiencyMulti = 1;
  public float ConstructionTaskMulti = 1;
  public int HarvesterCost = 500;
  public int BasicCost = 300;
  public int AdvancedCost = 800;

  public GameObject HarvesterText;
  public GameObject BasicText;
  public GameObject AdvancedText;
  public GameObject FullQueue;

  private FactoryBuildPlayer fbp; //this holds the factory for the player
  private bool queueFull = false; //can't build if we're busy, so we keep track
  private GameObject playerSupplyInventory;
  private List<string> buildQueue = new List<string>();
  private GameObject[] slots;
  private List<bool> busyQueue = new List<bool>();

  void UpdateText()
  {
    HarvesterText.GetComponent<Text>().text = "HARVESTER ("+(constant/(ConstructionEfficiencyMulti) * HarvesterCost)+")";
    BasicText.GetComponent<Text>().text = "BASIC UNIT ("+(constant/(ConstructionEfficiencyMulti) * BasicCost)+")";
    AdvancedText.GetComponent<Text>().text = "ADVANCED UNIT ("+(constant/(ConstructionEfficiencyMulti) * AdvancedCost)+")";
  }

  //this starting function locates the player factory
  void Start()
  {
    busyQueue.Add(false);

    UpdateText();

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

  public void AddTask()
  {
    busyQueue.Add(false); //make productive capacity one unit longer, and it starts out not busy (false)
  }

  void FixedUpdate() //basic handler to check for work and update queue Count/size
  {
    UpdateText();

    queueFull = buildQueue.Count > 4; //if it's 5, it's full
    string currentWork;

    for (int i = 0; i < busyQueue.Count; i++) //for each construction space
    {
      if (buildQueue.Count < (i + 1) || busyQueue[i]) //check to make sure the queue has a pending unit that's not already under cunstruction
      continue;


      currentWork = buildQueue[i]; //there's work to do
      busyQueue[i] = true;

      if (currentWork == "Basic")
      {
        StartCoroutine(BuildBasic(i));
      }
      else if (currentWork == "Advanced")
      {
        StartCoroutine(BuildAdvanced(i));
      }
      else //if currentWork == "Harvester"
      {
        StartCoroutine(BuildHarvester(i));
      }
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

  public void TryBuild(string unitType) //this enqueues, as well as determines sufficient funding
  {
    if (unitType == "Basic")
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= (int)(constant / ConstructionEfficiencyMulti) * BasicCost
      && !queueFull)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= (int)(constant / ConstructionEfficiencyMulti) * BasicCost;
        buildQueue.Add("Basic");
        StartCoroutine(CompletedAlready());
      }
      else if (queueFull)
      {
        StartCoroutine(QueueFull());
      }
      else
      StartCoroutine(InsufficientFunds());
    }
    else if (unitType == "Advanced")
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= (int)(constant / ConstructionEfficiencyMulti) * AdvancedCost
      && !queueFull)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= (int)(constant / ConstructionEfficiencyMulti) * AdvancedCost;
        buildQueue.Add("Advanced");
        StartCoroutine(CompletedAlready());
      }
      else if (queueFull)
      {
        StartCoroutine(QueueFull());
      }
      else
      StartCoroutine(InsufficientFunds());
    }
    else //Harvester
    {
      if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies >= (int)(constant / ConstructionEfficiencyMulti) * HarvesterCost
      && !queueFull)
      {
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= (int)(constant / ConstructionEfficiencyMulti) * HarvesterCost;
        buildQueue.Add("Harvester");
        StartCoroutine(CompletedAlready());
      }
      else if (queueFull)
      {
        StartCoroutine(QueueFull());
      }
      else
      StartCoroutine(InsufficientFunds());
    }
  }


  //all of these functions check if you have the right amount of money, and "build" the units if you do, set the NSF flag is you're out of funding
  private IEnumerator BuildBasic(int queuePos)
  {

    yield return new WaitForSeconds((constant / ConstructionEfficiencyMulti) * 3);
    fbp.SpawnBasic();
    buildQueue.Remove("Basic");

    busyQueue[queuePos] = false;
  }

  private IEnumerator BuildAdvanced(int queuePos)
  {

    yield return new WaitForSeconds((constant / ConstructionEfficiencyMulti) * 10);
    fbp.SpawnAdvanced();
    buildQueue.Remove("Advanced");
    busyQueue[queuePos] = false;
  }

  private IEnumerator BuildHarvester(int queuePos)
  {

    yield return new WaitForSeconds((constant / ConstructionEfficiencyMulti) * 5);
    fbp.SpawnHarvester();
    buildQueue.Remove("Harvester");
    busyQueue[queuePos] = false;
  }

  IEnumerator InsufficientFunds()
  {
    //Upgraded.SetActive(false);
    Completed.SetActive(false);
    Funds.SetActive(true);
    Funds.GetComponent<AudioSource>().Play();
    FullQueue.SetActive(false);
    yield return new WaitForSeconds(2);
    Funds.SetActive(false);
  }

  IEnumerator CompletedAlready()
  {
    Funds.SetActive(false);
    //Upgraded.SetActive(false);
    Completed.SetActive(true);
    Completed.GetComponent<AudioSource>().Play();
    FullQueue.SetActive(false);
    yield return new WaitForSeconds(1);
    Completed.SetActive(false);
  }

  IEnumerator QueueFull()
  {
    FullQueue.SetActive(true);
    FullQueue.GetComponent<AudioSource>().Play();
    Funds.SetActive(false);
    //Upgraded.SetActive(false);
    Completed.SetActive(false);
    yield return new WaitForSeconds(1);
    FullQueue.SetActive(false);
  }

  public bool PendingUnits()
  {
    if (buildQueue.Count > 0)
    return true;
    else
    return false;
  }

}
