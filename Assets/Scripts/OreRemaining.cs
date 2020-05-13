using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreRemaining : MonoBehaviour
{
    public float OreContent;

    void Start()
    {
      OreContent = Random.Range(100f, 1000f);
    }

    void FixedUpdate()
    {
        TextMesh tm = GetComponentInChildren<TextMesh>();

        if (Mathf.FloorToInt(OreContent) <= 0)
        {
            Destroy(gameObject);

        }/*
        else
        {
            tm.text = new string('-', Mathf.FloorToInt(OreContent) / 5);
        }
        Color color;

        switch (Mathf.FloorToInt(OreContent))
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

        tm.transform.forward = Camera.main.transform.forward;*/
    }

}
