using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_Respawn : MonoBehaviour
{
    [Header("復活できる回数"), SerializeField]
    int nRespawn = 0;

    [Header("ヒットストップ"), SerializeField]
    int nHitStop = 0;

    [Header("復活時SE"),SerializeField]
    AudioClip acRespawn= null;
    [Header("敵接触時SE"),SerializeField]
    AudioClip acHit= null;
    [Header("チェックポイント取得時SE"),SerializeField]
    AudioClip acCheckPoint= null;

    //コルーチン中かどうか
    bool isCoroutine = false;

    //復活位置
    Vector2 vecRespawnPos = Vector2.zero;

    //ゲッターセッターここから
    public
    Vector2 GetRespawnPos() { return vecRespawnPos; }
    public
    void SetReapawnPos(Vector2 _pos) { vecRespawnPos = _pos; }
    //ここまで

    AudioSource audiosource=null;

    // Start is called before the first frame update
    void Start()
    {
        vecRespawnPos=transform.position;
        audiosource=GetComponent<AudioSource>();
        if(!audiosource)
        {
            Debug.LogError("プレイヤーのスポーンのやつにAudioSourceがないって話だよね");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //当たったのが敵なら復活
        if (collision.transform.CompareTag("Enemy") &&  //タグチェック
            isCoroutine == false)                       //コルーチン中かチェック
        {
            StartCoroutine(HitStop());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たったのがチェックポイントなら復活位置更新
        if (collision.transform.CompareTag("Respawn"))
        {
            audiosource.PlayOneShot(acCheckPoint);
            SetReapawnPos(collision.transform.position);
            Destroy(collision.gameObject);
        }
    }

    //ヒットストップのコルーチン
    IEnumerator HitStop()
    {
        isCoroutine= true;

        //敵接触時音声再生
        audiosource.PlayOneShot(acHit);

        //Positionに関するコンポーネントのオンオフ
        transform.root.GetComponent<M_PlayerMove>().enabled = false;
        transform.root.GetComponent<BoxCollider2D>().enabled = false;
        
        //指定のフレーム待つ
        yield return new WaitForSeconds(nHitStop / 60);

        transform.root.GetComponent<BoxCollider2D>().enabled = true;
        transform.root.GetComponent<M_PlayerMove>().enabled = true;

        //復活orでっど
        if (nRespawn > 0)
        {
            //復活位置に転送
            transform.root.position = vecRespawnPos;

            M_GameMaster.SetDethCount(1);
        }
        else if(nRespawn <= 0)
        {
            //ゲームオーバーのフラグをオンにするとかの処理が入るのかもしれないよねって話だよね
            //デストロイ
            Destroy(transform.root.gameObject);
        }
        nRespawn--;

       //復活時音声再生
       audiosource.PlayOneShot(acRespawn);

       isCoroutine= false;
    }
}
