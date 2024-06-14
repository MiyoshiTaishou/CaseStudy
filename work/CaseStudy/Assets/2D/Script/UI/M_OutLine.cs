using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_OutLine : MonoBehaviour
{
    [Header("アウトラインカラー"), SerializeField]
    private Color selectColor;  

    public void OutLineOn()
    {
        GetComponent<Outline>().effectColor = selectColor;
    }

    public void OutLineOff()
    {
        GetComponent<Outline>().effectColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
