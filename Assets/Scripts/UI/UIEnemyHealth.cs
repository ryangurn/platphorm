using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIEnemyHealth : MonoBehaviour
{
	GameObject[] enemies;
	Text health;
    public bool HasUnits = true;

    void Start()
	{
		health = GetComponent<Text>();
	}

	void FixedUpdate()
	{
        HasUnits = true;
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int length = 0;
        float total = 0;
        int h = 0;


        foreach (GameObject g in enemies)
        {
			if(g.GetComponent<Health>())
			{
                length++;
				h += Mathf.FloorToInt(g.GetComponent<Health>().HealthLevel);
			}
		}

        if (length == 0)
            HasUnits = false;

		total = length * 4;

		health.text = "Enemy Health: "+ h.ToString() + " / " + total.ToString();
	}
}
