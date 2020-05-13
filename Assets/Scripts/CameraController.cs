using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float CameraMinX = -65.0f;
    public float CameraMinHeight = 5.0f;
    public float CameraMinZ = -20.0f;
	public float CameraMaxX = 20.0f;
    public float CameraMaxHeight = 40.0f;
    public float CameraMaxZ = 10.0f;
    public float MouseSpeed = 3.5f;
	public float KeyboardSpeed = 50.0f; //regular speed
	public float ScrollSpeed = 1.0f;
	private float m_shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	private float m_maxShift = 1000.0f; //Maximum speed when holdin gshift
	private float m_totalRun = 1.0f;
	private float m_mouseX;
	private float m_mouseY;
    public bool isPanning = false;
    private float CameraHeight = 12.0f;
    private Quaternion originalPan;

	void Update ()
	{
        //right click panning
        if (Input.GetMouseButtonDown(1))  //start panning and initiate scroll lock
        {
            originalPan = transform.rotation; //referenced later to see if we were panning or ordering a unit
            isPanning = true;
        }

        if ( Input.GetMouseButton(1)) //panning work
		{
			transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * MouseSpeed, -Input.GetAxis("Mouse X") * MouseSpeed, 0));
			m_mouseX = transform.rotation.eulerAngles.x;
			m_mouseY = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(m_mouseX, m_mouseY, 0);
		}

        if (Input.GetMouseButtonUp(1) && Quaternion.Angle(transform.rotation, originalPan) < 5f) //we didn't really pan, so we order a unit instead
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

        //other camera movement
        Vector3 p = GetBaseInput();
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
		Vector3 newPosition = transform.position;

        //translation from GetBaseInput
        transform.Translate(p);


        //clamp
		newPosition.x = Mathf.Clamp(transform.position.x, CameraMinX, CameraMaxX);
        newPosition.z = Mathf.Clamp(transform.position.z, CameraMinZ, CameraMaxZ);
        //newPosition.y = CameraHeight + GetGroundHeight();
        transform.position = newPosition;

        Vector3 priorPosition = transform.position;

        transform.Translate(new Vector3(0, 0, HeightAdjust()));

        //if any parameters are exceeded, return to prior otherwise keep new y difference and add it to camera height
        if (transform.position.x < CameraMinX || transform.position.x > CameraMaxX)
            transform.position = priorPosition;
        else if (transform.position.z < CameraMinZ || transform.position.z > CameraMaxZ)
            transform.position = priorPosition;
        else if ((CameraHeight + transform.position.y - priorPosition.y) < CameraMinHeight)
            transform.position = priorPosition;
        else if ((CameraHeight + transform.position.y - priorPosition.y) > CameraMaxHeight)
            transform.position = priorPosition;
        else
            CameraHeight = (CameraHeight + transform.position.y - priorPosition.y);

        newPosition.x = transform.position.x;
        newPosition.y = CameraHeight + GetGroundHeight();
        newPosition.z = transform.position.z;

        transform.position = newPosition;

    }

    private Vector3 GetBaseInput()
	{
	//returns the basic values, if it's 0 than it's not active.
		Vector3 p_Velocity = new Vector3();

        if (isPanning)
        {
            return p_Velocity;
        }


        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height - 7)
        {
            p_Velocity += new Vector3(0, 0, 1) * ScrollSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < 7)
        {
            p_Velocity += new Vector3(0, 0, -1) * ScrollSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < 7)
        {
            p_Velocity += new Vector3(-1, 0, 0) * ScrollSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - 7)
        {
            p_Velocity += new Vector3(1, 0, 0) * ScrollSpeed;
        }

        return p_Velocity;
    }

    private float HeightAdjust()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            return 3f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
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
        yield return new WaitForSeconds(.1f);
        isPanning = false;
    }

    private float GetGroundHeight()
    {

        RaycastHit hit;
        int[] offsetContainer = new int[2];

        //starting offset at 1, to prevent redundant calculations
        for (int offset = 1; offset < 2000; offset++ )
        {

            for (int i = 0; i < 8; i++)
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

                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2 + offsetContainer[0], Screen.height * transform.rotation.eulerAngles[0] / 90 + offsetContainer[1])); //find the ray to that point on screen
                Physics.Raycast(ray, out hit);

                //if we're looking at the ground, that's it. Otherwise, keep looking
                if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
                {
                    return hit.point.y;
                }

            }
        }
        return 0.0f;

    }





}
