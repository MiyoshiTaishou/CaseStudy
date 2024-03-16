using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[���T�[�`���鏈��
//Tag�����R�ɐݒ肵�ĒǐՂ���I�u�W�F�N�g��ς���Ƃ����ł���悤�ɂ�����

public class MPlayerSearch : MonoBehaviour
{
    [Header("����p"), SerializeField]
    private float fEnemyAngle = 45.0f;

    [Header("���F���̃}�e���A��"), SerializeField]
    private Material MTSearch;

    [Header("�ړ����x"), SerializeField]
    private float fMoveSpeed;

    /// <summary>
    /// ���X�̃}�e���A��
    /// </summary>
     private  Material MTDefault;

    /// <summary>
    /// �������Ă��邩
    /// </summary>
    private bool isSearch = false;

    /// <summary>
    /// ����͈͓���\�����郌���_��
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// ���G�p�R���C�_�[
    /// </summary>
    private CircleCollider2D ColSearch;

    /// <summary>
    /// �^�[�Q�b�g�̍��W
    /// </summary>
    private Transform TargetTransform;
   
    // Start is called before the first frame update
    void Start()
    {
        //���X�̃}�e���A����ۑ�
        MTDefault = GetComponent<SpriteRenderer>().material; 
        
        lineRenderer = GetComponent<LineRenderer>();

        //���_���ݒ�
        lineRenderer.positionCount = 3;

        // ���̑�����ݒ�
        lineRenderer.startWidth = 0.05f; // ���̎n�_�̑���
        lineRenderer.endWidth = 0.05f;   // ���̏I�_�̑���

        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();

        foreach(CircleCollider2D col in colliders)
        {
            if(col.isTrigger)
            {
                ColSearch = col;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isSearch);

        //�������Ă���
        if(isSearch)
        {
            //����������F��ς���
            this.GetComponent<SpriteRenderer>().material = this.MTSearch;
            Chase();
        }
        else
        {            
            //�������Ă��Ȃ��Ȃ�f�t�H���g
            this.GetComponent<SpriteRenderer>().material = this.MTDefault;
        }

        // ����͈͂�`�悷��
        DrawFieldOfView();
    }

    //����p���ɓ��������ǂ������m
    private void OnTriggerStay2D(Collider2D _collision)
    {
        //���E�͈̔͂̓����蔻��
        if (_collision.gameObject.CompareTag("Player"))
        {            
            //���E�͈͓̔��Ɏ��܂��Ă��邩

            //�����ƃv���C���[�̃x�N�g�������߂�
            Vector2 vecPos = _collision.transform.position - this.transform.position;

            //�O�����x�N�g����vecPos�̊p�x�����߂�
            float fPlayerAngle = Vector2.Angle(this.transform.right, vecPos);

            //fPlayerAngle������p���Ɏ��܂��Ă��邩
            if (fPlayerAngle < fEnemyAngle * 0.5f)
            {               
                // ���g�̃R���C�_�[�̔��a���擾
                float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

                //�Ԃɕǂ��Ȃ���
                RaycastHit2D RayHit = Physics2D.Raycast(transform.position + transform.right * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);
               
                if (RayHit.collider != null && RayHit.collider.CompareTag("Player"))
                {
                    Debug.Log("���F��");
                    isSearch = true;

                    //�^�[�Q�b�g�̍��W��ۑ�
                    TargetTransform = _collision.transform;
                }
                else
                {
                    isSearch = false;
                }
            }
            else
            {
                isSearch = false;
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isSearch = false;
    }

    //����͈͓���\������
    void DrawFieldOfView()
    {
        Vector3[] vecPositions = new Vector3[3]; // ���C���̒��_���W�̔z��

        // ���C���̎n�_��ݒ�i�G�L�����N�^�[�̈ʒu�j
        vecPositions[0] = transform.position;

        // ����͈͂̒[�_���v�Z
        Vector3 vecEndPositionRight = transform.position + Quaternion.Euler(0, 0, fEnemyAngle * 0.5f) * transform.right * ColSearch.radius;
        Vector3 vecEndPositionLeft = transform.position + Quaternion.Euler(0, 0, -fEnemyAngle * 0.5f) * transform.right * ColSearch.radius;

        // ���C���̒[�_��ݒ�
        vecPositions[1] = vecEndPositionRight;
        vecPositions[2] = vecEndPositionLeft;

        lineRenderer.SetPositions(vecPositions);
        lineRenderer.loop = true; // �O�p�`�����
    }

    //�ǐՏ����i�ȈՔŁj
    void Chase()
    {
        if(TargetTransform != null)
        {
            //�^�[�Q�b�g�Ɍ������Ĉړ����邾��
            Vector2 vecDir = (TargetTransform.position - transform.position).normalized;
            transform.Translate(vecDir * Time.deltaTime * fMoveSpeed);
        }
    }
}
