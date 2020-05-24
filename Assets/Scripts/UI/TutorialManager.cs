using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject Camera;

    public GameObject[] popups;
    private int popupIndex;

    private GameObject[] units;
    private GameObject[] harvesters;
    private GameObject[] attackingUnits;

    // Start is called before the first frame update
    void Start()
    {
      units = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
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
          popupIndex++;
        }

      }
      else if (popupIndex == 1)
      {
        if (Camera.GetComponent<CameraController>().CameraLeftRight)
        {
          popupIndex++;
        }
      }
      else if (popupIndex == 2)
      {
        if ( Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
          popupIndex++;
        }
      }
      else if (popupIndex == 3)
      {
        if (Camera.GetComponent<CameraController>().isPanning)
        {
          popupIndex++;
        }
      }
      else if (popupIndex == 4)
      {
        if (Camera.GetComponent<CameraController>().isLocked)
        {
          popupIndex++;
        }
      }
      else if (popupIndex == 5)
      {
        if (!Camera.GetComponent<CameraController>().isLocked)
        {
          popupIndex++;
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

          if(move)
          {
            popupIndex++;
          }
        }
      }
      else if (popupIndex == 7)
      {
        // do the same thing for the harvester

        popupIndex++;
      }


      units = GameObject.FindGameObjectsWithTag("Player");
    }
}
