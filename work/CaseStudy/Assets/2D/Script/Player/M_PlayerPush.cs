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

    [Header("風のアニメーション再生タイミング"), SerializeField]
    private float fDlayAnim = 1.0f;

    /// <summary>
    /// プレイヤー
    /// </summary>
    GameObject PlayerObj;

    /// <summary>
    /// ダクトマネージャ
    /// </summary>
    GameObject DuctManager;
    
    /// <summary>
    /// 押すオブジェクト
    /// </summary>
    GameObject PushObj;

    List<GameObject> PushList = new List<GameObject>();

    /// <summary>
    /// 押せるかどうか
    /// </summary>
    private bool isPush = false;

    /// <summary>
    ///アニメーション関連
    /// </summary>
    private Animator animator;   
    private Animator animator2;   
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        animator2 = GameObject.Find("T_main_chara").GetComponent<Animator>();

        DuctManager = GameObject.Find("DuctManager");

        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {      
        if(!M_GameMaster.GetGamePlay() || DuctManager.GetComponent<M_DuctManager>().ContainsTrueValue())
        {
            return;
        }

        if(isPush && PushObj && !animator2.GetCurrentAnimatorStateInfo(0).IsName("player_push") && !animator.GetCurrentAnimatorStateInfo(0).IsName("kaze01"))
        {
            float Distance = 100.0f;
            Vector3 pos=PlayerObj.transform.position;
            for (int i = 0; i < PushList.Count; i ++)
            {
                float check = (PushList[i].transform.position - pos).magnitude;
                if(check<Distance)
                {
                    Distance = check;
                    PushObj = PushList[i];
                }
            }

            Push(PushObj);            
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("EnemyPush") && !animator2.GetCurrentAnimatorStateInfo(0).IsName("player_push") && !animator.GetCurrentAnimatorStateInfo(0).IsName("kaze01"))
        {           
            animator2.SetTrigger("push");
            StartCoroutine(IEAnimDlay(fDlayAnim));
        }
      
        //アニメーション再生中は動かないようにする
        if(animator2.GetCurrentAnimatorStateInfo(0).IsName("player_push") && animator.GetCurrentAnimatorStateInfo(0).IsName("kaze01"))
        {
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
        }            
        else
        {
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {            
            if (collision.tag == "Enemy" || collision.tag == "EnemyBall")
            {
                isPush = true;
                //押すオブジェクト代入
                PushObj = collision.gameObject;
                if (PushList.Contains(collision.gameObject) == false)
                {
                    PushList.Add(collision.gameObject);
                }
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.tag == "Enemy" || collision.tag == "EnemyBall")
            {
                isPush = false;

                PushObj = null;

                if (PushList.Contains(collision.gameObject))
                {
                    PushList.Remove(collision.gameObject);
                }
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

                    //push.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    push.GetComponent<Rigidbody2D>().AddForce(dir * fPower, ForceMode2D.Impulse);
                    push.GetComponent<S_EnemyBall>().SetisPushing(true);

                    Debug.Log("押した");
                    StartCoroutine(M_Utility.GamePadMotor(fTime));                   
                }
                
                break;
        }       
    }

    IEnumerator IEAnimDlay(float _waitTime)
    {
        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        animator.SetTrigger("Start");
    }

}
