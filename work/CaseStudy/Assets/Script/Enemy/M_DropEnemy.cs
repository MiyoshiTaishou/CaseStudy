using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミーのアニメーション再生
//ドロップ以外もやるかも
public class M_DropEnemy : MonoBehaviour
{
    private Animator m_Animator;

    private bool isGround = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.SetBool("isDrop", false);
    }

    private void Update()
    {
        //レイを飛ばして下に何もなければ
        // 自身の下方向にRayを照射して地面をチェック
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
       
        if (hit.collider != null)
        {
            isGround = true; // 地面に接地している
        }
        else
        {
            isGround = false; // 地面に接地していない
        }

        Debug.Log(isGround);

        // 地面に接地していなければドロップ状態に設定
        m_Animator.SetBool("isDrop", !isGround);
    }
}
