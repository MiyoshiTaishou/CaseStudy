using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class K_DisplayStickEnemyNumVer2 : MonoBehaviour
{
    [Header("����\������e�L�X�g�̃v���n�u"), SerializeField]
    private GameObject TextPrefab;

    private GameObject[] enemies; // ��ʏシ�ׂĂ̓G
    private Text[] Texts; // �G�̐����̃e�L�X�g

    void Start()
    {
        // ��ʏ�̂��ׂĂ̓G���擾
        enemies = new GameObject[GameObject.FindGameObjectsWithTag("Enemy").Length];
        int index = 0;
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies[index] = enemyObj;
            index++;
        }

        // �e�L�X�g�𐶐�
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
            //�G�����݂��Ă��邩
            if(enemies[i]==null)
            {//���݂��Ȃ�������
                Texts[i].text = null;
            }
            else
            {//���݂�����
                // �G�̈ʒu�Ƀe�L�X�g�v�f��Ǐ]������
                Vector3 screenPos = Camera.main.WorldToScreenPoint(enemies[i].transform.position);
                Texts[i].transform.position = new Vector3(screenPos.x, screenPos.y + 50, screenPos.z); // �K�؂ȃI�t�Z�b�g����������
                
                // �e�L�X�g�ɔ��f
                int StickEnemyNum = enemies[i].GetComponent<S_EnemyBall>().GetStickCount();
                if(StickEnemyNum==0)
                {
                    Texts[i].text = null;
                }
                else
                {
                    Texts[i].text = StickEnemyNum.ToString();
                }
                //�T�C�Y�ς���
                Texts[i].fontSize = 30 + StickEnemyNum * 10;
            }
        }
    }
}
