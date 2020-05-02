using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
	GameObject[] players;
	Text health;
	float total;

	void Start()
	{
		health = GetComponent<Text>();
	}

	void Update()
	{
		players = GameObject.FindGameObjectsWithTag("Enemy");
		int length = players.Length;
		int i = 0;

		float h = 0;
		for (; i < length; i++)
		{
			if(players[i])
			{
				h += players[i].GetComponent<Health>().HealthLevel;
			}
		}

		total = length * 4;

		health.text = "Player Health: "+ h.ToString() + " / " + total.ToString();
	}
}
