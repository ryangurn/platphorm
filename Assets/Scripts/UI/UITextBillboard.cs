using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to canvas
public class UITextBillboard : MonoBehaviour
{
    private GameObject cameraTransform;

    void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
      transform.rotation = cameraTransform.transform.rotation;
    }
}
