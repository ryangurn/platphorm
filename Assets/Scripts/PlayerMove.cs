using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{


    void Update()
    {
       


        


    }

    public void Move()
    {
        if (Input.GetMouseButtonUp(1) && IsSelected()) //if the object is selected and player right clicked a location
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //find the ray to that point on screen
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.point.Equals(Vector3.zero)) //if the move order is made to a place a ray can't be made for, just stay put
                return;
            else
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point; //otherwise, go to the point

        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (Vector3.Distance(GetComponent<UnityEngine.AI.NavMeshAgent>().destination, transform.position) < 6f)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = transform.position;
        }
        

    }

    // Find out if it's selected i.e. if SelectionSymbol is visible
    bool IsSelected()
    {
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in children) //check each mesh renderer to see if it's the selected symbol
        {
            if (mr.gameObject.name == "SelectedSymbol" && mr.enabled) //if it is, and it's being displayed to the player, the object is selected
            {
                return true;
            }
        }
        return false;
    }
}