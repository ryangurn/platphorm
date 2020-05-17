using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQueueRotation : MonoBehaviour
{
    Vector3 m_targetRotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      m_targetRotation.y += 1;
      transform.localRotation = Quaternion.Euler(m_targetRotation);
    }
}
