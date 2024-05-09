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
        animator.Play("enemy_roll_start");
        //animator.Play("enemy_roll_loop");
        animator.SetBool("roll", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("enemy_roll_start") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // アニメーションが終了したら何かをする
            //Destroy(transform.Find("w").gameObject);
            transform.Find("w").gameObject.SetActive(false);
            animator.SetTrigger("enemy_roll_loop");
        }
        // 再生中のアニメーションの情報を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 再生中のアニメーションの名前を取得
        string currentAnimationName = stateInfo.IsName("enemy_roll_loop") ? "Idle" : "Unknown";

        // 取得したアニメーション名を出力
        Debug.Log("Current Animation: " + currentAnimationName);
    }
}
