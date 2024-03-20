using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目くらまし中の行動
public class M_BlindingMove : MonoBehaviour
{
    [Header("目くらまし中の移動速度"), SerializeField]
    private float fMoveSpeed = 5.0f;

    [Header("目くらましの時間"), SerializeField]
    private float fBlindingTime = 5.0f;

    /// <summary>
    /// 目くらまし中か
    /// </summary>
    private bool isBlinding = false;

    /// <summary>
    /// 時間計測用
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// 目くらましの方向
    /// </summary>
    private Vector2 vecDirBlinding;

    private Rigidbody2D rbEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //目くらまし中かどうか
        if(isBlinding)
        {
            //時間計測
            fTime += Time.deltaTime;

            BlindingMove();
        }
        else
        {
            fTime = 0.0f;
        }

        //目標時間まで経過したら目くらまし解除
        if(fTime > fBlindingTime)
        {
            isBlinding = false;
            fTime = 0.0f;
        }
    }

    /// <summary>
    /// 目くらまし中の行動
    /// </summary>
    private void BlindingMove()
    {       
        //動く力設定
        Vector2 vecMoveDirection = new Vector2(vecDirBlinding.x * fMoveSpeed, rbEnemy.velocity.y);
        rbEnemy.velocity = vecMoveDirection;
    }

    public void SetIsBlinding(bool _isBlinding)
    {
        isBlinding = _isBlinding;
    }

    public bool GetIsBlinding()
    {
        return isBlinding;
    }

    public void SetVecDirBlinding(Vector2 _vecDir)
    {
        vecDirBlinding = _vecDir;
    }
}
