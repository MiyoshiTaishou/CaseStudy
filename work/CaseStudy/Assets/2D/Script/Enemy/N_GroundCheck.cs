using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GroundCheck : MonoBehaviour
{
    [Header("地面に当たってるか"), SerializeField]
    private bool isGround = false;

    public List<GameObject> colList = new List<GameObject>();

    private float fallTime = 0.0f;
    private float OldFallTime = 0.0f;

    public float GetFallTime()
    {
        return OldFallTime;
    }

    public bool GroundCheck()
    {
        return isGround;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // コライダーリストの要素が何かしらあれば地面と接触していることになる
        if(colList.Count > 0)
        {
            isGround = true;

            if(fallTime != 0.0f)
            {
                OldFallTime = fallTime;
            }

            fallTime = 0.0f;
        }
        else
        {
            isGround = false;
            fallTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 接触判定したいタグ
        // コライダーリストに登録されていなければ
        if(/*(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Hologram")) &&*/
            !colList.Contains(collision.gameObject))
        {
            // リストに登録
            colList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 接触判定したいタグ
        // コライダーリストに登録されていなければ
        if (/*(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Hologram")) &&*/
            colList.Contains(collision.gameObject))
        {
            // リストから削除
            colList.Remove(collision.gameObject);
        }
    }
}
