using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AnimationControll : MonoBehaviour
{
    [Header("アニメーションの名前"), SerializeField]
    string AnimationName;

    private int nAnimationFrame = 0;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null )
        {
            Debug.LogError(transform.name+"にAnimatorないっす"+transform.root.name);
        }
        // シードを指定して乱数ジェネレータを初期化
        System.Random rand = new System.Random();

        // 範囲を指定して整数の乱数を生成
        int minRange = 0;
        int maxRange = 70;
        nAnimationFrame = rand.Next(minRange, maxRange);
        animator.Play("enemy_walk", 0, nAnimationFrame/70f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
