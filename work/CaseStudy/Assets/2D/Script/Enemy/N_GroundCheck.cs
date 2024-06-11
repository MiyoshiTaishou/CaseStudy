using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GroundCheck : MonoBehaviour
{
    [Header("地面に当たってるか"), SerializeField]
    private bool isGround = false;

    public List<GameObject> colList = new List<GameObject>();

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
        if (isGround)
        {
            //Debug.Log("当たり" + gameObject.transform.parent.gameObject.name);
        }
        else
        {
            //Debug.Log("はずれ" + gameObject.transform.parent.gameObject.name);

        }

        // コライダーリストの要素が何かしらあれば地面と接触していることになる
        if(colList.Count > 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        foreach(var a in colList)
        {
            Debug.Log(a.name);

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
