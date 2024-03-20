using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڂ���܂����̍s��
public class M_BlindingMove : MonoBehaviour
{
    [Header("�ڂ���܂����̈ړ����x"), SerializeField]
    private float fMoveSpeed = 5.0f;

    [Header("�ڂ���܂��̎���"), SerializeField]
    private float fBlindingTime = 5.0f;

    /// <summary>
    /// �ڂ���܂�����
    /// </summary>
    private bool isBlinding = false;

    /// <summary>
    /// ���Ԍv���p
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// �ڂ���܂��̕���
    /// </summary>
    private Vector2 vecDirBlinding;

    private Rigidbody2D rbEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ڂ���܂������ǂ���
        if(isBlinding)
        {
            //���Ԍv��
            fTime += Time.deltaTime;

            BlindingMove();
        }
        else
        {
            fTime = 0.0f;
        }

        //�ڕW���Ԃ܂Ōo�߂�����ڂ���܂�����
        if(fTime > fBlindingTime)
        {
            isBlinding = false;
            fTime = 0.0f;
        }
    }

    /// <summary>
    /// �ڂ���܂����̍s��
    /// </summary>
    private void BlindingMove()
    {       
        //�����͐ݒ�
        Vector2 vecMoveDirection = new Vector2(vecDirBlinding.x * fMoveSpeed, rbEnemy.velocity.y);
        rbEnemy.velocity = vecMoveDirection;
    }

    public void SetIsBlinding(bool _isBlinding)
    {
        isBlinding = _isBlinding;
    }

    public bool GetIsBlinding()
    {
        return isBlinding;
    }

    public void SetVecDirBlinding(Vector2 _vecDir)
    {
        vecDirBlinding = _vecDir;
    }
}
