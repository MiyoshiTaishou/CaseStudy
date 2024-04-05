using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Rendering;
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

    //押されているかどうか
    private bool isPushing = false;
    public bool GetisPushing() { return isPushing; }
    public void SetisPushing(bool _flg) { isPushing = _flg; }
    private GameObject ColObject;

    private float fStickCnt = 0;

    private Vector3 defaultScale;

    public int GetStickCount() 
    {
        int temp = 0;
        temp = Mathf.FloorToInt(fStickCnt);
        return temp; }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale= transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //吸収した敵の数に応じて巨大化
        Vector3 temp= defaultScale;
        temp.x -= fStickCnt / 5;
        temp.y += fStickCnt / 5;
        transform.localScale = temp;
        if(isPushing)
        {
            GetComponent<SEnemyMove>().enabled = false;
            GetComponent<M_BlindingMove>().enabled = false;
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
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing())
            {
                fStickCnt++;
                Destroy(ColObject);
                GetComponent<Rigidbody2D>().AddForce(GetComponent<Rigidbody2D>().velocity*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                StartCoroutine(HitStop());
            }
        }
    }
    IEnumerator HitStop()
    {
        //速度を保存し、0にする
        Vector2 vel=GetComponent<Rigidbody2D>().velocity;
        if(vel.x>fLimitSpeedx)
        {
            vel.x = fLimitSpeedx;
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //指定のフレーム待つ
        yield return new WaitForSeconds(fHitStop/60);
        //保存した速度で再開する
        GetComponent<Rigidbody2D>().velocity = vel;
    }
}
