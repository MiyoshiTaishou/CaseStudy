using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DropAnimation : MonoBehaviour
{
    [Header("�A�j���[�V�����p�{�[��"), SerializeField]
    private GameObject AnimBone;

    private bool isDrop = false;

    // Update is called once per frame
    void Update()
    {       
        AnimBone.GetComponent<Animator>().SetBool("Drop", isDrop);       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("�������Ă���");
        isDrop = false;
    }    

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�������Ă��Ȃ�");
        isDrop = true;
    }
}
