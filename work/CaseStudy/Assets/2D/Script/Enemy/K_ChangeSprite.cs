using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ChangeSprite : MonoBehaviour
{
    [Header("�G�X�v���C�g(�ʏ펞)"), SerializeField]
    private GameObject EnemyObjNormal;

    [Header("�G�X�v���C�g(�]���莞)"), SerializeField]
    private GameObject EnemyObjRolling;

    //���̃I�u�W�F�N�g�ɂ������Ă���G�X�v���C�g(�ʏ펞)
    private GameObject EnemySpriteNormal;

    //�]���莞�̓G�X�v���C�g(�]���莞)
    private GameObject EnemySpriteRolling;

    private S_EnemyBall EnemyBall;

    private void Start()
    {
        string strEnemySprites = EnemyObjNormal.name;
        EnemySpriteNormal = this.transform.Find(strEnemySprites).gameObject;

        EnemySpriteRolling= Instantiate(EnemyObjRolling, transform.position, Quaternion.identity);
        EnemySpriteRolling.SetActive(false);
        EnemySpriteRolling.transform.parent = this.transform;

        EnemyBall = GetComponent<S_EnemyBall>();
    }

    private void Update()
    {
        bool IsRolling = EnemyBall.GetisPushing();
        if (IsRolling)
        {
            EnemySpriteNormal.SetActive(false);
            EnemySpriteRolling.SetActive(true);
            EnemySpriteRolling.transform.position = this.transform.position;
        }
        else
        {
            EnemySpriteNormal.SetActive(true);
            EnemySpriteNormal.transform.position = this.transform.position;
            EnemySpriteRolling.SetActive(false);
        }
    }
}
