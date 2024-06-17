using System.Collections;
using UnityEngine;

public class SEnemyMove : MonoBehaviour
{
    [Header("メインカメラ"),SerializeField]
    Camera mainCamera= null;

    //[Header("何マス分移動するか"), SerializeField]
    //Vector2 MoveDistance= Vector2.zero;

    //[Header("目標位置まで到達して何秒待機するか"), SerializeField]
    //float fWaitTime = 0.0f;

    //[Header("敵衝突時の停止時間"), SerializeField]
    //float fFreezeTime = 0.0f;

    //反対方向を向いているか
    private bool IsReflectionX = false;

    //private bool IsReflectionY = false;

    //bool GetReflectionX() { return IsReflectionX; }
    //bool GetReflectionY() { return IsReflectionY; }

    //初期位置
    //private Vector2 defaultPos= Vector2.zero;

    private Rigidbody2D rb=null;

    //目標位置
    //private Vector2 GallPos=Vector2.zero;

    //壁や崖を判断するRay
    private Ray rayWall,rayGround;
    [Header("Rayの長さ(壁用)"), SerializeField]
    float fDistance=0.5f;
    [Header("Rayの長さ(足元)"), SerializeField]
    float fGroundDistance=1.0f;
    [Header("Rayの始点をどれだけ離すか(x)"), SerializeField]
    float fGup = 0.0f;
    [Header("足元のRayの方向"), SerializeField]
    float fGrounddirx = 0.2f, fGrounddiry = -0.8f;
    [Header("ピタッと止まる"), SerializeField]
    bool isStop = false;
    [Header("Rayの可視化"), SerializeField]
    bool isRayDraw = false;
    [Header("坂道抵抗力推奨20"), SerializeField]
    float Power = 20.0f;
    [Header("制限速度"), SerializeField]
    float fLimitSpeed = 2.0f;

    //Rayの始点
    private Vector2 Origin=Vector2.zero;

    //足元に飛ばすRayの方向
    private Vector2 GroundDirection=Vector2.zero;

    //以下、坂道計算用の変数
    private Vector2 slopeOrigin1=Vector2.zero;
    private Vector2 slopeOrigin2=Vector2.zero;
    private Vector2 slopePos1= Vector2.zero;
    private Vector2 slopePos2= Vector2.zero;
    Vector2 slopeGup1=Vector2.zero;
    private Vector2 slopeGup2=Vector2.zero;

    private float frictionCoefficient = 0.5f; // 摩擦係数
    //坂道上にいるか
    private bool isSlope = false;

    //地に足ついてるか
    private bool isGround = false;

    private bool isLook = false;

    //ホログラムとの衝突を検知(2024/4/17 木村記載)
    private bool IsCollidingHologram = false;

    // 隊列内での番号
    public int TeamNumber = 0;

    private N_EnemyManager enemyManager;

    private Transform thisTrans;

    [Header("壁レイヤーマスク設定"), SerializeField]
    private LayerMask layerMask_Wall;

    [Header("床レイヤーマスク設定"), SerializeField]
    private LayerMask layerMask_Ground;

    [Header("当たり判定コライダー"), SerializeField]
    private N_GroundCheck sc_GroundCheck;

    private bool isGroundOld = false;
    private bool isGroundNow = false;

    private Animator animator;

    public void StartMove() { isLook = true; }

    public N_EnemyManager GetManager()
    {
        return enemyManager;
    }

    //ワープしたかどうか
    private bool isWarped = false;
    public bool OldisWarped = false;
    Coroutine coroutine = null;
    //セッターとゲッター
    public void SetisWarped(bool _flg) { isWarped = _flg; }
    public bool GetIsWarped() { return isWarped; }

    S_LoopWall Warp;
    public void SetWarp(S_LoopWall wall) { Warp = wall; }
    public S_LoopWall GetWarp() { return Warp; }

    public bool GetIsCollidingHologram() { return IsCollidingHologram; }

