using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHealth : MonoBehaviour
{
	GameObject[] enemies;
	Text health;
	float total;

	void Start()
	{
		health = GetComponent<Text>();
	}

	void Update()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		int length = enemies.Length;
		int i = 0;

		float h = 0;
		for (; i < length; i++)
		{
			if(enemies[i])
			{
				h += enemies[i].GetComponent<Health>().HealthLevel;
			}
		}

		total = length * 4;

		health.text = "Enemy Health: "+ h.ToString() + " / " + total.ToString();
	}
}
