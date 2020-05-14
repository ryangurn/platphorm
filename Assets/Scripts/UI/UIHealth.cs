using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attaches to canvas child. Used for both teams
public class UIHealth : MonoBehaviour
{
    public string myTeam;
	private GameObject[] units;
	private Text health;
    public bool HasUnits = true; //used in the case of the enemy team for evaluating the win condition

    void Start()
	{
		health = GetComponent<Text>();
	}

	void FixedUpdate()
	{
        HasUnits = true;
		units = GameObject.FindGameObjectsWithTag(myTeam);
        int length = 0;
        float total = 0;
        int h = 0;

        foreach (GameObject g in units)
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

		health.text = myTeam + " Health: "+ h.ToString() + " / " + total.ToString();
	}
}
