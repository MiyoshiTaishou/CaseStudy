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

        // �A�j���[�^�[�̃p�����[�^�[��ݒ肵�A�A�j���[�V�������Đ�����
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
            // �A�j���[�V�������I�������牽��������
            //Destroy(transform.Find("w").gameObject);
            transform.Find("w").gameObject.SetActive(false);
            animator.SetTrigger("enemy_roll_loop");
        }
        // �Đ����̃A�j���[�V�����̏����擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // �Đ����̃A�j���[�V�����̖��O���擾
        string currentAnimationName = stateInfo.IsName("enemy_roll_loop") ? "Idle" : "Unknown";

        // �擾�����A�j���[�V���������o��
        Debug.Log("Current Animation: " + currentAnimationName);
    }
}
