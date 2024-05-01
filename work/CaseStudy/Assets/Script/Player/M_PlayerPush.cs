using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class M_PlayerPush : MonoBehaviour
{
    /// <summary>
    /// どの状態の時に押せるようにするか
    /// </summary>
    enum MODE
    {
        Back,
        Blinding,
        None,
    }

    [Header("押せる条件"),SerializeField]
    MODE mode;

    [Header("押す力"), SerializeField]
    float fPower = 5.0f;

    [Header("振動時間"), SerializeField]
    private float fTime = 0.5f;

    /// <summary>
    /// プレイヤー
    /// </summary>
    GameObject PlayerObj;

    /// <summary>
    /// 押すオブジェクト
    /// </summary>
    GameObject PushObj;    

    /// <summary>
    /// 押せるかどうか
    /// </summary>
    private bool isPush = false;

    private Animator animator;
  
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        // Animatorコンポーネントを取得
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

                //押すオブジェクト代入
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

    //押す処理
    void Push(GameObject push)
    {
        //押せる条件
        switch (mode)
        {
            //条件なし
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

            //目くらまし中
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

            //バレていない時
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

                    Debug.Log("押した");
                    StartCoroutine(M_Utility.GamePadMotor(fTime));

                    animator.SetTrigger("Start");
                }
                
                break;
        }       
    }   
}
