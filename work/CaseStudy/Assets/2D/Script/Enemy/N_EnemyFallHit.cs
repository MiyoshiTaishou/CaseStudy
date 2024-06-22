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

        //Debug.Log(animator.GetBool("fall"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator || isFallBall)
        {
            return;
        }

        string hostName = GetManagerName();
        string guestName = collision.GetComponent<N_EnemyFallHit>().GetManagerName();

        // 同じ名前のマネージャーに所属していたら合体しない
        if (hostName == guestName || hostName == "ManagerNull" || guestName == "ManagerNull")
        {
            return;
        }

        // 新チーム発足後インターバルを設ける
        if (EnemyMana.GetNewTeamIntavalTime() <= IntavalTime || collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() <= IntavalTime)
        {
            return;
        }

        Debug.Log("FallHit");
        if (animator.GetBool("fall"))
        {
            Debug.Log("玉合体");
            isFallBall = true;

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
        string guestName = collision.GetComponent<N_EnemyFallHit>().GetManagerName();

        // 同じ名前のマネージャーに所属していたら合体しない
        if (hostName == guestName || hostName == "ManagerNull" || guestName == "ManagerNull")
        {
            return;
        }

        // 新チーム発足後インターバルを設ける
        if (EnemyMana.GetNewTeamIntavalTime() <= IntavalTime || collision.GetComponent<N_EnemyFallHit>().GetNewTeamIntavalTime() <= IntavalTime)
        {
            return;
        }

        if (animator.GetBool("fall"))
        {
            Debug.Log("玉合体");
            isFallBall = true;
            
            collision.gameObject.transform.parent.gameObject.GetComponent<S_EnemyBall>().SetFallHitChangeBall(Parent);
        }
    }

    private float GetNewTeamIntavalTime()
    {
        return EnemyMana.GetNewTeamIntavalTime();
    }
}
