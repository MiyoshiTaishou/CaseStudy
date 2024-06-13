using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LoopWall : MonoBehaviour
{
    [Header("ワープ先のオブジェクト"), SerializeField]
    private GameObject warpObj;

    bool isWarped = false;
    public bool GetisWarped() { return isWarped; }

    [Header("右側に出る？"), SerializeField]
    private bool iswarpRight = false;

    [Header("ワープ時の音"), SerializeField]
    private AudioClip audioclip = null;

    private AudioSource audioSource = null;

    //ワープ時に右側に出るかを取得
    public bool GetiswarpRight() { return iswarpRight; }

    GameObject ColObject = null;
    float speedx = 0;

    /// <summary>
    /// 速度を保存するための変数
    /// </summary>
    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        if (!warpObj.GetComponent<S_LoopWall>())
        {
            Debug.LogError("ワープ先にこのスクリプトがないけんです");
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //すり抜け時に物体の速度を保存

        Vector2 vel2 = collision.GetComponent<Rigidbody2D>().velocity;
        vel = vel2;
        speedx = vel2.x;
        Debug.Log("ｓぴーーーーーーーーーど" + speedx);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ワープ先がワープ出来る状態か
        bool OK = warpObj.GetComponent<S_LoopWall>().GetisWarped();

        //自身がワープでき、ワープ先がワープでき、タグがプレイヤーかエネミーの時にワープ処理
        if (isWarped == false && OK == false &&
            (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy")))
        {
            //一定時間ワープ不可の状態にする
            StartCoroutine(CoolTime());
            
            //当たったのが敵ならワープしたという情報を付与
            if(collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<SEnemyMove>().SetisWarped(true);
            }

            //ワープ先の位置
            Vector3 newpos = warpObj.transform.position;

            //右側に出るか左側に出るか、位置を微調整
            if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == true)
            {
                newpos.x += 2.0f;
            }
            else if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == false)
            {
                newpos.x -= 2.0f;
            }
            audioSource.PlayOneShot(audioclip);
            //ワープ
            collision.gameObject.transform.position = newpos;
        }
        //敵玉の場合(見返してみれば分ける必要無かったかも)
        else if (isWarped == false && OK == false &&
            collision.collider.CompareTag("EnemyBall"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            //speedx = rb.velocity.x;
            //一定時間ワープ不可の状態にする
            StartCoroutine(CoolTime());

            //ワープ先の位置の設定と微調整
            Vector3 newpos = warpObj.transform.position;
            if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == true)
            {
                newpos.x += 2.0f;
            }
            else if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == false)
            {
                newpos.x -= 2.0f;
            }
            audioSource.PlayOneShot(audioclip);
            collision.gameObject.transform.position = newpos;

            //vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //敵玉が離れた場合に保存していた速度を再び与える
        if (collision.collider.CompareTag("EnemyBall"))
        {
            Debug.Log("すすめ");
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            vel.x = speedx;
            rb.velocity = vel;
            collision.gameObject.GetComponent<S_EnemyBall>().SetisPushing(true);
        }
    }

    IEnumerator CoolTime()
    {
        isWarped = true;
        warpObj.GetComponent<S_LoopWall>().isWarped = true;
        //指定のフレーム待つ
        yield return new WaitForSecondsRealtime(0.2f);
        Debug.Log("ああああ");
        isWarped = false;
        warpObj.GetComponent<S_LoopWall>().isWarped = false;
        //if(ColObject.CompareTag("EnemyBall"))
        //{
        //    Rigidbody2D rb = ColObject.GetComponent<Rigidbody2D>();
        //    Vector2 vel = rb.velocity;
        //    vel.x = speedx;
        //    rb.velocity = vel;
        //}
    }
}
