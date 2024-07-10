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

    private int ChildCount;
    private Vector3 InitSize;

    // Start is called before the first frame update
    void Start()
    {
        //コメント無くてごめんねー
        ChildCount = 0;
        InitSize = this.gameObject.transform.localScale;
        int num = Num;
        for(int i=0; num!=0; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            obj.transform.position = new Vector3(this.transform.position.x - i, this.transform.position.y, this.transform.position.z) ;
            obj.name = i.ToString();
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = Tex[num % 10];
            ChildCount++;
            if (num/10 == 0)
            {
                if(i==0)
                {
                    this.gameObject.transform.localScale = InitSize * Size;
                }
                else
                {
                    this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
                }
                break;
            }
            else
            {
                num = num / 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if(ActiveNumCount <= 1)
        {
            this.gameObject.transform.localScale = InitSize * Size;
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(InitSize.x * Size * 0.7f, InitSize.y * Size, InitSize.z * Size);
        }
    }
}
