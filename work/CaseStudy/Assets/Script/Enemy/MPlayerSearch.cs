using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    [Header("視野範囲内用レイの数（多いほどキレイに描画）"), SerializeField]
    private int nNumRays = 3;

    [Header("視野範囲の広さ（何故かコライダーのサイズと対応できなかったのでこちらでコライダーのサイズに合わせてほしい）"), SerializeField]
    private float fMaxDistance = 5.0f;

    /// <summary>
    /// 元々のマテリアル
    /// </summary>
     private  Material MTDefault;

    /// <summary>
    /// 見つかっているか
    /// </summary>
    private bool isSearch = false;

    public bool GetIsSearch(){ return isSearch; }

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

    /// <summary>
    /// どちらを向いているか
    /// </summary>
    private Vector3 isRight;

    private Rigidbody2D rbEnemy;
   
    // Start is called before the first frame update
    void Start()
    {
        //元々のマテリアルを保存
        MTDefault = GetComponent<SpriteRenderer>().material; 
        
        lineRenderer = GetComponent<LineRenderer>();       

        // 線の太さを設定
        lineRenderer.startWidth = 0.05f; // 線の始点の太さ
        lineRenderer.endWidth = 0.05f;   // 線の終点の太さ

        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();

        rbEnemy = GetComponent<Rigidbody2D>();

        //向きを取得
        isRight = transform.right;

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

        //velocityからどちらの方向を向いているか判断
        if(rbEnemy.velocity.x < 0.0f && !isSearch)
        {            
            isRight = transform.right * -1;
        }
        else if(rbEnemy.velocity.x > 0.0f && !isSearch)
        {          
            isRight = transform.right;
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
            float fPlayerAngle = Vector2.Angle(isRight, vecPos);

            //fPlayerAngleが視野角内に収まっているか
            if (fPlayerAngle < fEnemyAngle * 0.5f)
            {               
                // 自身のコライダーの半径を取得
                float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

                //間に壁がないか
                RaycastHit2D RayHit = Physics2D.Raycast(transform.position + isRight * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);

                Debug.DrawRay(transform.position + isRight * (selfColliderRadius + 0.1f), vecPos.normalized * vecPos.magnitude, Color.red);


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

        if (_collision.gameObject.CompareTag("Blinding"))
        {
            //当たり判定がオンなら
            if (_collision.gameObject.GetComponent<M_Blinding>().GetIsEnable())
            {
                //視界の範囲内に収まっているか

                //自分と目くらましのベクトルを求める
                Vector2 vecPos = _collision.transform.position - this.transform.position;

                //前向きベクトルとvecPosの角度を求める
                float fPlayerAngle = Vector2.Angle(isRight, vecPos);

                //fPlayerAngleが視野角内に収まっているか
                if (fPlayerAngle < fEnemyAngle * 0.5f)
                {
                    // 自身のコライダーの半径を取得
                    float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

                    //間に壁がないか
                    RaycastHit2D RayHit = Physics2D.Raycast(transform.position + isRight * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);

                    if (RayHit.collider != null && RayHit.collider.CompareTag("Blinding"))
                    {
                        Debug.Log("フラッシュ");

                        //エネミーの目くらまし変数をtrueにする
                        GetComponent<M_BlindingMove>().SetIsBlinding(true);

                        //エネミーから目くらましの向きを取得
                        UnityEngine.Vector2 vecDir = _collision.transform.position - this.transform.position;
                        vecDir.Normalize();

                        //向きを設定する                
                        GetComponent<M_BlindingMove>().SetVecDirBlinding(vecDir);
                    }
                }
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
        // ラインの頂点座標のリスト
        List<Vector3> vecPositions = new List<Vector3>();

        //ラインの始点
        vecPositions.Add(transform.position);

        //レイの角度間隔
        float fStepAngleSize = fEnemyAngle / nNumRays;

        for (int i = 0; i <= nNumRays; i++)
        {
            // レイの角度を計算
            float angle = transform.eulerAngles.z - fEnemyAngle / 2 + fStepAngleSize * i;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * isRight; // レイの方向を計算            

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dir, fMaxDistance);

            if (rayHit.collider != null)
            {
                // 障害物に当たった場合、障害物までの距離までの点を追加
                vecPositions.Add(rayHit.point);
            }
            else
            {
                // 障害物に当たらなかった場合、視野の端までの点を追加
                // コライダーの半径を視野範囲の距離とする
                vecPositions.Add(transform.position + dir * fMaxDistance);
            }
        }

        //最後に視野範囲内の形状に閉じる
        vecPositions.Add(transform.position);

        lineRenderer.positionCount = vecPositions.Count;
        lineRenderer.SetPositions(vecPositions.ToArray());

        //範囲内を塗りつぶす       
        DrawFieldFill(vecPositions);
    }


    //範囲内を塗りつぶす処理
    void DrawFieldFill(List<Vector3> vertices)
    {
        if (vertices.Count < 3) return; // 頂点数が3未満の場合は描画しない
        
        // メッシュを作成
        Mesh mesh = new Mesh();

        // 2Dの頂点座標に変換
        Vector3[] vecVertices = new Vector3[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            vecVertices[i] = new Vector3(vertices[i].x, vertices[i].y, 0);
        }

        // メッシュに頂点を設定
        mesh.vertices = vecVertices;

        // 頂点インデックスを生成して設定する
        int[] triangles = new int[(vertices.Count - 2) * 3];
        for (int i = 0, count = 0; i < vertices.Count - 2; i++, count += 3)
        {
            triangles[count] = 0;
            triangles[count + 1] = i + 2;
            triangles[count + 2] = i + 1;
        }

        // メッシュに三角形のインデックスを設定
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        // メッシュを描画
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, MTDefault, 0);
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
