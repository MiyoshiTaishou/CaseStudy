using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class ODropAnime : MonoBehaviour
{
    private bool isDrop = false;

    private void OnCollisionEnter2D(Collision2D ot)
    {
        //�R���C�_�[���������Ă���ƌp�����ČĂ΂��
        isDrop = false;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //�R���C�_�[�����ꂽ���ɌĂ΂��
        isDrop = true;
    }

}
