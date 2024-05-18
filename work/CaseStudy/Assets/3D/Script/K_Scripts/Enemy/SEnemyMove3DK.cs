using System.Collections;
using UnityEngine;

public class SEnemyMove3DK : MonoBehaviour
{
    [Header("メインカメラ"),SerializeField]
    Camera mainCamera= null;


    //反対方向を向いているか
    private bool IsReflectionX = false;

    private Rigidbody rb=null;

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
    private Vector3 Origin=Vector3.zero;

    //足元に飛ばすRayの方向
    private Vector3 GroundDirection=Vector3.zero;

    //以下、坂道計算用の変数
    private Vector3 slopeOrigin1=Vector3.zero;
    private Vector3 slopeOrigin2=Vector3.zero;
    private Vector3 slopePos1= Vector3.zero;
    private Vector3 slopePos2= Vector3.zero;
    Vector3 slopeGup1=Vector2.zero;
    private Vector3 slopeGup2=Vector3.zero;

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

    private N_EnemyManager3DK enemyManager;

    private Transform thisTrans;

    public void StartMove() { isLook = true; }

    public N_EnemyManager3DK GetManager()
    {
        return enemyManager;
    }

    public bool GetIsCollidingHologram() { return IsCollidingHologram; }

    // Start is called before the first frame update
    private Vector3 defaultScale= Vector3.zero;

