using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex;

    // Start is called before the first frame update
    void Start()
    {

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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
          popupIndex++;
        }

      }
      else if (popupIndex == 1)
      {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
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
        if ( Input.GetMouseButton(1) )
        {
          popupIndex++;
        }
      }

    }
}
