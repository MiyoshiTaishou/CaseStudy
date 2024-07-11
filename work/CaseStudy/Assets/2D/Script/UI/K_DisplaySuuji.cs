using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_DisplaySuuji : MonoBehaviour
{
    [Header("数字画像"), SerializeField]
    private Sprite[] Tex;

    [Header("表示したい数字"), SerializeField]
    private int Num = 0;
    public void SetNum(int _Num) { Num = _Num; }

    [Header("サイズ"), SerializeField]
    private float Size = 1;

    public void SetSize(int _Size) { Size = _Size; }

    //子オブジェクト数
    private int ChildCount;

    //初期サイズ
    private Vector3 InitSize;

    // Start is called before the first frame update
    void Start()
    {
        //初期情報保存
        ChildCount = 0;
        InitSize = this.gameObject.transform.localScale;
        //最大まで子オブジェクトを生成する
        int num = 2147483647;
        for(int i=0; num!=0; i++)
        {
            //桁毎の子オブジェクトを生成
            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            obj.transform.position = new Vector3(this.transform.position.x - i, this.transform.position.y, this.transform.position.z) ;
            obj.layer = 5;
            //名前変更
            obj.name = i.ToString();
            //スプライトレンダラー追加
            obj.AddComponent<SpriteRenderer>();
            //桁毎の数を割り振る
            obj.GetComponent<SpriteRenderer>().sprite = Tex[num % 10];
            obj.GetComponent<SpriteRenderer>().sortingOrder = 20;
            //子オブジェクトの数+1
            ChildCount++;
            //最大桁か判断する
            if (num/10 == 0)
            {//最大桁だったら
                //桁数に応じてサイズ変える
                if(i==0)
                {//一桁だったら
                    this.gameObject.transform.localScale = InitSize * Size;
                }
                else
                {//二桁以上だったら
                    this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
                }
                break;
            }
            else
            {//次の桁の計算に
                num = num / 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //数字を更新する
        int ActiveNumCount = 0;
        int num = Num;
        for (int i = 0; i< ChildCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            obj.GetComponent<SpriteRenderer>().sprite = Tex[num % 10];
            if(num<=0)
            {
                obj.SetActive(false);
            }
            else
            {
                ActiveNumCount++;
                obj.SetActive(true);
            }
            num = num / 10;
        }


        //桁数に応じてサイズ変える
        if(ActiveNumCount <= 1)
        {
            this.gameObject.transform.localScale = InitSize * Size;
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
        }

        //位置調整
        float Mediannum = 0;
        if(ActiveNumCount % 2==0)
        {
            Mediannum = ActiveNumCount / 2 + 0.5f;
            for (int i = 1; i <= ActiveNumCount; i++)
            {
                GameObject obj = transform.GetChild(i-1).gameObject;
                obj.transform.position = new Vector3(this.transform.position.x + (Mediannum - i) * Size * 0.65f, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            Mediannum = ActiveNumCount / 2 + 1;
            for (int i = 1; i <= ActiveNumCount; i++)
            {
                GameObject obj = transform.GetChild(i-1).gameObject;
                obj.transform.position = new Vector3(this.transform.position.x + (Mediannum - i) * Size * 0.65f, this.transform.position.y, this.transform.position.z);
            }
        }
    }
}
