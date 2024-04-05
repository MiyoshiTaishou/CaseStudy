using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class K_DisplayStickEnemyNumVer2 : MonoBehaviour
{
    [Header("数を表示するテキストのプレハブ"), SerializeField]
    private GameObject TextPrefab;

    private GameObject[] enemies; // 画面上すべての敵
    private Text[] Texts; // 敵の数分のテキスト

    void Start()
    {
        // 画面上のすべての敵を取得
        enemies = new GameObject[GameObject.FindGameObjectsWithTag("Enemy").Length];
        int index = 0;
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies[index] = enemyObj;
            index++;
        }

        // テキストを生成
        Texts = new Text[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject healthBar = Instantiate(TextPrefab, transform.position, Quaternion.identity);
            Texts[i] = healthBar.GetComponent<Text>();
            healthBar.transform.SetParent(transform);
        }
    }

    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            //敵が存在しているか
            if(enemies[i]==null)
            {//存在しなかったら
                Texts[i].text = null;
            }
            else
            {//存在したら
                // 敵の位置にテキスト要素を追従させる
                Vector3 screenPos = Camera.main.WorldToScreenPoint(enemies[i].transform.position);
                Texts[i].transform.position = new Vector3(screenPos.x, screenPos.y + 50, screenPos.z); // 適切なオフセットを持たせる
                
                // テキストに反映
                int StickEnemyNum = enemies[i].GetComponent<S_EnemyBall>().GetStickCount();
                if(StickEnemyNum==0)
                {
                    Texts[i].text = null;
                }
                else
                {
                    Texts[i].text = StickEnemyNum.ToString();
                }
                //サイズ変える
                Texts[i].fontSize = 30 + StickEnemyNum * 10;
            }
        }
    }
}
