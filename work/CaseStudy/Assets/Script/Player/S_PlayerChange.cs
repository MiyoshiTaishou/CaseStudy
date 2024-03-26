using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    ///Bボタンが押されたかどうか
    private bool isBDown = false;

    //敵が視界内にいるかどうか
    private bool isSerch = false;

    [Header("変身時間"), SerializeField]
    float fWaitTime = 10.0f;

    //変身中かどうか
    private bool isChanging = false;
    bool GetisChanging() { return isChanging; }

    //変身前の見た目(Player)
    private Sprite PlayerSprite;

    //変身後の見た目(Enemy)
    private Sprite EnemySprite;

    private SpriteRenderer srEnemy = null;

    private SpriteRenderer srPlayer = null;

    private Collider2D colEnemy;
    // Start is called before the first frame update
    void Start()
    {
        srPlayer = transform.root.GetComponent<SpriteRenderer>();
        if(!srPlayer)
        {
            Debug.LogError("SpriteRendererがありません");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //変身ボタンが押されたかどうか判定
        isBDown = Input.GetKeyDown(KeyCode.C);//ここでキー指定してね
        Debug.Log("ボタン押下状況=" + isBDown);
        if (isBDown&&isSerch) 
        {
            srEnemy = colEnemy.GetComponent<SpriteRenderer>();
            EnemySprite = srEnemy.sprite;
            StartCoroutine(Change());
        }
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        //敵に接触していればその敵の盲目状態と見た目の情報を取得
        if(_collision.tag=="Enemy")
        {
            M_BlindingMove blindingMove =_collision.GetComponent<M_BlindingMove>();
            isSerch=blindingMove.GetIsBlinding();
            colEnemy = _collision;
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        isSerch = false;
    }

    //コルーチン。
    IEnumerator Change()
    {
        //終わるまで待ってほしい処理を書く
        //変身中でなければプレイヤーと敵の見た目を入れ替える、変身中であれば変身時間をリセットするだけ
        if (!isChanging)
        {
            //現在の状況に応じてタグと見た目を変更
            transform.root.tag = "Enemy";
            PlayerSprite = srPlayer.sprite;
            //見た目の変更
            srPlayer.sprite = EnemySprite;
            srEnemy.sprite = PlayerSprite;
            isChanging= true;
        }
        //指定の秒数待つ
        yield return new WaitForSeconds(fWaitTime);
        //再開してから実行したい処理を書く

        //見た目を元に戻す
        srPlayer.sprite = PlayerSprite;
        srEnemy.sprite = EnemySprite;
        Debug.Log("変身解除");
        transform.root.tag = "Player";
        isChanging= false;
    }
}
