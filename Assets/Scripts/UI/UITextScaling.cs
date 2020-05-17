using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextScaling : MonoBehaviour
{
    Text m_Text;

    void Start()
    {
      m_Text = GetComponent<Text>();
    }

    public void Scale(Slider slide)
    {
      m_Text.fontSize = Mathf.RoundToInt(slide.value);
    }
}