    // Start is called before the first frame update
    private Vector2 defaultScale= Vector2.zero;
    void Start()
    {
        //defaultPos = transform.position;
        //GallPos = defaultPos + MoveDistance;
        rb= GetComponent<Rigidbody2D>();
        if(!rb)
        {
            Debug.LogError("RigidBody2Dがありません");
        }
        slopeGup1.x = 0.2f;
        slopeGup2 = slopeGup1;
        slopeGup2.x *= -1;
        if (!mainCamera)
        {
            Debug.LogError("Main camera がありません");
        }
        defaultScale = transform.localScale;

        enemyManager = this.transform.parent.gameObject.GetComponent<N_EnemyManager>();
        thisTrans = this.GetComponent<Transform>();

        animator = transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!M_GameMaster.GetGamePlay())
        {
            return;
        }
        //ワープ状態になっていたら一定時間後に解除
        if(isWarped && OldisWarped)
        {
            OldisWarped = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }   
            coroutine = StartCoroutine(ChangeWarped());
        }
        //if(this.GetComponent<MPlayerSearch>().GetIsSearch())
        //{
        //    return;
        //}
        if (!isLook)
        {
            // オブジェクトの境界ボックスを取得
            Bounds bounds = GetComponent<Renderer>().bounds;

            // オブジェクトの境界ボックスがカメラの視錐台内にあるかどうかを判定
            bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), bounds);
            if (isVisible) 
            {
                //isLook= true;
                enemyManager.IsLook(true);
            }
            else 
            {
                return;
            }
        }

        // 今フレームの接地判定を取得
        isGroundNow = sc_GroundCheck.GroundCheck();

        ////坂道計算
        ////傾きを計算するためのポジションを取得
        //slopeOrigin1 = transform.position;
        //slopeOrigin1.x += slopeGup1.x;
        //slopeOrigin1.y += slopeGup1.y;
        //slopeOrigin2 = transform.position;
        //slopeOrigin2.x += slopeGup2.x;
        //slopeOrigin2.y += slopeGup2.y;

        //RaycastHit2D hitSlope1 = Physics2D.Raycast(slopeOrigin1, Vector2.down, 2.0f);
        //RaycastHit2D hitSlope2 = Physics2D.Raycast(slopeOrigin2, Vector2.down, 2.0f);
        //if (hitSlope1.collider != null && hitSlope2.collider != null &&
        //    hitSlope1.collider.CompareTag("TileMap") && hitSlope2.collider.CompareTag("TileMap"))
        //{
        //    //2点間の傾きを計算
        //    Vector2 point1 = hitSlope1.point;
        //    Vector2 point2 = hitSlope2.point;
        //    float slopeAngle = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
        //    int isRight = IsReflectionX ? -1 : 1;

        //    if (slopeAngle < 170 && slopeAngle > 10)
        //    {
        //        Vector2 vel = rb.velocity;
        //        vel.x = fLimitSpeed * isRight;
        //        rb.velocity = vel;
        //        isSlope = true;
        //    }
        //    else
        //    {
        //        rb.drag = 1;
        //        isSlope = false;
        //    }
        //    //傾斜の角度が一定以上なら傾いていると判断

        //    //傾きに対する抵抗力を計算
        //    //傾斜に対する水平方向の抗力を計算
        //    float horizontalResistance = rb.mass * Mathf.Abs(rb.gravityScale) * frictionCoefficient * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
        //    Vector2 vecAngle = new Vector2(Mathf.Cos(slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeAngle * Mathf.Deg2Rad));

        //    Vector2 resistanceForce = vecAngle * horizontalResistance;
        //    if (IsReflectionX)
        //    {
        //        rb.AddForce(resistanceForce * Power * 20, ForceMode2D.Force);
        //    }
        //    else
        //    {
        //        rb.AddForce(resistanceForce * (Power / 2), ForceMode2D.Force);
        //    }
        //}


        //この辺コードくっそ汚いので気が向いたら直しておきます
        int coef=IsReflectionX? -1 : 1;
        Origin = transform.position;
        Origin.x += fGup * coef;
        GroundDirection.x=fGrounddirx*coef;
        GroundDirection.y=fGrounddiry;
        Vector2 scale= transform.localScale;
        scale.x = defaultScale.x * coef;
        transform.localScale = scale;
        RaycastHit2D hitWall = Physics2D.Raycast(Origin, Vector2.right*coef, fDistance, layerMask_Wall);
        RaycastHit2D hitGround = Physics2D.Raycast(Origin, GroundDirection, fGroundDistance,layerMask_Ground);
        if (isRayDraw)
        {
            Debug.DrawRay(Origin, Vector2.right * coef*fDistance, Color.red, 0.0f, false);
            Debug.DrawRay(Origin, GroundDirection * fGroundDistance, Color.blue, 0.1f, false);
            //Debug.DrawRay(slopeOrigin1, Vector2.down * 2.0f, Color.blue, 1.0f, false);
            //Debug.DrawRay(slopeOrigin2, Vector2.down * 2.0f, Color.green, 1.0f, false);
        }

        //壁判定用のRayがタイルマップに接触しているか、足元のRayが何も情報を得られなければ方向を切り替える
        if (sc_GroundCheck.GroundCheck()&&
            hitWall.collider!=null&&hitWall.collider.CompareTag("TileMap")||
            hitWall.collider!=null&&hitWall.collider.CompareTag("Hologram")||
            hitWall.collider!=null&&hitWall.collider.CompareTag("Ground")||
            hitGround.collider==null)
        {
            //ホログラムとの衝突を検知(2024/4/17 木村記載)
            if (hitWall.collider != null && hitWall.collider.CompareTag("Hologram"))
            {
                if (enemyManager != null)
                {
                    //Debug.Log("ホログラム検知");
                    IsCollidingHologram = true;
                    enemyManager.DetectionHologram(TeamNumber);
                }
            }
            else
            {
                IsCollidingHologram = false;
            }
        }
        else
        {
            IsCollidingHologram = false; ;
        }

        if (sc_GroundCheck.GroundCheck() &&
            hitWall.collider != null && hitWall.collider.CompareTag("TileMap") ||
            hitWall.collider != null && hitWall.collider.CompareTag("Ground") ||
            hitWall.collider != null && hitWall.collider.CompareTag("FieldObj") ||
            hitGround.collider == null)
        {
            if (enemyManager != null)
            {
                //Debug.Log("ホログラム以外検知");
                enemyManager.RequestRefletion();
            }
        }

        // 地面から離れたら
        if(isGroundNow == false && Time.time >= 0.5f)
        {
            animator.SetBool("fall", true);
        }

        // 空中から地面に接地したなら
        if(isGroundNow == true && isGroundOld == false && Time.time >= 0.5f)
        {
            // 隊列内でのy座標がバラバラになっている可能性があるので
            // y座標が近いものどおしの隊列に組みなおし

            animator.SetBool("fall", false);

            if (enemyManager != null)
            {
                Debug.Log("隊列組みなおし");
                enemyManager.PartitionTeamHeight();
            }
        }

        // 今フレームの接地判定を次フレームに持ち込み
        isGroundOld = isGroundNow;
    }

    public bool GetIsReflection()
    {
        return IsReflectionX;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if(_collision.transform.GetComponent<S_LoopWall>())
        {
            enemyManager.PartitionTeamHeight();
        }
        if (_collision.transform.CompareTag("Enemy"))
        {
            SEnemyMove enemyMove = _collision.gameObject.GetComponent<SEnemyMove>();
            N_EnemyManager colManager = enemyMove.GetManager();

            // どちらかがマネージャーを持っていないなら
            if (!colManager || !this.enemyManager)
            {
                return;
            }

            // 違う隊列に所属している敵同士なら
            if (this.enemyManager.name != colManager.name)
            {
                // 進行方向が違うなら
                if (IsReflectionX != enemyMove.GetIsReflection())
                {
                    // 方向転換
                    //enemyManager.RequestRefletion();

                    // 生成順が速い方に合体
                    if(this.enemyManager.GetGenerateNumber() < colManager.GetGenerateNumber())
                    {
                        // 合体
                        enemyManager.UnionTeam(colManager);
                        //Debug.Log("合体");

                    }
                }
                // 同じ方向に進んでいるなら
                else
                {
                    // こっちの分岐に入る = どちらかのスピードがおおきい
                    // 大きい方が小さい方のケツからぶつかっているはずなので
                    // スピードが大きい奴だけ方向転換
                    if(this.enemyManager.GetMoveSpeed() > colManager.GetMoveSpeed())
                    {
                        // 方向転換
                        //enemyManager.RequestRefletion();

                        // 合体
                        enemyManager.UnionTeam(colManager);
                        //Debug.Log("合体");

                    }
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D _collision)
    {
        if (_collision.transform.CompareTag("Enemy"))
        {
            SEnemyMove enemyMove = _collision.gameObject.GetComponent<SEnemyMove>();
            N_EnemyManager colManager = enemyMove.GetManager();

            // どちらかがマネージャーを持っていないなら
            if(!colManager || !this.enemyManager)
            {
                return;
            }

            // 違う隊列に所属している敵同士なら
            if (this.enemyManager.name != colManager.name)
            {
                // 進行方向が違うなら
                if (IsReflectionX != enemyMove.GetIsReflection())
                {
                    // 方向転換
                    //enemyManager.RequestRefletion();

                    // 生成順が速い方に合体
                    if (this.enemyManager.GetGenerateNumber() < colManager.GetGenerateNumber())
                    {
                        // 合体
                        enemyManager.UnionTeam(colManager);
                        //Debug.Log("合体");

                    }
                }
                // 同じ方向に進んでいるなら
                else
                {
                    // こっちの分岐に入る = どちらかのスピードがおおきい
                    // 大きい方が小さい方のケツからぶつかっているはずなので
                    if (this.enemyManager.GetMoveSpeed() >= colManager.GetMoveSpeed())
                    {
                        // 方向転換
                        //enemyManager.RequestRefletion();

                        // 合体
                        enemyManager.UnionTeam(colManager);
                        //Debug.Log("合体");

                    }
                }
            }
        }
    }

    //コルーチンで待機処理
    //IEnumerator Gall(float _wait)
    //{
    //    //終わるまで待ってほしい処理を書く
    //    this.enabled = false;
    //    if (isStop)
    //    {
    //        Vector2 vel = rb.velocity;
    //        vel.x = 0;
    //        vel.y = 0;
    //        rb.velocity = vel;
    //        if (isSlope)
    //        {
    //            rb.isKinematic = true;
    //        }
    //    }
    //    //指定の秒数待つ
    //    yield return new WaitForSeconds(_wait);
    //    //再開してから実行したい処理を書く
    //    this.enabled = true;
    //    if(isSlope)
    //    {
    //        rb.isKinematic = false;
    //    }
    //}
    IEnumerator ChangeWarped()
    {
        isWarped = true;
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(1);
        isWarped = false;
    }
    private bool GroundCheck()
    {
        Vector2 origin = transform.position;
        origin.y -= 1.2f;
        RaycastHit2D hit=Physics2D.Raycast(origin, Vector2.down,0.4f);
        //Debug.Log(hit.collider);
        //Debug.DrawRay(origin, Vector2.down * 0.4f, Color.yellow);
        if (hit.collider != null && (hit.collider.CompareTag("TileMap") || hit.collider.CompareTag("Hologram"))) 
        {
            //ホロ床すり抜けなどで位置が大きく変わった場合に目標位置等の更新を行う
            //if(!isGround) 
            //{
            //    if (IsReflectionX)
            //    {
            //        GallPos.x = defaultPos.x - MoveDistance.x;
            //        StartCoroutine(Gall(fWaitTime));
            //    }
            //    else if (!IsReflectionX)
            //    {
            //        GallPos.x = defaultPos.x + MoveDistance.x;
            //        StartCoroutine(Gall(fWaitTime));
            //    }
            //    defaultPos = transform.position;
            //}
          isGround= true;
            //Debug.Log("地面当たり");
        }
        else
        {
            //Debug.Log("地面はずれ");

            isGround = false;
        }
        return isGround;
    }

    public void SetNumber(int _num)
    {
        TeamNumber = _num;
    }

    public void SetEnemyManager()
    {
        enemyManager = this.transform.parent.gameObject.GetComponent<N_EnemyManager>();
    }

    public void NullEnemyManager()
    {
        enemyManager = null;
    }

    public void EnemyMove(float _move, bool _ref)
    {
        if(GetComponent<S_EnemyBall>().GetisPushing())
        {
            return;
        }
        thisTrans.Translate(_move, 0.0f, 0.0f, Space.Self);
        IsReflectionX = _ref;
        rb.velocity = Vector2.zero;
    }

    public void ChaseTarget(float _move)
    {
        thisTrans.Translate(_move, 0.0f, 0.0f, Space.Self);
        rb.velocity = Vector2.zero;
    }

    public int GetTeamNumber()
    {
        return TeamNumber;
    }
}
