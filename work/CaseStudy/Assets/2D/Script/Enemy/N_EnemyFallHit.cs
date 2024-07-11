using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_EnemyFallHit : MonoBehaviour
{
    public GameObject Parent;
    public GameObject EnemySprite;
    public Animator animator;
    public SEnemyMove EnemyMove;
    public N_EnemyManager EnemyMana;

    // 新チーム発足後結合可能までの時間
    private float IntavalTime = 1.0f;

    public string ManagerName = "";

    public bool isFallBall = false;

    public bool init = false;

    private bool isUnion = false;

    public void SetUnion(bool _union)
    {
        isUnion = _union;
    }

    public bool GetUnion()
    {
        return isUnion;
    }

    public void SetFallBall(bool _fallball)
    {
        isFallBall = _fallball;
    }

    public bool GetFallBall()
    {
        return isFallBall;
    }

    public string GetManagerName()
    {
        EnemyMana = EnemyMove.GetManager();
        if(EnemyMana == null)
        {
            return "ManagerNull";
        }
        ManagerName = EnemyMana.name;
        return ManagerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            Parent = transform.parent.gameObject;
            EnemySprite = Parent.transform.GetChild(2).gameObject;
            animator = EnemySprite.GetComponent<Animator>();
            EnemyMove = Parent.GetComponent<SEnemyMove>();
            EnemyMana = EnemyMove.GetManager();
            //ManagerName =EnemyMana.name;

            init = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator || isFallBall)
        {
            return;
        }

        string hostName = GetManagerName();
        N_EnemyFallHit colFallHit = collision.GetComponent<N_EnemyFallHit>();
        string guestName = colFallHit.GetManagerName();

        // 同じ名前のマネージャーに所属していたら合体しない
        if (hostName == guestName || hostName == "ManagerNull" || guestName == "ManagerNull")
        {
            if(guestName == "ManagerNull")
            {
                if (animator.GetBool("fall"))
                {
                    Debug.Log("玉合体エントリー");
                    isFallBall = true;

                    colFallHit.SetFallBall(true);
                    collision.gameObject.transform.parent.gameObject.GetComponent<S_EnemyBall>().SetFallHitChangeBall(Parent);
                }
            }
            return;
        }

        // 新チーム発足後インターバルを設ける
        if (EnemyMana.GetNewTeamIntavalTime() <= IntavalTime || collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() <= IntavalTime)
        {
            // どちらもインターバル中なら抜けずに合体
            if (!(EnemyMana.GetNewTeamIntavalTime() > 0.0f && collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() > 0.0f))
            {
                return;
            }
        }

        Debug.Log("FallHit");
        if (animator.GetBool("fall"))
        {
            Debug.Log("玉合体エントリー");
            isFallBall = true;

            colFallHit.SetFallBall(true);
            collision.gameObject.transform.parent.gameObject.GetComponent<S_EnemyBall>().SetFallHitChangeBall(Parent);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!animator || isFallBall)
        {
            return;
        }

        string hostName = GetManagerName();
        N_EnemyFallHit colFallHit = collision.GetComponent<N_EnemyFallHit>();
        string guestName = colFallHit.GetManagerName();

        // 同じ名前のマネージャーに所属していたら合体しない
        if (hostName == guestName || hostName == "ManagerNull" || guestName == "ManagerNull")
        {
            if (guestName == "ManagerNull")
            {
                if (animator.GetBool("fall"))
                {
                    Debug.Log("玉合体エントリー");
                    isFallBall = true;

                    colFallHit.SetFallBall(true);
                    collision.gameObject.transform.parent.gameObject.GetComponent<S_EnemyBall>().SetFallHitChangeBall(Parent);
                }
            }
            return;
        }

        // 新チーム発足後インターバルを設ける
        if (EnemyMana.GetNewTeamIntavalTime() <= IntavalTime || collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() <= IntavalTime)
        {
            // どちらもインターバル中なら抜けずに合体
            if (!(EnemyMana.GetNewTeamIntavalTime() > 0.0f && collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() > 0.0f))
            {
                return;
            }
        }

        if (animator.GetBool("fall"))
        {
            Debug.Log("玉合体ステイ");
            isFallBall = true;

            colFallHit.SetFallBall(true);
            collision.gameObject.transform.parent.gameObject.GetComponent<S_EnemyBall>().SetFallHitChangeBall(Parent);
        }
    }

    private float GetNewTeamIntavalTime()
    {
        return EnemyMana.GetNewTeamIntavalTime();
    }
}
