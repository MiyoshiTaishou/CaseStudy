using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//プレイヤーの移動関連の処理
public class M_PlayerMove : MonoBehaviour
{
    [Header("移動速度"), SerializeField]
    private float fMoveSpeed = 10.0f;

    enum DASHMODE
    {
        DASH,     // 加速形式
        TELEPORT, // 瞬間移動形式
    }

    [Header("ダッシュのモード"), SerializeField]
     DASHMODE DashMode = DASHMODE.DASH;

    [Header("ダッシュ速度"), SerializeField]
    private float fDashSpeed = 10.0f;

    [Header("何フレームかけてテレポートするか"), SerializeField]
    private int iTeleportFlame = 5;

    [Header("テレポート距離"), SerializeField]
    private float fTeleportDistance = 3.0f;

    private int NowTeleportFlame = 0;
    private bool isNowTeleport = false;
    private bool ButtonTrigger = false;

    private float fStamina;

    [Header("スタミナ関連")]
    [Header("スタミナ最大値"),SerializeField] private float fStaminaMax = 100.0f;
    [Header("スタミナ消費量"),SerializeField] private float fUseDashStamina = 1.0f;
    [Header("スタミナ回復速度"),SerializeField] private float fRecoverySpeed = 2.0f;
    [Header("待機時スタミナ回復速度"), SerializeField] private float fStayRecoverySpeed = 4.0f;
    [Header("スタミナを使い切った後の回復速度"), SerializeField] private float fUsedStaminaRecoverySpeed = 1.0f;

    [Header("スタミナUI関連")]
    [Header("使い切ったときの色"), SerializeField] Color UseColor;
    [Header("注意色"), SerializeField] Color CautionColor;
    [Range(0, 100), Header("どの割合から注意色にするか"), SerializeField] private float CautionParcent = 40.0f;
    [Header("警告色"), SerializeField] Color WarningColor;
    [Range(0, 100), Header("どの割合から警告色にするか"), SerializeField] private float WarningParcent = 20.0f;

    /// <summary>
    /// スタミナUI関連
    /// </summary>
    private Image StaminaImage;

    /// <summary>
    /// 初期の色
    /// </summary>
    private Color StaminaColor;

    /// <summary>
    /// スタミナが回復しきったかどうか
    /// </summary>
    private bool isStamina = true;

    /// <summary>
    /// ダッシュ中か
    /// </summary>
    private bool isDash = true;
  

    private Rigidbody2D rbPlayer;
   
    /// <summary>
    /// どちらを向いているか
    /// </summary>
    private Vector3 vecDir;

    public Vector3 GetDir() { return vecDir; }
    public void SetDir(Vector3 _dir) { vecDir = _dir; }

    /// <summary>
    /// 移動可能か
    /// </summary>
    private bool isMove = true;

    /// <summary>
    /// ダッシュしているか
    /// </summary>
    private bool isNowDash = true;

    public void SetIsNowDahs(bool _dash) { isNowDash = _dash; }

    private Transform PlayerTrans;

    public bool GetIsDash() { return isNowDash; }

    public bool GetIsMove() { return isMove; }
    public void SetIsMove(bool _move) { isMove = _move; }

    /// <summary>
    ///アニメーション関連
    /// </summary>
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        vecDir = transform.right;

        PlayerTrans = gameObject.transform;

        //スタミナを最大にする
        fStamina = fStaminaMax;

        //UIを探す
        StaminaImage = GameObject.Find("StaminaBar").GetComponent<Image>();
        StaminaColor = StaminaImage.color;

        // 子オブジェクトの中から、アニメーターを取得
        animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {        
        if(!isMove || !M_GameMaster.GetGamePlay())
        {
            isDash = false;
            return;
        }

        // キーボード入力を受け取る
        float fHorizontalInput = Input.GetAxis("Horizontal");
       
        //ダッシュボタンを押しているか
        //if(Input.GetButton("DashButton"))
        if(Input.GetAxis("DashButton") < 0)
        {
            if (!isNowTeleport && !ButtonTrigger)
            {
                // テレポート用変数
                isNowTeleport = true;
                ButtonTrigger = true;
            }

            // ダッシュ用変数
            isDash = true;
        }
        else
        {
            isDash = false;
            ButtonTrigger = false;
        }

        //移動処理を呼ぶ
        switch (DashMode)
        {
            case DASHMODE.DASH:
                MoveUpdate(fHorizontalInput);
                //UI処理を呼ぶ
                StaminaUIUpdate();
                break;

            case DASHMODE.TELEPORT:
                Teleport(fHorizontalInput);
                break;
        }
    }

