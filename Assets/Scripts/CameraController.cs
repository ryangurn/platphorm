using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float CameraMinX = -65.0f;
	public float CameraMinZ = -20.0f;
	public float CameraMaxX = 20.0f;
	public float CameraMaxZ = 20.0f;
	public float MouseSpeed = 3.5f;
	public float KeyboardSpeed = 50.0f; //regular speed
	public float ZoomSpeed = 2.0f;
	private float m_shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	private float m_maxShift = 1000.0f; //Maximum speed when holdin gshift
	private float m_totalRun = 1.0f;
	private float m_mouseX;
	private float m_mouseY;

	void Update ()
	{
		if ( Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) )
		{
			transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * MouseSpeed, -Input.GetAxis("Mouse X") * MouseSpeed, 0));
			m_mouseX = transform.rotation.eulerAngles.x;
			m_mouseY = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(m_mouseX, m_mouseY, 0);
		}

		//Keyboard commands
		Vector3 p = GetBaseInput();
		if ( Input.GetKey (KeyCode.LeftShift) )
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
		Vector3 newPosition = transform.position;
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			transform.Translate(ZoomSpeed * new Vector3(0, 0 , 1));
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			transform.Translate(ZoomSpeed * new Vector3(0, 0, -1));
		}
		else
		{
			//If player wants to move on X and Z axis only which they always do
			transform.Translate(p);
			// 	Mathf.Clamp(transform.position.x, CameraMinX, CameraMaxX),
			// Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
			// Mathf.Clamp(transform.position.z, CameraMinZ, CameraMaxZ));
			// adding bounding?
			newPosition.x = Mathf.Clamp(transform.position.x, CameraMinX, CameraMaxX);
			newPosition.z = Mathf.Clamp(transform.position.z, CameraMinZ, CameraMaxZ);
			transform.position = newPosition;
		}

	}

	private Vector3 GetBaseInput()
	{
	//returns the basic values, if it's 0 than it's not active.
		Vector3 p_Velocity = new Vector3();

		if (Input.GetKey (KeyCode.W))
		{
			p_Velocity += new Vector3(0, 0 , 1);
		}

		if (Input.GetKey (KeyCode.S))
		{
			p_Velocity += new Vector3(0, 0, -1);
		}

		if (Input.GetKey (KeyCode.A))
		{
			p_Velocity += new Vector3(-1, 0, 0);
		}

		if (Input.GetKey (KeyCode.D))
		{
			p_Velocity += new Vector3(1, 0, 0);
		}

		return p_Velocity;
	}
}
