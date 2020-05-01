using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour
{

	Vector2 start = new Vector2(0, 0); //this inits the member variable to store the x, y of the initial click of the selection
	Vector2 cur = new Vector2(0, 0);  //inits member variable to store x, y of the current position of the mouse after initial click 
	bool visible = false;

	Rect CurrentRect() //computes the selection rectangle
	{
		Vector2 min = new Vector2(Mathf.Min(start.x, cur.x),  //necessary to enable player can draw the selection in any direction
								  Mathf.Min(start.y, cur.y));
		Vector2 max = new Vector2(Mathf.Max(start.x, cur.x),
								  Mathf.Max(start.y, cur.y));
		return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
	}

	void Update()
	{  
		//start selection when left button clicked
		if (Input.GetMouseButtonDown(0) && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			start.x = Input.mousePosition.x - 7f;
			start.y = Screen.height - Input.mousePosition.y - 7f;

			visible = true;
		}

		// continue selection while left button is held down
		if (Input.GetMouseButton(0))
		{
			// update current mouse position coords
			cur.x = Input.mousePosition.x + 7f;
			cur.y = Screen.height - Input.mousePosition.y + 7f;
		}

		// when not clicking anymore
		if (Input.GetMouseButtonUp(0))
		{
			// make list of all objects belonging to player
			GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject g in units)
			{
				// Convert each unit's position to position on the screen/camera
				Vector3 p = Camera.main.WorldToScreenPoint(g.transform.position);
				Vector2 screenPos = new Vector2(p.x, Screen.height - p.y);

				// is this inside our selection rectangle?
				if (CurrentRect().Contains(screenPos))
				{
					SetSelectedSymbolVis(g, true); //add mesh rendering to the SelectedSymbol object of this game object
				}
				else
				{
					SetSelectedSymbolVis(g, false); //remove it for units no longer in the selection
				}       
			}

			visible = false; //stop showing the selection box
		}
	}

	void SetSelectedSymbolVis(GameObject g, bool visible)
	{
		MeshRenderer[] gChildren = g.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer mr in gChildren)
		{
			if (mr.gameObject.name == "SelectedSymbol")
			{
				// Make the selected symbol mesh visible or not
				mr.enabled = visible;
			}
		}
	}

	void OnGUI()
	{
		if (visible && !cur.Equals(start)) //selection rectangle should be visible while the selection is happening & mouse has been moved from intial click
		{
			GUI.Box(CurrentRect(), "");
		}
	}
}