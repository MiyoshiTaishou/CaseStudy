using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_�N�g�̈ړ�����
public class M_DuctWarp : MonoBehaviour
{
    //�ݒ肵���_�N�g�Ɉړ�����
    [Header("��_�N�g"), SerializeField]
    private GameObject UpDuct;

    [Header("���_�N�g"), SerializeField]
    private GameObject DownDuct;

    [Header("���_�N�g"), SerializeField]
    private GameObject LeftDuct;

    [Header("�E�_�N�g"), SerializeField]
    private GameObject RightDuct;

    [Header("�ړ��ɂ����鎞��"), SerializeField]
    private float fMoveTime = 1.0f;

    [Header("�\������UI"), SerializeField]
    private GameObject UIObj;

    [Header("���鎞�ɖ炷SE"), SerializeField]
    private AudioSource SEDuctEnter;

    [Header("�ړ����ɖ炷SE"), SerializeField]
    private AudioSource SEDuctMove;

    /// <summary>
    /// �v���C���[
    /// </summary>
    private GameObject PlayerObj;
   
    /// <summary>
    /// �_�N�g�ɓ����Ă��邩
    /// </summary>
    static bool isInDuct;   

    /// <summary>
    /// �_�N�g�ɐG��Ă���
    /// </summary>
    private bool isTouch;

    /// <summary>
    /// �ړ���
    /// </summary>
    private bool isMoveDuct;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        if(!PlayerObj)
        {
            Debug.Log("�v���C���[��������܂���");
        }

        if (!UpDuct)
        {
            Debug.Log("��_�N�g��������܂���");
        }

        if (!DownDuct)
        {
            Debug.Log("���_�N�g��������܂���");
        }

        if (!LeftDuct)
        {
            Debug.Log("���_�N�g��������܂���");
        }

        if (!RightDuct)
        {
            Debug.Log("�E�_�N�g��������܂���");
        }

        //UI��\��
        UIObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�_�N�g�ړ����͏������Ȃ�
        if (isMoveDuct)
        {
            return;
        }

        //�G��Ă���Ԃ͉������тɓ�������o���肷��
        if (Input.GetKeyDown(KeyCode.V) && isTouch)
        {
            Debug.Log("�_�N�g�ɓ�����");
            isInDuct = !isInDuct;
            PlayerObj.transform.position = transform.position;
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        //�G��Ă���_�N�g�̈ړ�����
        if (isInDuct && isTouch)
        {            
            //�����Ă���Ԃ̏���
            InDuctMove();
        }
        
        if(isInDuct)
        {
            //�����Ȃ�
            //PlayerObj.GetComponent<SpriteRenderer>().enabled = false;
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
        }
        else
        {
            //�����Ȃ�
            PlayerObj.GetComponent<SpriteRenderer>().enabled = true;
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if(collision.gameObject.CompareTag("Player"))
        {
            isTouch = true;

            UIObj.SetActive(true);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouch = false;
            UIObj.SetActive(false);
        }
    }

    //�_�N�g���m���ړ�����
    void InDuctMove()
    {        
        //��_�N�g�Ɉړ�
        if(Input.GetKeyDown(KeyCode.W) && UpDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime,UpDuct));            
        }

        //���_�N�g�Ɉړ�
        if (Input.GetKeyDown(KeyCode.S) && DownDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, DownDuct));
        }

        //���_�N�g�Ɉړ�
        if (Input.GetKeyDown(KeyCode.A) && LeftDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, LeftDuct));
        }

        //�E�_�N�g�Ɉړ�
        if (Input.GetKeyDown(KeyCode.D) && RightDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, RightDuct));
        }
    }

    //�_�N�g�Ɉړ����郁�\�b�h
    IEnumerator IEMoveDuct(float _waitTime,GameObject _obj)
    {
        isMoveDuct = true;

        // �ҋ@����
        yield return new WaitForSeconds(_waitTime);

        PlayerObj.transform.position = _obj.transform.position;

        isMoveDuct = false;
    }
}
