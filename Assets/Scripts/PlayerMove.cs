using UnityEngine;
using System.Collections;

//attaches to player units
public class PlayerMove : MonoBehaviour
{
    private MeshRenderer selectedSymbol;

    void Awake()
    {
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in children) //check each mesh renderer to see if it's the selected symbol
        {
            if (mr.gameObject.name == "SelectedSymbol") //if it is, that's what we'll use for this unit instance 
            {
                selectedSymbol = mr;
            }
        }
    }

    public void Move() //this is called from the Camera.cs script
    {
        if (selectedSymbol.enabled) //if the object is selected, move to requested click location
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //find the ray to that point on screen
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.point.Equals(Vector3.zero)) //if the move order is made to a place a ray can't be made for, just stay put
                return;
            else
                DestinationSet(hit.point);
            
        }
    }

    void OnTriggerEnter(Collider other) //prevents units from bumping into eachother when mulliple are ordered to the same location at the same time. Stop when close enough and close to another unit
    {
        if (Vector3.Distance(GetComponent<UnityEngine.AI.NavMeshAgent>().destination, transform.position) < 6f)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = transform.position;
        }      
    }

    void DestinationSet(Vector3 location)
    {
        if (gameObject.GetComponent<PlayerHarvester>()) //if we are a harvester
        {
            gameObject.GetComponent<PlayerHarvester>().myOrePileLocation = location;
        }
        else
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = location;
        }
    }

}