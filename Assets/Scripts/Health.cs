using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthLevel;


    void Update()
    {
        TextMesh tm = GetComponentInChildren<TextMesh>();
        tm.text = new string('-', Mathf.FloorToInt(HealthLevel));

        Color color;

        switch (Mathf.FloorToInt(HealthLevel))
        {
            default:
                color = Color.green;
                break;
            case (3):
                color = Color.magenta;
                break;
            case (2):
                color = Color.yellow;
                break;
            case (1):
                color = Color.red;
                break;
            case (0):
                color = Color.white;
                break;
        }

        tm.GetComponent<Renderer>().material.color = color;

        tm.transform.forward = Camera.main.transform.forward;


    }



    void LateUpdate()
    {  
        if (HealthLevel < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator RunDamageAnimation()
    {
        GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1);

        GetComponent<ParticleSystem>().Stop();

    }

}
