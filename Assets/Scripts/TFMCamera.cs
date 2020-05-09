using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFMCamera : MonoBehaviour
{
    public bool InvertedPan = false;
    public float ScrollSpeed = 1f;
    public bool isPanning = false;
    private Quaternion originalPan;

    void Update()
    {
 
        Vector3 safeLocation = transform.position; //aka where it used to be before we transform anything
        float priorGroundLevel = GetGroundLevel();
   
        transform.Translate(GetBaseInput());

        transform.position += new Vector3(0, safeLocation.y - transform.position.y, 0);

        Quaternion safeRotation = transform.rotation;
        MouseRotate();

        transform.Translate(new Vector3(0, 0, camHeightAdjust()));

        //now deal with y and revert to old position if necessary
        float groundLevel = GetGroundLevel();
        if (groundLevel == 0)
        {
            transform.position = safeLocation;
            transform.rotation = safeRotation;
            
        }
        else
        {         
            transform.position += new Vector3(0, groundLevel - priorGroundLevel, 0);
        }

        


        

    }

    private float camHeightAdjust()
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

    private void MouseRotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            originalPan = transform.rotation;
            isPanning = true;
        }

        if (Input.GetMouseButton(1))
        {
            
            transform.Rotate(new Vector3(2 * Input.GetAxis("Mouse Y"), -2 * Input.GetAxis("Mouse X"), 0));  //make option to invert this 
            float m_mouseX = transform.rotation.eulerAngles.x;
            float m_mouseY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(m_mouseX, m_mouseY, 0);
        }

        if (Input.GetMouseButtonUp(1) && Quaternion.Angle(transform.rotation, originalPan) < 5f)
        {
            isPanning = false;
            GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject g in playerUnits)
            {
                if (g.GetComponent<PlayerMove>())
                    g.GetComponent<PlayerMove>().Move();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(EdgeScrollLockout());
        }
    }


    private Vector3 GetBaseInput()
    {
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


    float GetGroundLevel()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height * transform.rotation.eulerAngles[0] / 90)); //find the ray to that point on screen 
        Physics.Raycast(ray, out hit);

        //if we're looking in to the abyss, return zero. This is an error condition
        if (hit.collider == null)
        {
            return 0f;
        }

        //this block steps the raycast's vector to the right one pixel until it's looking at a "Ground"-tagged object. This prevents the height from rising when a unit is in the middle of the screen
        int offset = 0;
        while (hit.collider != null && hit.collider.gameObject.tag != "Ground")
        {
            ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2 + offset++, Screen.height * transform.rotation.eulerAngles[0] / 90)); //find the ray to that point on screen 
            Physics.Raycast(ray, out hit);
        }

        return hit.point.y;
    }

    private IEnumerator EdgeScrollLockout()
    {
        yield return new WaitForSeconds(1f);
        isPanning = false;
    }




}
