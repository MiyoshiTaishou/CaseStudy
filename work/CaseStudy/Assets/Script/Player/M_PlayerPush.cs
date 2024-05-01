using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("�U������"), SerializeField]
    private float fTime = 0.5f;

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

    private Animator animator;
  
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {      
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
                    Vector3 dir;

                    if (this.transform.eulerAngles.y == 180.0f)
                    {
                        dir = transform.right;
                    }
                    else
                    {
                        dir = -transform.right;
                    }

                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);
                }

                break;

            //�ڂ���܂���
            case MODE.Blinding:
               
                if ((Input.GetKeyDown(KeyCode.Return)|| Input.GetButtonDown("EnemyPush")) && push.GetComponent<M_BlindingMove>().GetIsBlinding())
                {
                    Vector3 dir;

                    if (this.transform.eulerAngles.y == 180.0f)
                    {
                        dir = transform.right;
                    }
                    else
                    {
                        dir = -transform.right;
                    }

                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);
                }

                break;

            //�o���Ă��Ȃ���
            case MODE.Back:

                if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("EnemyPush")) && !push.GetComponent<MPlayerSearch>().GetIsSearch())
                {
                    Vector3 dir = Vector3.zero;                    

                    if (PlayerObj.transform.eulerAngles.y >= 180.0f)
                    {
                        dir = transform.right;
                    }     
                    else if(PlayerObj.transform.eulerAngles.y <= 60.0f)
                    {
                        Debug.Log(PlayerObj.transform.eulerAngles);
                        dir = transform.right;
                    }

                    push.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);

                    Debug.Log("������");
                    StartCoroutine(M_Utility.GamePadMotor(fTime));

                    animator.SetTrigger("Start");
                }
                
                break;
        }       
    }   
}
