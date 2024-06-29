using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダクトを管理する
/// </summary>

public class M_DuctManager : MonoBehaviour
{
    // GameObject をキーとし、bool を値とする辞書
    private static Dictionary<GameObject, bool> ductDictionary = new Dictionary<GameObject, bool>();

    /// <summary>
    /// 移動中かどうか
    /// </summary>
    private bool isMove = false;

    /// <summary>
    /// 入った瞬間
    /// </summary>
    private bool isDuctIn = false;

    public bool isNowDuct = false;

    public bool GetNowDuct()
    {
        return isNowDuct;
    }

    public void SetIsMove(bool _move) { isMove = _move; }

    public bool GetIsMove() { return isMove; }

    public void PlayDuctInSE() { GetComponent<AudioSource>().Play(); }

    /// <summary>
    /// プレイヤー
    /// </summary>
    private GameObject PlayerObj;
    private GameObject PlayerRes;

    /// <summary>
    /// プレイヤーが持っているレンダラ
    /// </summary>
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private Animator animator;
    private bool init = false;
    private bool warp = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerRes = GameObject.Find("PlayerRespawn");

        if (!PlayerObj)
        {
            Debug.Log("プレイヤーが見つかりません");
        }

        // "Duct" タグが付いているすべての GameObject を取得して辞書に追加
        GameObject[] ducts = GameObject.FindGameObjectsWithTag("Duct");
        foreach (GameObject duct in ducts)
        {
            // 各 GameObject をキーとして、最初は false で設定した状態を辞書に追加
            ductDictionary.Add(duct, false);
        }

        // プレイヤーが持っているSpriteRendererを全て取得
        SpriteRenderer[] spriteRenderers = PlayerObj.GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length == 0)
        {
            Debug.Log("持ってません");
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            renderers.Add(spriteRenderer);
        }

        // 辞書のキーのコレクションを取得
        var keys = new List<GameObject>(ductDictionary.Keys);

        // キーのコレクション上で反復処理を行い、値を変更
        foreach (var key in keys)
        {
            ductDictionary[key] = false;
        }

        // ダクトから出たらプレイヤーの元のレイヤーに戻す
        PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");

        //見えるようにする
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = true;
        }
        PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
        //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
        PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
        PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
        PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

        isNowDuct = false;
    }

    private void Update()
    {
        if (!init)
        {
            animator = PlayerObj.transform.GetChild(3).GetComponent<Animator>();

            init = false;
        }

        GameObject ductObj = GetObjectInDictionary();
        bool isRockDuct = false;
        if (GetObjectInDictionary() != null)
        {
            //Debug.Log("nullじゃない");
            isRockDuct = ductObj.GetComponent<M_DuctWarp>().GetisRock();

        }

        if (ductDictionary.ContainsValue(true) && Input.GetButtonDown("Duct") && !isMove && !isDuctIn && !isRockDuct && !animator.GetBool("duct"))
        {           
            Debug.Log("ダクトを出た");

            GetComponent<AudioSource>().Play();

            // 辞書のキーのコレクションを取得
            var keys = new List<GameObject>(ductDictionary.Keys);

            // キーのコレクション上で反復処理を行い、値を変更
            foreach (var key in keys)
            {
                ductDictionary[key] = false;
            }

            // ダクトから出たらプレイヤーの元のレイヤーに戻す
            PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");

            //見えるようにする
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = true;
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

            animator.SetBool("ductFinish", true);

            isNowDuct = false;

        }

        // ダクトがロックされていたら
        if (ductDictionary.ContainsValue(true) && Input.GetButtonDown("Duct") && !isMove && !isDuctIn && !isRockDuct)
        {
            // se再生

        }

        if (ductDictionary.ContainsValue(true))
        {
            // ダクトに入ったらプレイヤーのレイヤーを変更
            PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (isDuctIn && !warp)
            {
                animator.SetBool("duct", true);

            }

            if (!animator.GetBool("duct") && !isDuctIn)
            {
                //見えないようにする
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].enabled = false;
                }
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerMove>().SetIsNowDahs(false);
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(false);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = false;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = false;

        }

        warp = false;
        isDuctIn = false;
    }
    //M_DuctWarpから呼ばれて指定したところまでプレイヤーを飛ばす
    public void DuctWarp(GameObject _duct1, GameObject _duct2)
    {
        PlayerObj.transform.position = _duct1.transform.position;

        warp = true;

        // _duct1 を true に、_duct2 を false に設定
        SetContains(_duct2, false);
        SetContains(_duct1, true);       
    }

    // ダクトの状態を更新するメソッド
    public void SetContains(GameObject duct, bool state)
    {
        ductDictionary[duct] = state;        

        // ダクトの状態が変わった後にプレイヤーの挙動を制御する
        if (ContainsTrueValue())
        {
            Debug.Log("ダクトに入った");

            //GetComponent<AudioSource>().Play();

            // ダクトに入ったらプレイヤーのレイヤーを変更
            PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            Vector3 vecPos = duct.transform.position;
            PlayerObj.transform.position = new Vector3(vecPos.x, vecPos.y, -1.0f);
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            ////見えないようにする
            //for (int i = 0; i < renderers.Count; i++)
            //{
            //    renderers[i].enabled = false;
            //}
            PlayerObj.GetComponent<M_PlayerMove>().FullStamina();
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerMove>().SetIsNowDahs(false);
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(false);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = false;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = false;

            //Debug.Log(PlayerObj.GetComponent<M_PlayerMove>().GetIsMove());

            isNowDuct = true;

            isDuctIn = true;
        }
        else
        {
            //Debug.Log("ダクトを出た");

            //// ダクトから出たらプレイヤーの元のレイヤーに戻す
            //PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");            

            ////見えるようにする
            //for (int i = 0; i < renderers.Count; i++)
            //{
            //    renderers[i].enabled = true;
            //}
            //PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            ////PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            //PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
            //PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
            //PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            //PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

            //isNowDuct = false;
        }
    }

    // trueになっているダクトのオブジェクトを返す
    public GameObject GetObjectInDictionary()
    {
        // ダクト辞書からtrueになっているオブジェクトを探して返す
        foreach(var obj in ductDictionary)
        {
            // boolを取得
            bool InDuct = obj.Value;

            // キーを取得
            if (InDuct)
            {
                return obj.Key;
            }
        }

        return null;
    }


    // 辞書内に true の値が存在するかを確認するメソッド
    public bool ContainsTrueValue()
    {
        // 辞書の値のコレクションに true が含まれているかを確認
        return ductDictionary.ContainsValue(true);
    }

    public bool GetValue(GameObject _duct)
    {
        return ductDictionary[_duct];
    }

    public bool GetIsDuctIn()
    {
        return isDuctIn;
    }
}
