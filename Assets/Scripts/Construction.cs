using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    private FactoryBuildPlayer fbp;
    private bool busy = false;
    private bool sufficientFunds = true;
    private GameObject playerSupplyInventory;

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

    private IEnumerator buildBasic()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 300 < 0)
        {
            StartCoroutine(notSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 300;
        yield return new WaitForSeconds(3);
        fbp.SpawnBasic();
        busy = false;
    }

    private IEnumerator buildAdvanced()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 800 < 0)
        {
            StartCoroutine(notSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 800;
        yield return new WaitForSeconds(10);
        fbp.SpawnAdvanced();
        busy = false;

    }

    private IEnumerator buildHarvester()
    {
        busy = true;
        if (playerSupplyInventory.GetComponent<SupplyInventory>().Supplies - 500 < 0)
        {
            StartCoroutine(notSufficientFunds());
            yield break;
        }
        playerSupplyInventory.GetComponent<SupplyInventory>().Supplies -= 500;
        yield return new WaitForSeconds(5);
        fbp.SpawnHarvester();
        busy = false;
    }

    private IEnumerator notSufficientFunds()
    {
        sufficientFunds = false;
        yield return new WaitForSeconds(2);
        sufficientFunds = true;
        busy = false;
    }

    void OnGUI()
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

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 225,
                                         Screen.height - 70,
                                         140,
                                         30), "", "box");


            if (GUILayout.Button("Construct Basic Unit"))
            {

                StartCoroutine(buildBasic());
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 75,
                                         Screen.height - 70,
                                         170,
                                         30), "", "box");


            if (GUILayout.Button("Construct Advanced Unit"))
            {

                StartCoroutine(buildAdvanced());
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 105,
                                         Screen.height - 70,
                                         140,
                                         30), "", "box");


            if (GUILayout.Button("Construct Harvester"))
            {
                StartCoroutine(buildHarvester());
            }

            GUILayout.EndArea();

        }


    }
}
