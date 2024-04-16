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

    //���̃I�u�W�F�N�g�ɂ������Ă���G�X�v���C�g(�]���莞)
    private GameObject EnemySpriteRolling;

    private S_EnemyBall EnemyBall;

    private void Start()
    {
        string strEnemySprites = EnemyObjNormal.name;
        EnemySpriteNormal = this.transform.Find(strEnemySprites).gameObject;

        strEnemySprites = EnemyObjRolling.name;
        EnemyObjRolling = this.transform.Find(strEnemySprites).gameObject;
        EnemyObjRolling.SetActive(false);

        EnemyBall = GetComponent<S_EnemyBall>();
    }

    private void Update()
    {
        int StickEnemyNum = EnemyBall.GetStickCount();
        if(StickEnemyNum > 0)
        {
            EnemySpriteNormal.SetActive(false);
            EnemyObjRolling.SetActive(true);
        }
    }
}
