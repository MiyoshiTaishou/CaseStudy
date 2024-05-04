using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AnimationControll : MonoBehaviour
{
    [Header("�A�j���[�V�����̖��O"), SerializeField]
    string AnimationName;

    private int nAnimationFrame = 0;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null )
        {
            Debug.LogError(transform.name+"��Animator�Ȃ�����"+transform.root.name);
        }
        // �V�[�h���w�肵�ė����W�F�l���[�^��������
        System.Random rand = new System.Random();

        // �͈͂��w�肵�Đ����̗����𐶐�
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
