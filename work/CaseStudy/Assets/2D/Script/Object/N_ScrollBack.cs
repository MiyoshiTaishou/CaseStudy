using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ScrollBack : MonoBehaviour
{
    enum SCROLLMODE
    {
        STATIC,
        DYNAMIC
    }

    // スクロール時のモード
    [Header("スクロールモード"), SerializeField]
    private SCROLLMODE scrollMode = SCROLLMODE.STATIC;

    [Header("移動の強さ(0.0～0.99)"), SerializeField]
    private float TranckingStrength = 0.99f;

    // BasicAreaをもとに移動距離が決まる
    [Header("移動量の元になる値"), SerializeField]
    private float BasicArea = 1.0f;

    // レイヤーを移動の強さとリンクさせる
    private int layer;

    // Start is called before the first frame update
    void Start()
    {
        if(TranckingStrength <= 0.0f)
        {
            TranckingStrength = 0.0f;
        }
        else if(TranckingStrength >= 0.99f)
        {
            TranckingStrength = 0.99f;
        }

        layer = -100 + (int)(100 * TranckingStrength);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
    }

    // Update is called once per frame
    void Update()
    {
        // キーボード入力を受け取る
        float fHorizontalInput = Input.GetAxis("Horizontal");

        // 追従するなら
        if (scrollMode == SCROLLMODE.DYNAMIC)
        {
            Vector3 vec = new Vector3(-fHorizontalInput * TranckingStrength * BasicArea * Time.deltaTime,0.0f,0.0f);
            gameObject.transform.Translate(vec, Space.Self);
        }
    }
}