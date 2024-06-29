using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch : MonoBehaviour
{
    public N_EnemyManager enemyManager;
    private SEnemyMove enemyMove;
    private S_EnemyBall enemyBall;

    private GameObject Parent;

    [Header("見失う距離(コライダーのサイズ以上)"), SerializeField]
    private float LostSightDistance = 6.0f;

    private Transform transTarget;
    private GameObject Target;

    [SerializeField]
    public bool isSearch = false;

    public bool isRaycast = false;

    public bool isCheck = false;

    public float elapsedTime = 0.0f;

    private bool init = false;

    [Header("レイヤーマスク設定"), SerializeField]
    private LayerMask layerMask;

    public bool GetIsSearch() { return isSearch; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            // 親オブジェクト取得
            Parent = transform.parent.gameObject;
            enemyMove = Parent.GetComponent<SEnemyMove>();
            enemyBall = Parent.GetComponent<S_EnemyBall>();
            enemyManager = enemyMove.GetManager();

            init = true;
        }

        //玉状態なら追跡状態を解除
        if (enemyBall.GetisBall())
        {
            isSearch = false;
            isRaycast = false;
            elapsedTime = 0.0f;
            enemyManager = null;
        }
        else
        {
            if (enemyManager == null)
            {
                enemyManager = enemyMove.GetManager();
            }

            // 追跡対象が視野範囲内に入った時
            // 壁を挟んでいるかを判定する
            if (isRaycast /*&& !isCheck*/)
            {
                RayCastCheck();
            }

            // 見つけているときのみ見失うための計算実行
            if (isSearch)
            {
                // ターゲットとの距離を計算
                Vector2 vec = transform.position - transTarget.position;

                float dis = vec.x * vec.x + vec.y * vec.y;

                if (dis > LostSightDistance * LostSightDistance)
                {
                    isSearch = false;
                    isRaycast = false;
                    elapsedTime = 0.0f;
                    Debug.Log("gomi");
                }

                if(Target.GetComponent<BoxCollider2D>().enabled == false)
                {
                    isSearch = false;
                    isRaycast = false;
                    elapsedTime = 0.0f;
                    isCheck = false;
                    Debug.Log("kuso");

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRaycast || isSearch)
        {
            return;
        }
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                Target = collision.gameObject;
                transTarget = Target.transform;

                //Debug.Log("エンター");

                isRaycast = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (isRaycast || isSearch)
        {
            return;
        }
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                Target = collision.gameObject;
                transTarget = Target.transform;

                //Debug.Log("エンター");

                isRaycast = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                isRaycast = false;
                elapsedTime = 0.0f;

                isCheck = false;
            }
        }
    }

    // プレイヤーが視界に入った時にその間に壁があるかどうか判断するレイを飛ばす
    private void RayCastCheck()
    {
        //Debug.Log("例とバス");

        Vector3 startPoint = gameObject.transform.position;
        Vector2 direction = Vector2.right;
        if (enemyMove.GetIsReflection())
        {
            direction = Vector2.left;
        }
        float distance = elapsedTime * 30.0f;
        elapsedTime += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction,distance,layerMask);

        Debug.DrawRay(startPoint, direction * distance, Color.black, 0.0f, false);

        // 先に敵に当たったら追跡
        // 壁に当たったらなにもなし
        if (hit.collider != null)
        {
            Debug.Log("ヒットした");
            elapsedTime = 0.0f;
            isCheck = true;

            if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Decoy"))
            {
                Debug.Log("接敵");
                isSearch = true;
                enemyManager.SetTarget(Target);
                //isRaycast = false;
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    S_Respawn.SetIsFounded(true);
                    //Debug.Log("みつかった");
                }
                else
                {
                    S_Respawn.SetIsFounded(false);
                    //Debug.Log("みつかってない");
                }
            }
            else if(hit.collider.gameObject.CompareTag("Ground"))
            {
                Debug.Log("壁検知");

                isSearch = false;
                //isRaycast = false;
                S_Respawn.SetIsFounded(false);
                //Debug.Log("みつかってない");
            }
            else
            {
                Debug.Log("なんか違うもの");
            }
        }
        else
        {
            Debug.Log("ヒットなし");
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Decoy"))
    //    {
    //        //enemyManager.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);
    //        isSearch = false;
    //        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!");
    //    }
    //}

    //private void OnDestroy()
    //{
    //    //Debug.Log("やられた！");
    //    isSearch = false;
    //}
}
