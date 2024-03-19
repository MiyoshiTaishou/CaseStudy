using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class M_Blinding : MonoBehaviour
{
    [Header("光るまでの時間"),SerializeField]
    private float fDelay = 2.0f;

    [Header("消えるまでの時間"), SerializeField]
    private float fDeleteTime = 1.0f;
   
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;

        // 一定時間後に当たり判定を有効にするコルーチンを開始
        StartCoroutine(EnableColliderAfterDelay());
    }

    IEnumerator EnableColliderAfterDelay()
    {
        // 光るまでの時間待機
        yield return new WaitForSeconds(fDelay);

        // 当たり判定を有効にする
        circleCollider.enabled = true;

        // 消えるまでの時間待機
        yield return new WaitForSeconds(fDeleteTime);

        // オブジェクトを削除
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {        
        if (_collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(_collision.name);
            //自分とプレイヤーのベクトルを求める
            UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;
           
            //間に壁がないか
            RaycastHit2D RayHit = Physics2D.Raycast(transform.position , vecPos.normalized, vecPos.magnitude);
           
            if (RayHit.collider != null && RayHit.collider.CompareTag("Player"))
            {
                Debug.Log(RayHit.collider.name + "HIT");                
            }                    
        }


        //エネミーの当たり判定
        if(_collision.gameObject.CompareTag("Enemy"))
        {
            //自分とプレイヤーのベクトルを求める
            UnityEngine.Vector2 vecPos = _collision.transform.position - this.transform.position;

            // 自身のコライダーの半径を取得
            float selfColliderRadius = GetComponent<CircleCollider2D>().radius;

            //間に壁がないか
            RaycastHit2D RayHit = Physics2D.Raycast(transform.position + transform.right * (selfColliderRadius + 0.1f), vecPos.normalized, vecPos.magnitude);
            
            if (RayHit.collider == null)
            {
                Debug.Log("エネミーヒット");
            }
        }
    }
}
