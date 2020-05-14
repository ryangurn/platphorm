using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to canvas
public class UITextBillboard : MonoBehaviour
{
    public Camera cameraTransform;

    void Update()
    {
      transform.rotation = cameraTransform.transform.rotation;
    }
}
