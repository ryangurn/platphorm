using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

	public float CameraMinX = -65.0f;
	public float CameraMinHeight = 5.0f;
	public float CameraMinZ = -20.0f;
	public float CameraMaxX = 20.0f;
	public float CameraMaxHeight = 40.0f;
	public float CameraMaxZ = 60.0f;
	public float MouseSpeed = 3.5f;
	public float KeyboardSpeed = 50.0f; //regular speed
	public float ScrollSpeed = 1.0f;
	public float pauseTime = 0.0f;

	public bool CameraLeftRight = false;
	public bool CameraFrontBack = false;
	public bool CameraZoom = false;
	public bool EdgeControl = true;
	public bool isLocked = false;
	public int InvertPan = 1;

	private float m_shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	private float m_maxShift = 1000.0f; //Maximum speed when holding shift
	private float m_totalRun = 1.0f;
	private float m_mouseX;
	private float m_mouseY;
	public bool isPanning = false;

	public GameObject PauseMenu;

	private Selection SelectionLocked;
	private float CameraHeight = 12.0f; //cam height relative to ground
	private Quaternion originalPan; //this keeps track of where we were before we started panning. If the pan wasn't that much, the user probably meant to move a unit

	void Start()
	{
		SelectionLocked = GetComponent<Selection>();
	}

	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.P)) //escape menu
		{
			if (PauseMenu.activeSelf) {
				unPause();
				PauseMenu.SetActive(false);
			} else {
				Pause();
				PauseMenu.SetActive(true);
			}
		}

		if (isLocked) //prevent things from being selected when paused
		return;

		if (Input.GetKeyUp(KeyCode.X)) //exit the level to  menu
		{
			SceneManager.LoadScene("Menu");
		}

		float priorGroundHeight = transform.position.y - CameraHeight; //before we change our look or camera height, we need to know what the ground height used to be for later

		//right click panning
		if (Input.GetMouseButtonDown(1))  //start panning and lock scrolling
		{
			originalPan = transform.rotation; //referenced to later to see if we were panning or just ordering a unit by right-click
			isPanning = true;
		}

		if ( Input.GetMouseButton(1)) //user is panning....
		{
			transform.Rotate(new Vector3(-InvertPan*Input.GetAxis("Mouse Y") * MouseSpeed, InvertPan*Input.GetAxis("Mouse X") * MouseSpeed, 0));
			m_mouseX = transform.rotation.eulerAngles.x;
			m_mouseY = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(m_mouseX, m_mouseY, 0);
		}

		if ( (Input.GetMouseButtonUp(1) && Quaternion.Angle(transform.rotation, originalPan) < 5f )) //we didn't really pan, so we order a unit instead
		{
			isPanning = false;
			GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject g in playerUnits)
			{
				if (g.GetComponent<PlayerMove>())
				g.GetComponent<PlayerMove>().Move();
			}
		}

		else if (Input.GetMouseButtonUp(1)) //done panning, countdown to scrolling enabled again
		{
			StartCoroutine(EdgeScrollLockout());
		}

		//camera gross movement
		Vector3 p = GetBaseInput(); //base input (wasd and edge scroll)

		if (Input.GetKey (KeyCode.LeftShift))  //use left shift to slow movement
		{
			m_totalRun += Time.deltaTime;
			p  = p * m_totalRun * m_shiftAdd;
			p.x = Mathf.Clamp(p.x, -m_maxShift, m_maxShift);
			p.y = Mathf.Clamp(p.y, -m_maxShift, m_maxShift);
			p.z = Mathf.Clamp(p.z, -m_maxShift, m_maxShift);
		}
		else
		{
			m_totalRun = Mathf.Clamp(m_totalRun * 0.5f, 1f, 1000f);
			p = p * KeyboardSpeed;
		}
		p = p * Time.deltaTime;

		Vector3 clampedPosition;
		clampedPosition.y = transform.position.y; //y isn't clamped here since we only look at relative height for it

		//translation from GetBaseInput
		transform.Translate(p); //add in the wasd and edge scrol input

		//clamp
		clampedPosition.x = Mathf.Clamp(transform.position.x, CameraMinX, CameraMaxX); //bounds checking and adjustment is oob
		clampedPosition.z = Mathf.Clamp(transform.position.z, CameraMinZ, CameraMaxZ);
		transform.position = clampedPosition;

		//rotation clamp
		Vector3 rotation = transform.rotation.eulerAngles;

		if (200f < rotation.x && rotation.x < 360f)
		rotation.x = Mathf.Clamp(rotation.x, 360f, Mathf.Infinity);
		else
		rotation.x = Mathf.Clamp(rotation.x, 5f, 40f);

		transform.rotation = Quaternion.Euler(rotation);

		//mouse scroll wheel (for height)
		Vector3 priorPosition = transform.position; //take note of the position before we do scrolling adjustment. a simple clamp won't work now since the camera height is relative

		transform.Translate(new Vector3(0, 0, HeightAdjust())); //HeightAdjust() function takes mousewheel input

		float priorCameraHeight = CameraHeight;
		CameraHeight += (transform.position.y - priorPosition.y); //add difference in camera y to the Camera height

		//if any parameters are exceeded, return to location and camera height before using the scroll wheel
		if (transform.position.x < CameraMinX || transform.position.x > CameraMaxX
		|| transform.position.z < CameraMinZ || transform.position.z > CameraMaxZ
		|| (CameraHeight < CameraMinHeight || CameraHeight > CameraMaxHeight))
		{
			transform.position = priorPosition;
			CameraHeight = priorCameraHeight;
		}


		//adjust for terrain
		Vector3 finalPosition;
		finalPosition.x = transform.position.x;
		finalPosition.z = transform.position.z;

		transform.Translate(new Vector3(0, 0, GroundHeightAdjust(priorGroundHeight) / 2));
		finalPosition.y = transform.position.y;

		transform.position = finalPosition;

	}

	private Vector3 GetBaseInput()
	{
		//returns wasd and edge scroll values
		Vector3 p_Velocity = new Vector3();

		//if you're panning (or had just been panning .5 or so seconds ago), it doesn't allow movement. This is in case your mouse leaves the screen, you want edge scrolling locked out.


		if ((Input.GetKey(KeyCode.W) || (Input.mousePosition.y > Screen.height - 7 && !isPanning && EdgeControl))) //7 pixels seems to be a good screen edge for scrolling.
		{
			CameraFrontBack = true;
			CameraLeftRight = false;
			p_Velocity += new Vector3(0, 0, 1) * ScrollSpeed;
		}

		if ((Input.GetKey(KeyCode.S) || (Input.mousePosition.y < 7 && !isPanning && EdgeControl)))
		{
			CameraFrontBack = true;
			CameraLeftRight = false;
			p_Velocity += new Vector3(0, 0, -1) * ScrollSpeed;
		}

		if ((Input.GetKey(KeyCode.A) || (Input.mousePosition.x < 7 && !isPanning && EdgeControl)))
		{
			CameraFrontBack = false;
			CameraLeftRight = true;
			p_Velocity += new Vector3(-1, 0, 0) * ScrollSpeed;
		}

		if ((Input.GetKey(KeyCode.D) || (Input.mousePosition.x > Screen.width - 7 && !isPanning && EdgeControl)))
		{
			CameraFrontBack = false;
			CameraLeftRight = true;
			p_Velocity += new Vector3(1, 0, 0) * ScrollSpeed;
		}

		return p_Velocity;
	}

	private float HeightAdjust() //mouse scroll wheel input
	{
		if ( (Input.GetAxis("Mouse ScrollWheel") > 0) )
		{
			return 3f;
		}
		else if ( (Input.GetAxis("Mouse ScrollWheel") < 0) )
		{
			return -3f;
		}
		else
		{
			return 0f;
		}
	}

	private IEnumerator EdgeScrollLockout()
	{
		yield return new WaitForSeconds(.5f); //this is the delay to allow your mouse cursor to return to the screen after panning without immediate scrolling.
		isPanning = false;
	}

	private float GroundHeightAdjust(float priorGroundHeight) //this does the raycasting for figuring out the ground height
	{
		RaycastHit hit;
		int[] offsetContainer = new int[2];

		//starting offset at 1, to prevent redundant calculations
		for (int offset = 1; offset < 2000; offset++ ) //we limit the possible places a "Ground" object can be to 2000 pixels in each direction
		{
			for (int i = 0; i < 8; i++) //this stages the offsetContainer for which of the 8 directions we check for ground in
			{
				switch (i)
				{
					default: //left
					offsetContainer[0] = -offset;
					offsetContainer[1] = 0;
					break;
					case 1: //right
					offsetContainer[0] = offset;
					offsetContainer[1] = 0;
					break;
					case 2: //up
					offsetContainer[0] = 0;
					offsetContainer[1] = -offset;
					break;
					case 3: //down
					offsetContainer[0] = 0;
					offsetContainer[1] = offset;
					break;
					case 4: //upper-left
					offsetContainer[0] = -offset;
					offsetContainer[1] = -offset;
					break;
					case 5: //upper-right
					offsetContainer[0] = offset;
					offsetContainer[1] = -offset;
					break;
					case 6: //lower-right
					offsetContainer[0] = offset;
					offsetContainer[1] = offset;
					break;
					case 7: //lower-left
					offsetContainer[0] = -offset;
					offsetContainer[1] = offset;
					break;

				}
				//the height component is ajusted based on how steep the camera angel is in euler angels
				Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2 + offsetContainer[0], Screen.height * transform.rotation.eulerAngles[0] / 90 + offsetContainer[1]));
				Physics.Raycast(ray, out hit);

				//if we're looking at the ground, that's it. Otherwise, keep looking
				if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
				{
					return priorGroundHeight - hit.point.y;
				}

			}
		}
		return priorGroundHeight; //this shouldn't happen as there should always be a ground object the raycast can find
	}

	public void UpdateScrollSpeed(GameObject slide)
	{
		ScrollSpeed = slide.GetComponent<Slider>().value;
	}

	public void UpdateVolume(GameObject slide)
	{
		AudioListener.volume = slide.GetComponent<Slider>().value;
	}

	public void ToggleEdgeControls(GameObject slide)
	{
		EdgeControl = slide.GetComponent<Toggle>().isOn;
	}

	public void ToggleInvertPan(GameObject toggle)
	{
		bool t = toggle.GetComponent<Toggle>().isOn;
		if(t == true)
		{
			InvertPan = -1;
		}
		else
		{
			InvertPan = 1;
		}
	}

	private void Pause()
	{
		isLocked = true;
		SelectionLocked.Lock();
		Time.timeScale = pauseTime;
	}

	public void unPause()
	{
		isLocked = false;
		SelectionLocked.Unlock();
		Time.timeScale = 1;
	}

}
