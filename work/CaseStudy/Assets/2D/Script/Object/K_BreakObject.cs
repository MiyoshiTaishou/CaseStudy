using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class K_BreakObject : MonoBehaviour
{
    [Header("壊すのに必要な敵の数"), SerializeField]
    private uint iBreakNum = 3;

    [Header("テキストサイズ"), SerializeField]
    private int iTexSize = 100;

    [Header("耐久値を表示するか?"), SerializeField]
    private bool IsDisplayBreakNum = true;

    private TextMeshProUGUI TextEndurance;

    [Header("破壊時のエフェクト"), SerializeField]
    private GameObject Eff_Explosion;

    [Header("爆発音"), SerializeField]
    private AudioClip audioclip;

    private bool isQuitting;

    private void Start()
    {
        if(IsDisplayBreakNum == true)
        {
            //プレハブ実体化
            GameObject cd = transform.GetChild(0).gameObject; ;
            GameObject gcd = cd.GetComponent<Transform>().transform.GetChild(0).gameObject;
            TextEndurance = gcd.GetComponent<TextMeshProUGUI>();
            TextEndurance.text = iBreakNum.ToString();
            TextEndurance.fontSize = iTexSize;
        }
    }

    private void Update()
    {
        if (this.gameObject == null)
        {
            TextEndurance.text = null;
        }
        else
        {
           if (IsDisplayBreakNum == true)
           {
               // オブジェクト位置にテキスト要素を追従させる
               Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
               TextEndurance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z); // 適切なオフセットを持たせる
           }
        }
    }

    public uint GetBreakNum()
    {
        return iBreakNum;
    }

    void OnApplicationQuit()
    {

        isQuitting = true;

    }

    private void OnDestroy()
    {
        if(!isQuitting)
        {
            //爆発エフェクト再生
            if(Eff_Explosion)
            {
                Instantiate(Eff_Explosion, transform.position, Quaternion.identity);
            }
            if (audioclip)
            {
                AudioSource.PlayClipAtPoint(audioclip, transform.position);
            }
        }
    }
}