    void Start()
    {
        //defaultPos = transform.position;
        //GallPos = defaultPos + MoveDistance;
        rb= GetComponent<Rigidbody>();
        if(!rb)
        {
            Debug.LogError("RigidBodyがありません");
        }
        slopeGup1.x = 0.2f;
        slopeGup2 = slopeGup1;
        slopeGup2.x *= -1;
        if (!mainCamera)
        {
            Debug.LogError("Main camera がありません");
        }
        defaultScale = transform.localScale;

        enemyManager = this.transform.parent.gameObject.GetComponent<N_EnemyManager3DK>();
        thisTrans = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLook)
        {
            // オブジェクトの境界ボックスを取得
            //Bounds bounds = GetComponent<Renderer>().bounds;
            Bounds bounds = GetComponent<BoxCollider>().bounds;

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
        //坂道計算
        //傾きを計算するためのポジションを取得
        slopeOrigin1 = transform.position;
        slopeOrigin1.x += slopeGup1.x;
        slopeOrigin1.y += slopeGup1.y;
        slopeOrigin2 = transform.position;
        slopeOrigin2.x += slopeGup2.x;
        slopeOrigin2.y += slopeGup2.y;

        RaycastHit2D hitSlope1 = Physics2D.Raycast(slopeOrigin1, Vector3.down, 2.0f);
        RaycastHit2D hitSlope2 = Physics2D.Raycast(slopeOrigin2, Vector3.down, 2.0f);
        if (hitSlope1.collider != null && hitSlope2.collider != null &&
            hitSlope1.collider.CompareTag("TileMap") && hitSlope2.collider.CompareTag("TileMap"))
        {
            //2点間の傾きを計算
            Vector3 point1 = hitSlope1.point;
            Vector3 point2 = hitSlope2.point;
            float slopeAngle = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
            int isRight = IsReflectionX ? -1 : 1;

            if (slopeAngle < 170 && slopeAngle > 10)
            {
                Vector3 vel = rb.velocity;
                vel.x = fLimitSpeed * isRight;
                rb.velocity = vel;
                isSlope = true;
            }
            else
            {
                rb.drag = 1;
                isSlope = false;
            }
            //傾斜の角度が一定以上なら傾いていると判断

            //傾きに対する抵抗力を計算
            //傾斜に対する水平方向の抗力を計算
            float horizontalResistance = rb.mass * Mathf.Abs(rb.mass) * frictionCoefficient * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
            Vector3 vecAngle = new Vector3(Mathf.Cos(slopeAngle * Mathf.Deg2Rad), Mathf.Sin(slopeAngle * Mathf.Deg2Rad));

            Vector3 resistanceForce = vecAngle * horizontalResistance;
            if (IsReflectionX)
            {
                rb.AddForce(resistanceForce * Power * 20, ForceMode.Force);
            }
            else
            {
                rb.AddForce(resistanceForce * (Power / 2), ForceMode.Force);
            }
        }


        //この辺コードくっそ汚いので気が向いたら直しておきます
        int coef=IsReflectionX? -1 : 1;
        Origin = transform.position;
        Origin.x += fGup * coef;
        GroundDirection.x=fGrounddirx*coef;
        GroundDirection.y=fGrounddiry;
        Vector3 scale= transform.localScale;
        scale.x = defaultScale.x * coef;
        transform.localScale = scale;
        //RaycastHit2D hitWall = Physics2D.Raycast(Origin, Vector3.right*coef, fDistance);
        Ray HitWall = new Ray(Origin, Vector3.right * coef); // Rayを生成
        RaycastHit hitWallInfo;
        Physics.Raycast(HitWall,out hitWallInfo, fDistance);
        //RaycastHit2D hitGround = Physics2D.Raycast(Origin, GroundDirection, fGroundDistance);
        Ray HitGround = new Ray(Origin, GroundDirection); // Rayを生成
        RaycastHit hitGroundInfo;
        Physics.Raycast(HitGround, out hitGroundInfo, fDistance);
        if (isRayDraw)
        {
            //Debug.DrawRay(Origin, Vector3.right * coef*fDistance, Color.red, 0.0f, false);
            Debug.DrawRay(HitWall.origin, HitWall.direction * fDistance, Color.red, 0.0f); // 長さ３０、赤色で５秒間可視化
            Debug.DrawRay(HitGround.origin, HitGround.direction * fGroundDistance, Color.red, 0.0f); // 長さ３０、赤色で５秒間可視化
        }

        //壁判定用のRayがタイルマップに接触しているか、足元のRayが何も情報を得られなければ方向を切り替える
        if (GroundCheck()&&
            hitWallInfo.collider!=null&& hitWallInfo.collider.CompareTag("TileMap")||
            hitWallInfo.collider!=null&& hitWallInfo.collider.CompareTag("Hologram")||
            hitWallInfo.collider!=null&& hitWallInfo.collider.CompareTag("Ground")||
            hitGroundInfo.collider==null)
        {
            //ホログラムとの衝突を検知(2024/4/17 木村記載)
            if (hitWallInfo.collider != null && hitWallInfo.collider.CompareTag("Hologram"))
            {
                //Debug.Log("ホログラム検知");
                IsCollidingHologram = true;
                enemyManager.DetectionHologram(TeamNumber);
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

        if (GroundCheck() &&
            hitWallInfo.collider != null && hitWallInfo.collider.CompareTag("TileMap") ||
            hitWallInfo.collider != null && hitWallInfo.collider.CompareTag("Ground") ||
            hitWallInfo.collider != null && hitWallInfo.collider.CompareTag("FieldObj") ||
            hitGroundInfo.collider == null)
        {
            //Debug.Log("ホログラム以外検知");
            enemyManager.RequestRefletion();
        }
    }

    public bool GetIsReflection()
    {
        return IsReflectionX;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.CompareTag("Enemy"))
        {
            SEnemyMove3DK enemyMove = _collision.gameObject.GetComponent<SEnemyMove3DK>();
            N_EnemyManager3DK colManager = enemyMove.GetManager();

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
    private void OnCollisionStay(Collision _collision)
    {
        if (_collision.transform.CompareTag("Enemy"))
        {
            SEnemyMove3DK enemyMove = _collision.gameObject.GetComponent<SEnemyMove3DK>();
            N_EnemyManager3DK colManager = enemyMove.GetManager();

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
                    // スピードが大きい奴だけ方向転換
                    if (this.enemyManager.GetMoveSpeed() > colManager.GetMoveSpeed())
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

    private bool GroundCheck()
    {
        Vector2 origin = transform.position;
        origin.y -= 1.2f;
        RaycastHit2D hit=Physics2D.Raycast(origin, Vector2.down,0.4f);
        //Debug.DrawRay(origin,Vector2.down*0.2f, Color.yellow);
        if(hit.collider != null && hit.collider.CompareTag("TileMap")) 
        {
          isGround= true;
        }
        else
        {
            isGround= false;
        }
        return isGround;
    }

    public void SetNumber(int _num)
    {
        TeamNumber = _num;
    }

    public void SetEnemyManager()
    {
        enemyManager = this.transform.parent.gameObject.GetComponent<N_EnemyManager3DK>();
    }

    public void EnemyMove(float _move, bool _ref)
    {
        thisTrans.Translate(_move, 0.0f, 0.0f, Space.Self);
        IsReflectionX = _ref;
        rb.velocity = Vector3.zero;
    }

    public void ChaseTarget(float _move)
    {
        thisTrans.Translate(_move, 0.0f, 0.0f, Space.Self);
        rb.velocity = Vector3.zero;
    }

    public int GetTeamNumber()
    {
        return TeamNumber;
    }
}
