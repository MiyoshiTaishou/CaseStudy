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
    /// プレイヤー
    /// </summary>
    private GameObject PlayerObj;

    /// <summary>
    /// プレイヤーが持っているレンダラ
    /// </summary>
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    /// <summary>
    /// ダクトに入っているか
    /// </summary>
    static bool isInDuct;   

    /// <summary>
    /// ダクトに触れている
    /// </summary>
    private bool isTouch;

    /// <summary>
    /// 移動中
    /// </summary>
    private bool isMoveDuct;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        if(!PlayerObj)
        {
            Debug.Log("プレイヤーが見つかりません");
        }

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

        // プレイヤーが持っているSpriteRendererを全て取得
        SpriteRenderer[] spriteRenderers = PlayerObj.GetComponentsInChildren<SpriteRenderer>();

        if(spriteRenderers.Length==0)
        {
            Debug.Log("持ってません");
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            renderers.Add(spriteRenderer);
        }

        //UI非表示
        UIObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ダクト移動中は処理しない
        if (isMoveDuct)
        {
            return;
        }

        //触れている間は押すたびに入ったり出たりする
        if (Input.GetKeyDown(KeyCode.V) && isTouch)
        {
            Debug.Log("ダクトに入った");
            isInDuct = !isInDuct;

            if (isInDuct)
            {
                // ダクトに入ったらプレイヤーのレイヤーを変更
                PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            else
            {
                // ダクトから出たらプレイヤーの元のレイヤーに戻す
                PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer"); ;
            }

            PlayerObj.transform.position = transform.position;
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        //触れているダクトの移動処理
        if (isInDuct && isTouch)
        {            
            //入っている間の処理
            InDuctMove();
        }
        
        if(isInDuct)
        {            
            //見えないようにする
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = false;
            }           
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);            
        }
        else
        {
            //見えるようにする
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = true;
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);                        
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

    //ダクト同士を移動する
    void InDuctMove()
    {        
        //上ダクトに移動
        if(Input.GetKeyDown(KeyCode.W) && UpDuct)
        {
            StartCoroutine(IEMoveDuct(fMoveTime,UpDuct));            
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
    IEnumerator IEMoveDuct(float _waitTime,GameObject _obj)
    {
        isMoveDuct = true;

        // 待機時間
        yield return new WaitForSeconds(_waitTime);

        PlayerObj.transform.position = _obj.transform.position;

        isMoveDuct = false;
    }
}
