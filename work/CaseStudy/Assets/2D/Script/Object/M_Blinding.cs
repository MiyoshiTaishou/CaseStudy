using System.Collections;
using System.Collections.Generic;
using System.Numerics;
#if UNITY_EDITOR
using UnityEditor.U2D.Path;
#endif
using UnityEngine;

public class M_Blinding : MonoBehaviour
{
    [Header("����܂ł̎���"),SerializeField]
    private float fDelay = 2.0f;

    [Header("������܂ł̎���"), SerializeField]
    private float fDeleteTime = 0.1f;

    /// <summary>
    /// �ڂ���܂��N�����Ă��邩
    /// </summary>
    private bool isEnable = false;
   
    private SpriteRenderer spriteRenderer;   
    private CircleCollider2D[] colliders;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        

        colliders = GetComponents<CircleCollider2D>();
       
        // ��莞�Ԍ�ɓ����蔻���L���ɂ���R���[�`�����J�n
        StartCoroutine(EnableColliderAfterDelay());
    }

    IEnumerator EnableColliderAfterDelay()
    {
        // ����܂ł̎��ԑҋ@
        yield return new WaitForSeconds(fDelay);
        
        //���̔�����I��
        isEnable = true;

        // ������܂ł̎��ԑҋ@
        yield return new WaitForSeconds(fDeleteTime);

        // �I�u�W�F�N�g���폜
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
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


        ////�G�l�~�[�̓����蔻��
        //if(_collision.gameObject.CompareTag("Enemy"))
        //{            
        //    //�����ƃG�l�~�[�̃x�N�g�������߂�
        //    UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;

        //    // ���g�̃R���C�_�[�̔��a���擾
        //    float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

        //    //�Ԃɕǂ��Ȃ���
        //    RaycastHit2D RayHit = Physics2D.Raycast(transform.position, vecPos.normalized, vecPos.magnitude);
           
        //    if (RayHit.collider == null)
        //    {
        //        Debug.Log("�G�l�~�[�q�b�g");

        //        //�G�l�~�[�̖ڂ���܂��ϐ���true�ɂ���
        //        _collision.gameObject.GetComponent<M_BlindingMove>().SetIsBlinding(true);

        //        //�G�l�~�[���玩�g�̌������擾
        //        UnityEngine.Vector2 vecDir = this.transform.position - _collision.transform.position;
        //        vecDir.Normalize();

        //        //������ݒ肷��                
        //        _collision.gameObject.GetComponent<M_BlindingMove>().SetVecDirBlinding(vecDir);
        //    }
        //}
    }

    public bool GetIsEnable()
    {
        return isEnable;
    }
}
