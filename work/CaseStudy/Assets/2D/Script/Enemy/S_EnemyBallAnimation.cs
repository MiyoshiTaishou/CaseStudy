using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyBallAnimation : MonoBehaviour
{
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // アニメーターのパラメーターを設定し、アニメーションを再生する
        //animator.Play("enemy_roll_start");
        animator.SetBool("roll", true);
        animator.Play("enemy_roll_loop");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("enemy_roll_start") &&
        //    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    // アニメーションが終了したら何かをする
        //    //Destroy(transform.Find("w").gameObject);
        //    transform.Find("w").gameObject.SetActive(false);
        //    Vector3 scale= transform.localScale;
        //    scale.x = 0.5f;
        //    scale.y = 0.5f;
        //    transform.localScale = scale;
        //    animator.SetTrigger("enemy_roll_loop");
        //}
    }
}
