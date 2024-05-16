using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyReaction : MonoBehaviour
{
    [Header("ホログラム検知"), SerializeField]
    private GameObject EnemyReactionPrefab;

    [Header("追跡対象発見"), SerializeField]
    private GameObject EnemyFoundPrefab;

    // 壁,床ホロ
    private bool IsSearchHologram=false;

    // プレイヤー,プレイヤーホロ
    private bool IsSearchTarget = false;

    // ターゲットを見失った
    private bool IsTargetLost = false;

    private GameObject EnemyQuestion;

    private GameObject EnemyFoundTarget;

    private SEnemyMove EnemyMove;

    public void SetIsSearchHologram(bool _search)
    {
        IsSearchHologram = _search;
    }

    public void SetIsSearchTarget(bool _search)
    {
        IsSearchTarget = _search;
    }
    public void SetIsLostTarget(bool _search)
    {
        IsTargetLost = _search;
    }

    // Start is called before the first frame update
    void Start()
    {
        //プレハブ実体化
        EnemyQuestion = Instantiate(EnemyReactionPrefab, transform.position, Quaternion.identity);
        EnemyQuestion.SetActive(false);
        EnemyQuestion.transform.parent = gameObject.transform;

        EnemyFoundTarget = Instantiate(EnemyFoundPrefab, transform.position, Quaternion.identity);
        EnemyFoundTarget.SetActive(false);
        EnemyFoundTarget.transform.parent = gameObject.transform;

        EnemyMove = GetComponent<SEnemyMove>();
    }

    private void Update()
    {
        //IsSearchHologram = EnemyMove.GetIsCollidingHologram();
        //ホログラム検知したら
        if (IsSearchHologram || IsTargetLost)
        {
            EnemyQuestion.SetActive(true);
            Vector3 ReactionPos = this.transform.position;
            ReactionPos.y += 3.0f;
            EnemyQuestion.transform.position = ReactionPos;
        }
        else
        {
            EnemyQuestion.SetActive(false);
        }

        if (IsSearchTarget)
        {
            EnemyFoundTarget.SetActive(true);
            Vector3 ReactionPos = this.transform.position;
            ReactionPos.y += 3.0f;
            EnemyFoundTarget.transform.position = ReactionPos;
        }
        else
        {
            EnemyFoundTarget.SetActive(false);
        }
    }

    public void AllSetFalse()
    {
        IsSearchTarget = false;
        EnemyQuestion.SetActive(false);
        EnemyFoundTarget.SetActive(false);
    }

    //敵が消えたら
    void OnDestroy()
    {
        Destroy(EnemyQuestion);
        Destroy(EnemyFoundTarget);
    }
}
