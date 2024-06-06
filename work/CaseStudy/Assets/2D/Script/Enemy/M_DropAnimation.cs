using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DropAnimation : MonoBehaviour
{
    [Header("アニメーション用ボーン"), SerializeField]
    private GameObject AnimBone;

    private bool isDrop = false;

    // Update is called once per frame
    void Update()
    {       
        AnimBone.GetComponent<Animator>().SetBool("Drop", isDrop);                
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isDrop = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isDrop = true;
    }
}
