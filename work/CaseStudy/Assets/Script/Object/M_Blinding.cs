using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class M_Blinding : MonoBehaviour
{
    [Header("����܂ł̎���"),SerializeField]
    private float fDelay = 2.0f;

    [Header("������܂ł̎���"), SerializeField]
    private float fDeleteTime = 1.0f;
   
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;

        // ��莞�Ԍ�ɓ����蔻���L���ɂ���R���[�`�����J�n
        StartCoroutine(EnableColliderAfterDelay());
    }

    IEnumerator EnableColliderAfterDelay()
    {
        // ����܂ł̎��ԑҋ@
        yield return new WaitForSeconds(fDelay);

        // �����蔻���L���ɂ���
        circleCollider.enabled = true;

        // ������܂ł̎��ԑҋ@
        yield return new WaitForSeconds(fDeleteTime);

        // �I�u�W�F�N�g���폜
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {        
        if (_collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(_collision.name);
            //�����ƃv���C���[�̃x�N�g�������߂�
            UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;
           
            //�Ԃɕǂ��Ȃ���
            RaycastHit2D RayHit = Physics2D.Raycast(transform.position , vecPos.normalized, vecPos.magnitude);
           
            if (RayHit.collider != null && RayHit.collider.CompareTag("Player"))
            {
                Debug.Log(RayHit.collider.name + "HIT");                
            }                    
        }


        //�G�l�~�[�̓����蔻��
        if(_collision.gameObject.CompareTag("Enemy"))
        {
            //�����ƃv���C���[�̃x�N�g�������߂�
            UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;

            // ���g�̃R���C�_�[�̔��a���擾
            float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

            //�Ԃɕǂ��Ȃ���
            RaycastHit2D RayHit = Physics2D.Raycast(transform.position + transform.right * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);
            
            if (RayHit.collider == null)
            {
                Debug.Log("�G�l�~�[�q�b�g");
            }
        }
    }
}
