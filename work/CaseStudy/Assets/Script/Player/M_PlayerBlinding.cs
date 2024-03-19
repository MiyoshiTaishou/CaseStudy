using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目くらまし処理
public class M_PlayerBlinding : MonoBehaviour
{
    [Header("投げるオブジェクト"), SerializeField]
    private GameObject BlindingObj;

    [Header("投げる力"), SerializeField]
    private float fThrowPower = 5.0f;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }

    //投げる処理
    void Throw()
    {
        //プレハブから生成する
        GameObject blinding = Instantiate(BlindingObj, transform.position, Quaternion.identity);

        Rigidbody2D rb = blinding.GetComponent<Rigidbody2D>();

        // 斜め上に力を加える
        rb.AddForce(this.transform.up * fThrowPower, ForceMode2D.Impulse);
    }
}
