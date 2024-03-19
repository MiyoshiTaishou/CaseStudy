using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    [Header("����͈͓��p���C�̐��i�����قǃL���C�ɕ`��j"), SerializeField]
    private int nNumRays = 3;

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
        // ���C���̒��_���W�̃��X�g
        List<Vector3> vecPositions = new List<Vector3>();

        //���C���̎n�_
        vecPositions.Add(transform.position);

        //���C�̊p�x�Ԋu
        float fStepAngleSize = fEnemyAngle / nNumRays;

        for(int i = 0; i <= nNumRays; i++)
        {
            // ���C�̊p�x���v�Z
            float angle = transform.eulerAngles.z - fEnemyAngle / 2 + fStepAngleSize * i;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * transform.right; // ���C�̕������v�Z

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dir, ColSearch.radius);

            if (rayHit.collider != null)
            {
                // ��Q���ɓ��������ꍇ�A��Q���܂ł̋����܂ł̓_��ǉ�
                vecPositions.Add(rayHit.point);
            }
            else
            {
                // ��Q���ɓ�����Ȃ������ꍇ�A����̒[�܂ł̓_��ǉ�
                vecPositions.Add(transform.position + dir * ColSearch.radius);
            }
        }

        //�Ō�Ɏ���͈͓��̌`��ɕ���
        vecPositions.Add(transform.position);

        lineRenderer.positionCount = vecPositions.Count;
        lineRenderer.SetPositions(vecPositions.ToArray());
      
        //�͈͓���h��Ԃ�       
        DrawFieldFill(vecPositions);
    }

    //�͈͓���h��Ԃ�����
    void DrawFieldFill(List<Vector3> vertices)
    {
        if (vertices.Count < 3) return; // ���_����3�����̏ꍇ�͕`�悵�Ȃ�
        
        // ���b�V�����쐬
        Mesh mesh = new Mesh();

        // 2D�̒��_���W�ɕϊ�
        Vector3[] vecVertices = new Vector3[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            vecVertices[i] = new Vector3(vertices[i].x, vertices[i].y, 0);
        }

        // ���b�V���ɒ��_��ݒ�
        mesh.vertices = vecVertices;

        // ���_�C���f�b�N�X�𐶐����Đݒ肷��
        int[] triangles = new int[(vertices.Count - 2) * 3];
        for (int i = 0, count = 0; i < vertices.Count - 2; i++, count += 3)
        {
            triangles[count] = 0;
            triangles[count + 1] = i + 2;
            triangles[count + 2] = i + 1;
        }

        // ���b�V���ɎO�p�`�̃C���f�b�N�X��ݒ�
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        // ���b�V����`��
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, MTDefault, 0);
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