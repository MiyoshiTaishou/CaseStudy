using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーをサーチする処理
//Tagを自由に設定して追跡するオブジェクトを変えるとかもできるようにしたい

public class MPlayerSearch : MonoBehaviour
{
    [Header("視野角"), SerializeField]
    private float fEnemyAngle = 45.0f;

    [Header("視認中のマテリアル"), SerializeField]
    private Material MTSearch;

    [Header("移動速度"), SerializeField]
    private float fMoveSpeed;

    /// <summary>
    /// 元々のマテリアル
    /// </summary>
     private  Material MTDefault;

    /// <summary>
    /// 見つかっているか
    /// </summary>
    private bool isSearch = false;

    /// <summary>
    /// 視野範囲内を表示するレンダラ
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// 索敵用コライダー
    /// </summary>
    private CircleCollider2D ColSearch;

    /// <summary>
    /// ターゲットの座標
    /// </summary>
    private Transform TargetTransform;
   
    // Start is called before the first frame update
    void Start()
    {
        //元々のマテリアルを保存
        MTDefault = GetComponent<SpriteRenderer>().material; 
        
        lineRenderer = GetComponent<LineRenderer>();

        //頂点数設定
        lineRenderer.positionCount = 3;

        // 線の太さを設定
        lineRenderer.startWidth = 0.05f; // 線の始点の太さ
        lineRenderer.endWidth = 0.05f;   // 線の終点の太さ

        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();

        foreach(CircleCollider2D col in colliders)
        {
            if(col.isTrigger)
            {
                ColSearch = col;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isSearch);

        //見つかっている
        if(isSearch)
        {
            //見つかったら色を変える
            this.GetComponent<SpriteRenderer>().material = this.MTSearch;
            Chase();
        }
        else
        {            
            //見つかっていないならデフォルト
            this.GetComponent<SpriteRenderer>().material = this.MTDefault;
        }

        // 視野範囲を描画する
        DrawFieldOfView();
    }

    //視野角内に入ったかどうか検知
    private void OnTriggerStay2D(Collider2D _collision)
    {
        //視界の範囲の当たり判定
        if (_collision.gameObject.CompareTag("Player"))
        {            
            //視界の範囲内に収まっているか

            //自分とプレイヤーのベクトルを求める
            Vector2 vecPos = _collision.transform.position - this.transform.position;

            //前向きベクトルとvecPosの角度を求める
            float fPlayerAngle = Vector2.Angle(this.transform.right, vecPos);

            //fPlayerAngleが視野角内に収まっているか
            if (fPlayerAngle < fEnemyAngle * 0.5f)
            {               
                // 自身のコライダーの半径を取得
                float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

                //間に壁がないか
                RaycastHit2D RayHit = Physics2D.Raycast(transform.position + transform.right * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);
               
                if (RayHit.collider != null && RayHit.collider.CompareTag("Player"))
                {
                    Debug.Log("視認中");
                    isSearch = true;

                    //ターゲットの座標を保存
                    TargetTransform = _collision.transform;
                }
                else
                {
                    isSearch = false;
                }
            }
            else
            {
                isSearch = false;
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isSearch = false;
    }

    //視野範囲内を表示する
    void DrawFieldOfView()
    {
        Vector3[] vecPositions = new Vector3[3]; // ラインの頂点座標の配列

        // ラインの始点を設定（敵キャラクターの位置）
        vecPositions[0] = transform.position;

        // 視野範囲の端点を計算
        Vector3 vecEndPositionRight = transform.position + Quaternion.Euler(0, 0, fEnemyAngle * 0.5f) * transform.right * ColSearch.radius;
        Vector3 vecEndPositionLeft = transform.position + Quaternion.Euler(0, 0, -fEnemyAngle * 0.5f) * transform.right * ColSearch.radius;

        // ラインの端点を設定
        vecPositions[1] = vecEndPositionRight;
        vecPositions[2] = vecEndPositionLeft;

        lineRenderer.SetPositions(vecPositions);
        lineRenderer.loop = true; // 三角形を閉じる
    }

    //追跡処理（簡易版）
    void Chase()
    {
        if(TargetTransform != null)
        {
            //ターゲットに向かって移動するだけ
            Vector2 vecDir = (TargetTransform.position - transform.position).normalized;
            transform.Translate(vecDir * Time.deltaTime * fMoveSpeed);
        }
    }
}
