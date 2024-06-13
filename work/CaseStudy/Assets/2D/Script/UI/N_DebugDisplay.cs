using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class N_DebugDisplay : MonoBehaviour
{
    [Header("•\Ž¦‚·‚é?"), SerializeField]
    private bool isDisplay = false;

    [Header("“G"), SerializeField]
    private List<Rigidbody2D> ListRigid = new List<Rigidbody2D>();

    private TextMeshProUGUI textMesh;

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
            textMesh.text = "";
            foreach (var r in ListRigid)
            {
                if (r != null)
                {
                    textMesh.text = textMesh.text + r.velocity.ToString() + "\n";
                }
                else
                {
                    textMesh.text = textMesh.text + "\n";
                }
            }
            //textMesh.text = fNum.ToString();
            //textMesh.text = textMesh.text + "\n" + "pos " + pos.ToString();
            //textMesh.text = textMesh.text + "\n" + "size " + size.ToString();
            //textMesh.text = textMesh.text + "\n" + "offset " + offset.ToString();
        }
    }
}