    // 瞬間移動
    private void Teleport(float _fhorizontal)
    {
        // テレポート中
        if (isNowTeleport && NowTeleportFlame < iTeleportFlame)
        {
            // このフレームでの移動距離
            float moveDis = fTeleportDistance / iTeleportFlame;

            if (vecDir.x > 0)
            {
                PlayerTrans.position += new Vector3(moveDis, 0.0f, 0.0f);
            }
            else if (vecDir.x < 0)
            {
                PlayerTrans.position -= new Vector3(moveDis, 0.0f, 0.0f);
            }

            NowTeleportFlame++;

            // テレポート用
            // 指定フレーム分経ったら
            if (NowTeleportFlame >= iTeleportFlame)
            {
                // テレポート終了
                isNowTeleport = false;
                NowTeleportFlame = 0;
            }
        }
        else
        {
            // 入力に基づいて移動する
            Vector2 vecMoveDirection = new Vector2(_fhorizontal * fMoveSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;
            animator.SetBool("run", true);

            if (_fhorizontal > 0.0f)
            {
                vecDir = new Vector3(1.0f,0.0f,0.0f);
                transform.eulerAngles = Vector3.zero;
            }
            else if (_fhorizontal < 0.0f)
            {
                vecDir = new Vector3(-1.0f,0.0f,0.0f);
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
        }
    }

    //移動関連の処理をする
    private void MoveUpdate(float _forizontal)
    {
        //スタミナを使い切っていなくて走るボタンを押している時
        if (isDash && isStamina)
        {
                    
            // 入力に基づいて移動する
            Vector2 vecMoveDirection = new Vector2(_forizontal * fDashSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;            

            if (_forizontal > 0.0f)
            {
                vecDir = transform.right;
                transform.eulerAngles = Vector3.zero;
            }
            else if (_forizontal < 0.0f)
            {
                vecDir = -transform.right;
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }

            //移動している場合はスタミナを消費する
            if (_forizontal != 0.0f)
            {
                fStamina -= fUseDashStamina * Time.deltaTime;
                isNowDash = true;
                animator.SetBool("glider", true);
                animator.SetBool("run", false);
            }
            else
            {
                fStamina += fStayRecoverySpeed * Time.deltaTime;
                isNowDash = false;
                animator.SetBool("glider", false);
                animator.SetBool("run", false);

            }
        }
        else
        {
            // 入力に基づいて移動する
            Vector2 vecMoveDirection = new Vector2(_forizontal * fMoveSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;
            animator.SetBool("run", true);
            animator.SetBool("glider", false);

            if (_forizontal > 0.0f)
            {
                vecDir = transform.right;
                transform.eulerAngles = Vector3.zero;
            }
            else if (_forizontal < 0.0f)
            {
                vecDir = -transform.right;
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }

            //使いきったか待機しているか歩いているかで回復速度を変える
            if (isStamina)
            {
                if (_forizontal == 0.0f)
                {                    
                    fStamina += fStayRecoverySpeed * Time.deltaTime;
                    animator.SetBool("run", false);
                    animator.SetBool("glider", false);
                }
                else
                {                    
                    fStamina += fRecoverySpeed * Time.deltaTime;
                }
            }
            else
            {               
                fStamina += fUsedStaminaRecoverySpeed * Time.deltaTime;
            }

            isNowDash = false;
        }

        //スタミナが最大まで回復したか
        if (fStamina > fStaminaMax)
        {
            fStamina = fStaminaMax;
            isStamina = true;
        }

        //スタミナを使い切ったか
        if (fStamina < 0)
        {
            fStamina = 0;
            isStamina = false;
        }

        if (_forizontal == 0.0f && Input.GetAxis("DashButton") == 0)
        {            
            animator.SetBool("run", false);
        }       
    }

    //UI関連の処理をする
    private void StaminaUIUpdate()
    {
        //UIの伸びを計算
        StaminaImage.fillAmount = fStamina / fStaminaMax;

        //％にする
        float StamineParcent = (fStamina / fStaminaMax) * 100;

        //スタミナを使い切った時は色を変える
        if(!isStamina)
        {
            StaminaImage.color = UseColor;

            return;
        }

        if(StamineParcent > CautionParcent)
        {
            StaminaImage.color = StaminaColor;

            return;
        }

        //警告ライン寄りしたなら色を変える
        if (StamineParcent < WarningParcent)
        {
            StaminaImage.color = WarningColor;

            return;
        }

        //注意ライン寄りしたなら色を変える
        if (StamineParcent < CautionParcent)
        {
            StaminaImage.color = CautionColor;

            return;
        }      
    }
}
