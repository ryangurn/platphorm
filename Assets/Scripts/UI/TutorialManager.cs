using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
  public GameObject Camera;
  public GameObject TimeObject;
  public GameObject NextSound;
  public GameObject[] popups;

  private int popupIndex;
  private int changeCountHarvester;
  private int changeCountAttack;

  private GameObject[] units;
  private List<GameObject> harvesters = new List<GameObject>();
  private List<GameObject> attackingUnits = new List<GameObject>();
  private bool PanLock = false;
  private bool ScrollLock = false;
  float time = 2.5f;

  void UpdateUnits()
  {
    units = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject unit in units)
    {
      if (unit.name == "Harvester") {
        harvesters.Add(unit);
      }
      else
      {
        attackingUnits.Add(unit);
      }
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    changeCountHarvester = 0;
    changeCountAttack = 0;
    UpdateUnits();
  }

  // Update is called once per frame
  void LateUpdate()
  {

    for (int i = 0; i < popups.Length; i++)
    {
      if (i == popupIndex)
      {
        popups[i].SetActive(true);
      }
      else
      {
        popups[i].SetActive(false);
      }
    }


    if (popupIndex == 0)
    {
      if (Camera.GetComponent<CameraController>().CameraFrontBack)
      {
        StartCoroutine(wait());
      }

    }
    else if (popupIndex == 1)
    {
      if (Camera.GetComponent<CameraController>().CameraLeftRight)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 2)
    {
      if ( Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
      {
        ScrollLock = true;
      }

      if (ScrollLock)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 3)
    {
      if (Camera.GetComponent<CameraController>().isPanning)
      {
        PanLock = true;
      }

      if (PanLock)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 4)
    {
      if (Camera.GetComponent<CameraController>().isLocked)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 5)
    {
      if (!Camera.GetComponent<CameraController>().isLocked)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 6)
    {
      bool move = false;
      foreach(GameObject unit in units)
      {
        // get the mesh renders
        MeshRenderer[] unitChildren = unit.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in unitChildren)
        {
          if (mr.gameObject.name == "SelectedSymbol")
          {
            // Make the selected symbol mesh visible or not
            if(mr.enabled)
            {
              move = true;
            }
          }
        }
      }

      if(move)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 7)
    {
      bool unselected = true;
      foreach(GameObject unit in units)
      {
        // get the mesh renders
        MeshRenderer[] unitChildren = unit.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in unitChildren)
        {
          if (mr.gameObject.name == "SelectedSymbol")
          {

            if(mr.enabled)
            {
              unselected = false;
            }
          }
        }
      }

      if(unselected)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 8)
    {
      // do the same thing for the harvester
      foreach (GameObject harvest in harvesters) {
        // check if there is motion
        if (harvest.transform.hasChanged)
        {
          changeCountHarvester++;
          harvest.transform.hasChanged = false;
        }
      }

      if (changeCountHarvester >= 100)
      {
        StartCoroutine(wait());
      }
    }
    else if (popupIndex == 9)
    {
      // do the same thing for the harvester
      foreach (GameObject attack in attackingUnits) {
        // check if there is motion
        if (attack.transform.hasChanged)
        {
          changeCountAttack++;
          attack.transform.hasChanged = false;
        }
      }

      if (changeCountAttack >= 100)
      {
        StartCoroutine(wait());
      }
    }

    UpdateUnits();
  }

  IEnumerator wait()
  {
    time -= Time.deltaTime;
    TimeObject.GetComponent<Text>().text = time.ToString("F1");
    if (time < 0)
    {
      // Play the ping
      AudioSource audio = NextSound.GetComponent<AudioSource>();
      audio.Play();
      popupIndex++;
      time = 2.5f;
    }
    yield return new WaitForSeconds(0.0f);
  }

  IEnumerator shortWait()
  {
      yield return new WaitForSeconds(1.0f);
  }
}
