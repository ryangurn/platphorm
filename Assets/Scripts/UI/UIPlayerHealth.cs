using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
	GameObject[] players;
	Text health;
	

	void Start()
	{
		health = GetComponent<Text>();
	}

	void FixedUpdate()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
        int length = 0;
        float total;
        int h = 0;


        foreach (GameObject g in players)
		{
			if(g.GetComponent<Health>())
			{
                length++;
                h += Mathf.FloorToInt(g.GetComponent<Health>().HealthLevel);
			}
		}

		total = length * 4;

		health.text = "Player Health: "+ h.ToString() + " / " + total.ToString();
	}
}
