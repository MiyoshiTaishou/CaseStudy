using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerPush : MonoBehaviour
{
    /// <summary>
    /// �ǂ̏�Ԃ̎��ɉ�����悤�ɂ��邩
    /// </summary>
    enum MODE
    {
        Back,
        Blinding,
        None,
    }

    [Header("���������"),SerializeField]
    MODE mode;

    [Header("������"), SerializeField]
    float fPower = 5.0f;

    /// <summary>
    /// �v���C���[
    /// </summary>
    GameObject PlayerObj;

    /// <summary>
    /// �����I�u�W�F�N�g
    /// </summary>
    GameObject PushObj;

    /// <summary>
    /// �����邩�ǂ���
    /// </summary>
    private bool isPush = false;
   
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerObj.GetComponent<M_PlayerMove>().GetDir().x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(PlayerObj.GetComponent<M_PlayerMove>().GetDir().x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }   
        
        if(isPush && PushObj)
        {
            Push(PushObj);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {            
            if (collision.tag == "Enemy")
            {
                isPush = true;

                //�����I�u�W�F�N�g���
                PushObj = collision.gameObject;
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.tag == "Enemy")
            {
                isPush = false;

                PushObj = null;
            }
        }
    }

    //��������
    void Push(GameObject push)
    {
        //���������
        switch (mode)
        {
            //�����Ȃ�
            case MODE.None:

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("EnemyPush"))
                {
                    Vector3 dir = PlayerObj.GetComponent<M_PlayerMove>().GetDir();

                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);
                }

                break;

            //�ڂ���܂���
            case MODE.Blinding:
               
                if ((Input.GetKeyDown(KeyCode.Return)|| Input.GetButtonDown("EnemyPush")) && push.GetComponent<M_BlindingMove>().GetIsBlinding())
                {
                    Vector3 dir = PlayerObj.GetComponent<M_PlayerMove>().GetDir();

                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);
                }

                break;

            //�o���Ă��Ȃ���
            case MODE.Back:

                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("EnemyPush")) && !push.GetComponent<MPlayerSearch>().GetIsSearch())
                {
                    Vector3 dir = PlayerObj.GetComponent<M_PlayerMove>().GetDir();

                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);
                }

                break;
        }       
    }
}
