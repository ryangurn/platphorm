using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    void FixedUpdate()
    {
        if (gameObject.activeInHierarchy)
            Pause();

    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    public void unPause()
    {
        Time.timeScale = 1;
    }
}
