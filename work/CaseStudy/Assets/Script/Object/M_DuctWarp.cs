using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダクトの移動処理
public class M_DuctWarp : MonoBehaviour
{
    //設定したダクトに移動する
    [Header("上ダクト"), SerializeField]
    private GameObject UpDuct;

    [Header("下ダクト"), SerializeField]
    private GameObject DownDuct;

    [Header("左ダクト"), SerializeField]
    private GameObject LeftDuct;

    [Header("右ダクト"), SerializeField]
    private GameObject RightDuct;

    [Header("移動にかかる時間"), SerializeField]
    private float fMoveTime = 1.0f;

    [Header("表示するUI"), SerializeField]
    private GameObject UIObj;

    [Header("入る時に鳴らすSE"), SerializeField]
    private AudioSource SEDuctEnter;

    [Header("移動時に鳴らすSE"), SerializeField]
    private AudioSource SEDuctMove;
   
    /// <summary>
    /// ダクトに触れている
    /// </summary>
    private bool isTouch;

    /// <summary>
    /// ダクトマネージャ
    /// </summary>
    private GameObject DuctManager;

    /// <summary>
    /// 移動中
    /// </summary>
    private bool isMoveDuct;
  
    // 20240407 二宮追記
    /// <summary>
    /// 対象追跡カメラスクリプト
    /// </summary>
    private N_TrackingPlayer trackingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        DuctManager = GameObject.Find("DuctManager");

        if (!UpDuct)
        {
            Debug.Log("上ダクトが見つかりません");
        }

        if (!DownDuct)
        {
            Debug.Log("下ダクトが見つかりません");
        }

        if (!LeftDuct)
        {
            Debug.Log("左ダクトが見つかりません");
        }

        if (!RightDuct)
        {
            Debug.Log("右ダクトが見つかりません");
        }      
       
        //UI非表示
        UIObj.SetActive(false);

        // 20240407 二宮追記
        trackingPlayer = GameObject.Find("Main Camera").GetComponent<N_TrackingPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //ダクト移動中は処理しない
        if (isMoveDuct)
        {
            return;
        }

        //入る処理
        if (Input.GetKeyDown(KeyCode.V) && isTouch)
        {           
            //マネージャに自身のダクトにプレイヤーが入ったことを知らせる
            DuctManager.GetComponent<M_DuctManager>().SetContains(this.gameObject, true);
        }      
        
        if(DuctManager.GetComponent<M_DuctManager>().GetValue(gameObject))
        {
            InDuctMove();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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

    //ダクト同士を移動する
    void InDuctMove()
    {
        Debug.Log(gameObject);

        //上ダクトに移動
        if (Input.GetKeyDown(KeyCode.W) && UpDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, UpDuct));
        }

        //下ダクトに移動
        if (Input.GetKeyDown(KeyCode.S) && DownDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, DownDuct));
        }

        //左ダクトに移動
        if (Input.GetKeyDown(KeyCode.A) && LeftDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, LeftDuct));
        }

        //右ダクトに移動
        if (Input.GetKeyDown(KeyCode.D) && RightDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime, RightDuct));
        }
    }

    //ダクトに移動するメソッド
    IEnumerator IEMoveDuct(float _waitTime, GameObject _obj)
    {
        isMoveDuct = true;

        // 20240407 二宮追記
        trackingPlayer.SetWarpInfo(_waitTime, _obj);

        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        //ダクトマネージャのワープ処理を呼ぶ
        DuctManager.GetComponent<M_DuctManager>().DuctWarp(_obj, this.gameObject);

        isMoveDuct = false;
    }
}
