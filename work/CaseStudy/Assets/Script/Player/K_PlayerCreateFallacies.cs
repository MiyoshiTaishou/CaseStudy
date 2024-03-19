using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerCreateFallacies : MonoBehaviour
{
    //レイの長さ(仮置き)
    private const float fRayLength = 0.5f;
    //プレイヤー半径(仮置き)
    private const float fPlayerRad = 1.0f;

    //プレイヤーの向き(-1は左、1は右を向いている)
    private int iPlayreDirection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの向きを決める
        float fHorizontalInput = Input.GetAxis("Horizontal");
        if(fHorizontalInput<0.0f)
        {
            iPlayreDirection = -1;
        }
        if(0.0f< fHorizontalInput)
        {
            iPlayreDirection = 1;
        }

        //Fが押されたら
        if(Input.GetKeyDown(KeyCode.F))
        {
            //レイ飛ばす
            Vector3 RayPos = new Vector3(transform.position.x, transform.position.y - fPlayerRad, transform.position.z);
            RaycastHit2D raycastHit = Physics2D.Raycast(RayPos, Vector2.down, fRayLength);
            if (raycastHit)//当たったら
            {
                //接触しているオブジェクトの名前を取得
                string objectName = raycastHit.collider.gameObject.name;

                //取得した名前と向きを基に消すべき床を決める

                //消すタイルの番号
                int index = GetIndexFromObjectName(objectName) + iPlayreDirection;
       
                //消すべき床の名前
                string selectedObjectName = "Tile" + index.ToString();

                // 指定した名前のオブジェクトを検索し、存在する場合は破棄する
                GameObject objectToDelete = GameObject.Find(selectedObjectName);
                if (objectToDelete != null)
                {
                    Destroy(objectToDelete);
                    //ログ出力(debug)
                    Debug.Log("接触している床　: " + objectName);
                    Debug.Log("消した床番号　: " + index);
                }
            }
        }
    }

    // オブジェクトの名前からインデックスを取得するユーティリティメソッド
    int GetIndexFromObjectName(string objectName)
    {
        string indexString = objectName.Substring("Tile".Length);
        return int.Parse(indexString);
    }
}
