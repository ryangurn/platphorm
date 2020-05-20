using UnityEngine;
using System.Collections;

//Attaches to camera. The selection box on screen: is represented with a GUI rectangle
public class Selection : MonoBehaviour
{
	public bool isLocked = false;

	private Vector2 start = new Vector2(0, 0); //this inits the member variable to store the x, y of the initial click of the selection
	private Vector2 cur = new Vector2(0, 0);  //inits member variable to store x, y of the current position of the mouse after initial click
	private bool visible = false;
	private GameObject playerFactory;
	private GameObject playerTechCenter;
	private GameObject playerPowerCenter;

	private Rect CurrentRect() //computes the selection rectangle
	{
		Vector2 min = new Vector2(Mathf.Min(start.x, cur.x),  //necessary to enable player can draw the selection in any direction
		Mathf.Min(start.y, cur.y));
		Vector2 max = new Vector2(Mathf.Max(start.x, cur.x),
		Mathf.Max(start.y, cur.y));
		return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
	}

	void Start()
	{
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("PlayerBuilding");
		foreach (GameObject g in buildings)
		{
			if (g.name == "Factory")
			{
				playerFactory = g;
			}
			else if (g.name == "PowerPlant")
			{
				playerPowerCenter = g;
			}
			else if (g.name == "TechnologyCenter")
			{
				playerTechCenter = g;
			}

		}
	}

	//rectangle that coincides with construction options when they're on the screen
	private Rect constructionButtonLocation = new Rect(Screen.width / 2 - 300, Screen.height - 100, 600, 60);

	void Update()
	{
		if (isLocked) { return; }

		//quit if we clicked in the construction button area on the screen
		if (constructionButtonLocation.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) && playerFactory.GetComponent<FactoryBuildPlayer>().isSelected)
		{
			return;
		}

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
			visible = false;
			bool unitSelected = false; //at least one unit is selected
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
					SetSelected(g, true); //add mesh rendering to the SelectedSymbol object of this game object
					unitSelected = true;
				}
				else
				{
					SetSelected(g, false); //remove it for units no longer in the selection
				}
			}



			//check for buildings

			start.x = Input.mousePosition.x - 50f;  //give the player more room to single-click on the buildings since the check for containment is only one pixel
			start.y = Screen.height - Input.mousePosition.y - 50f;

			cur.x = Input.mousePosition.x + 50f;
			cur.y = Screen.height - Input.mousePosition.y + 50f;

			GameObject[] buildings = GameObject.FindGameObjectsWithTag("PlayerBuilding");
			foreach (GameObject g in buildings)
			{
				// Convert each unit's position to position on the screen/camera
				Vector3 p = Camera.main.WorldToScreenPoint(g.transform.position);
				Vector2 screenPos = new Vector2(p.x, Screen.height - p.y);

				// is this inside our selection rectangle?
				if (CurrentRect().Contains(screenPos) && !unitSelected)
				{
					SetSelected(g, true); //add mesh rendering to the SelectedSymbol object of this game object
				}
				else
				{
					SetSelected(g, false); //remove it for units no longer in the selection
				}

			}

		}

	}

	void SetSelected(GameObject g, bool selected)
	{
		MeshRenderer[] gChildren = g.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer mr in gChildren)
		{
			if (mr.gameObject.name == "SelectedSymbol")
			{
				// Make the selected symbol mesh visible or not
				mr.enabled = selected;
			}

		}

		//for player factory at the moment
		if (g == playerFactory)
		g.GetComponent<FactoryBuildPlayer>().isSelected = selected;

	}

	void OnGUI()
	{
		if (visible && !cur.Equals(start)) //selection rectangle should be visible while the selection is happening & mouse has been moved from intial click
		{
			GUI.Box(CurrentRect(), "");
		}
	}

	public void Lock()
	{
		isLocked = true;
	}

	public void Unlock()
	{
		isLocked = false;
	}
}
