using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyBallAnimation3DK : MonoBehaviour
{
    private Animator animator;
    private S_EnemyBall3DK ball =null;
    [Header("すぴーど"), SerializeField]
    float fspeed;
    // Start is called before the first frame update
    void Start()
    {
        ball= transform.parent.GetComponent<S_EnemyBall3DK>();
        if(ball == null ) 
        {
            Debug.Log("ballがない");
        }
        animator = GetComponent<Animator>();

        // アニメーターのパラメーターを設定し、アニメーションを再生する
        AnimPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("enemy_roll_loop_Reverce"))
        {
            Debug.Log("The animation 'AnimationName' is currently playing.");
        }
        if(ball.GetisPushing() == true)
        {
            AnimPlay();
        }
        else if(ball.GetisPushing() == false) 
        {
            animator.speed = 0.0f;
        }
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

    void AnimPlay()
    {
        animator.speed = 1.0f;

        if (!ball.GetisLeft())
        {
            animator.Play("enemy_roll_loop");
        }
        else if (ball.GetisLeft())
        {
            animator.Play("enemy_roll_loop_Reverce");
        }
    }
}
