using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthLevel = 4; //everything has a default health level of 4
    public TextMesh tm;

    void Update()
    {
        // TextMesh tm = GetComponentInChildren<TextMesh>();

        if (Mathf.FloorToInt(HealthLevel) < 0)
        {
            tm.text = "";
        }
        else
        {
            tm.text = new string('-', Mathf.FloorToInt(HealthLevel)); //print a bar for each unit of health remaining
        }
        Color color;

        switch (Mathf.FloorToInt(HealthLevel)) //change the "text" color depending on damage level
        {
            default:
                color = Color.green;
                break;
            case (3):
                color = Color.magenta;
                break;
            case (2):
                color = Color.yellow;
                break;
            case (1):
                color = Color.red;
                break;
            case (0):
                color = Color.white;
                break;
        }

        tm.GetComponent<Renderer>().material.color = color;

        tm.transform.forward = Camera.main.transform.forward;
    }

    void LateUpdate() //it's dead, get rid of the object
    {
        if (HealthLevel < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void RunDamageAnimation() //my teammates have opted to not show this damage "animation" script I have created for the alpha build. I will work with them on a solution we can agree on for future builds
    {

        // StartCoroutine(StartPS());

    }

    // private IEnumerator StartPS()
    // {
    //     // MeshRenderer[] gChildren = GetComponentsInChildren<MeshRenderer>();
    //     // foreach (MeshRenderer mr in gChildren)
    //     // {
    //     //     if (mr.gameObject.name == "Damage" && mr.enabled == false)
    //     //     {
    //     //         mr.enabled = true;
    //     //         yield return new WaitForSeconds(1);
    //     //         mr.enabled = false;
    //     //     }
    //     //
    //     // }
    // }

}
