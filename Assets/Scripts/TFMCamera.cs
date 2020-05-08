using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFMCamera : MonoBehaviour
{
    public float CamHeight = 12;
    

    void Update()
    {
        //
        Vector3 safeLocation = transform.position; //aka where it used to be before we transform anything
        Vector3 NewPosition = GetBaseInput() + transform.position;
        
        transform.position = NewPosition;

        //now deal with y and revert to old position if necessary
        float groundLevel = GetGroundLevel();
        if (groundLevel == 0)
        {
            transform.position = safeLocation;
            return;
        }
        
        float x = transform.position.x;
        float y = Mathf.Lerp(transform.position.y, groundLevel + CamHeight, 4);
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
        


    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height - 7)
        {
            p_Velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < 7)
        {
            p_Velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < 7)
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - 7)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }

        return p_Velocity;
    }

    float GetGroundLevel()
    {
        RaycastHit hit;  

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); //find the ray to that point on screen 
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
            ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2 + offset++, Screen.height / 2)); //find the ray to that point on screen 
            Physics.Raycast(ray, out hit);
        }

        return hit.point.y;
    }
}
