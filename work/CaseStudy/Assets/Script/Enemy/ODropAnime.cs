using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class ODropAnime : MonoBehaviour
{
    private bool isDrop = false;

    private void OnCollisionEnter2D(Collision2D ot)
    {
        //コライダーが当たっていると継続して呼ばれる
        isDrop = false;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //コライダーが離れた時に呼ばれる
        isDrop = true;
    }

}
