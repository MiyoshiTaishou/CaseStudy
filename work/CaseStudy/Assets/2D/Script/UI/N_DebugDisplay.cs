using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class N_DebugDisplay : MonoBehaviour
{
    [Header("•\Ž¦‚·‚é?"), SerializeField]
    private bool isDisplay = false;

    private TextMeshProUGUI textMesh;

    public static float fNum = 0.0f;
    public static Vector3 pos = Vector3.zero;
    public static Vector3 size = Vector3.zero;
    public static Vector2 offset = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisplay)
        {
            textMesh.text = fNum.ToString();
            textMesh.text = textMesh.text + "\n" + "pos " + pos.ToString();
            textMesh.text = textMesh.text + "\n" + "size " + size.ToString();
            textMesh.text = textMesh.text + "\n" + "offset " + offset.ToString();
        }
    }
}
