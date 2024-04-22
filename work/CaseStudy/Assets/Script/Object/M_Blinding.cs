using System.Collections;
using System.Collections.Generic;
using System.Numerics;
#if UNITY_EDITOR
using UnityEditor.U2D.Path;
#endif
using UnityEngine;

public class M_Blinding : MonoBehaviour
{
    [Header("光るまでの時間"),SerializeField]
    private float fDelay = 2.0f;

    [Header("消えるまでの時間"), SerializeField]
    private float fDeleteTime = 0.1f;

    /// <summary>
    /// 目くらまし起動しているか
    /// </summary>
    private bool isEnable = false;
   
    private SpriteRenderer spriteRenderer;   
    private CircleCollider2D[] colliders;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        

        colliders = GetComponents<CircleCollider2D>();
       
        // 一定時間後に当たり判定を有効にするコルーチンを開始
        StartCoroutine(EnableColliderAfterDelay());
    }

    IEnumerator EnableColliderAfterDelay()
    {
        // 光るまでの時間待機
        yield return new WaitForSeconds(fDelay);
        
        //光の判定をオン
        isEnable = true;

        // 消えるまでの時間待機
        yield return new WaitForSeconds(fDeleteTime);

        // オブジェクトを削除
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {        
        if (_collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(_collision.name);
            //自分とプレイヤーのベクトルを求める
            UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;
           
            //間に壁がないか
            RaycastHit2D RayHit = Physics2D.Raycast(transform.position , vecPos.normalized, vecPos.magnitude);
           
            if (RayHit.collider != null && RayHit.collider.CompareTag("Player"))
            {
                Debug.Log(RayHit.collider.name + "HIT");                
            }                    
        }


        ////エネミーの当たり判定
        //if(_collision.gameObject.CompareTag("Enemy"))
        //{            
        //    //自分とエネミーのベクトルを求める
        //    UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;

        //    // 自身のコライダーの半径を取得
        //    float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

        //    //間に壁がないか
        //    RaycastHit2D RayHit = Physics2D.Raycast(transform.position, vecPos.normalized, vecPos.magnitude);
           
        //    if (RayHit.collider == null)
        //    {
        //        Debug.Log("エネミーヒット");

        //        //エネミーの目くらまし変数をtrueにする
        //        _collision.gameObject.GetComponent<M_BlindingMove>().SetIsBlinding(true);

        //        //エネミーから自身の向きを取得
        //        UnityEngine.Vector2 vecDir = this.transform.position - _collision.transform.position;
        //        vecDir.Normalize();

        //        //向きを設定する                
        //        _collision.gameObject.GetComponent<M_BlindingMove>().SetVecDirBlinding(vecDir);
        //    }
        //}
    }

    public bool GetIsEnable()
    {
        return isEnable;
    }
}
