using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script attaches to the camera
public class Construction : MonoBehaviour
{
    private FactoryBuildPlayer fbp; //this holds the factory for the player
    private bool busy = false; //can't build if we're busy, so we keep track
    private bool sufficientFunds = true;
    private GameObject playerSupplyInventory;

    //this starting function locates the player factory
    void Start()
    {
        playerSupplyInventory = GameObject.FindGameObjectWithTag("PlayerSupplyInventory");
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject g in playerUnits)
        {
            if (g.GetComponent<FactoryBuildPlayer>())
            {
                fbp = g.GetComponent<FactoryBuildPlayer>();
            }
        }
    }

    //all of these functions check if you have the right amount of money, and "build" the units if you do, set the NSF flag is you're out of funding
    private IEnumerator BuildBasic()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 300 < 0)
        {
            StartCoroutine(NotSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
        yield return new WaitForSeconds(3);
        fbp.SpawnBasic();
        busy = false;
    }

    private IEnumerator BuildAdvanced()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 800 < 0)
        {
            StartCoroutine(NotSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
        yield return new WaitForSeconds(10);
        fbp.SpawnAdvanced();
        busy = false;

    }

    private IEnumerator BuildHarvester()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 500 < 0)
        {
            StartCoroutine(NotSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
        yield return new WaitForSeconds(5);
        fbp.SpawnHarvester();
        busy = false;
    }

    private IEnumerator NotSufficientFunds()
    {
        sufficientFunds = false;
        yield return new WaitForSeconds(2);
        sufficientFunds = true;
        busy = false;
    }

    void OnGUI() //GUI objects
    {

        if (busy && sufficientFunds)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 110,
                                         Screen.height - 70,
                                         220,
                                         25), "Please Wait. Under Construction.", "box");
            
            GUILayout.EndArea();
        }

        else if (busy && !sufficientFunds)
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

                StartCoroutine(BuildBasic());
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 75,
                                         Screen.height - 70,
                                         200,
                                         30), "", "box");

            if (GUILayout.Button("Construct Advanced Unit-$800"))
            {

                StartCoroutine(BuildAdvanced());
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 135,
                                         Screen.height - 70,
                                         170,
                                         30), "", "box");

            if (GUILayout.Button("Construct Harvester-$500"))
            {
                StartCoroutine(BuildHarvester());
            }

            GUILayout.EndArea();
        }

    }
}
