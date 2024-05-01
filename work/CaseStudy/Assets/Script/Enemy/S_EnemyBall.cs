using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Rendering;
#endif
using UnityEngine;

public class S_EnemyBall : MonoBehaviour
{
    [Header("増幅係数"), SerializeField]
    float fBoost = 1.5f;

    [Header("AudioClip"), SerializeField]
    AudioClip audioclip;

    [Header("ヒットストップ"), SerializeField]
    float fHitStop = 0;

    [Header("制限速度(x)"), SerializeField]
    float fLimitSpeedx = 15.0f;

    [Header("停止判定"), SerializeField]
    float fStopjudge = 0.0f;

    [Header("大きさの段階と必要な吸収数"), SerializeField]
    int[] nGiantNum;

    [Header("振動時間"), SerializeField]
    private float fTime = 0.1f;

    //塊になっているかどうか
    private bool isBall = false;
    public bool GetisBall() { return isBall; }

    //プレイヤーによって押されているかどうか
    private bool isPushing = false;
    public bool GetisPushing() { return isPushing; }
    public void SetisPushing(bool _flg) { isPushing = _flg; }

    private GameObject ColObject;

    private Vector3 defaultScale;

    //くっついている個数
    private float fStickCnt = 0;

    private Rigidbody2D rb;

    private Vector2 vel;

    public int GetStickCount() 
    {
        int temp = 0;
        temp = Mathf.FloorToInt(fStickCnt);
        return temp; }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale= transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isPushing)
        {
           GetComponent<SEnemyMove>().enabled = false;
           GetComponent<M_BlindingMove>().enabled = false;
           GetComponent<MPlayerSearch>().enabled = false;
            vel = rb.velocity;
        }
        if(isBall&& Mathf.Abs(rb.velocity.x) < fStopjudge) 
        {
            isPushing = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject= _collision.gameObject;
        if(ColObject.tag=="Enemy")
        {
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing()||
                (ColObject.GetComponent<S_EnemyBall>().GetisPushing()&&fStickCnt > ColObject.GetComponent<S_EnemyBall>().GetStickCount()))
            {
                isBall = true;
                fStickCnt++;
                if(fStickCnt==1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x -= GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Destroy(ColObject);
                rb.AddForce(vel*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch+=0.2f;

                StartCoroutine(M_Utility.GamePadMotor(fTime));
                StartCoroutine(HitStop());
            }
        }
    }
    private void OnCollisionStay2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //あたったオブジェクトが敵かつ押されていなければ吸収
        ColObject= _collision.gameObject;
        if(ColObject.tag=="Enemy")
        {
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing()||
                (ColObject.GetComponent<S_EnemyBall>().GetisPushing()&&fStickCnt > ColObject.GetComponent<S_EnemyBall>().GetStickCount()))
            {
                isBall = true;
                fStickCnt++;
                if (fStickCnt == 1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //吸収した敵の数に応じて巨大化
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x -= GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Destroy(ColObject);
                rb.AddForce(rb.velocity*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                StartCoroutine(HitStop());
            }
        }
    }
    IEnumerator HitStop()
    {
        //速度を保存し、0にする
        Vector2 vel=rb.velocity;
        if(vel.x>fLimitSpeedx)
        {
            vel.x = fLimitSpeedx;
        }
        else if(vel.x<-fLimitSpeedx)
        {
            vel.x = -fLimitSpeedx;
        }
        rb.velocity = Vector2.zero;
        //指定のフレーム待つ
        yield return new WaitForSeconds(fHitStop/60);
        //保存した速度で再開する
        rb.velocity = vel;
        isPushing = true;
    }

    //何段階巨大化したかを取得する関数
    private int GetGiantLv()
    {
        int temp = Mathf.FloorToInt(fStickCnt);
        int[] array = nGiantNum;
        int lv = 0;
        int i = 0;
        while(i<nGiantNum.Length) 
        {
            temp -= nGiantNum[i];
            if(temp>=0)
            {
                lv++;
            }
            else if(temp<0)
            {
                break;
            }
            i++;
        }
        return lv;
    }
}
