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

    private Transform TransQuestion;
    private Transform TransFound;

    private Vector2 InitScale_Que = Vector2.zero;
    private Vector2 InitScale_Fou = Vector2.zero;

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
        TransQuestion = EnemyQuestion.GetComponent<Transform>();
        InitScale_Que = TransQuestion.localScale;

        EnemyFoundTarget = Instantiate(EnemyFoundPrefab, transform.position, Quaternion.identity);
        EnemyFoundTarget.SetActive(false);
        EnemyFoundTarget.transform.parent = gameObject.transform;
        TransFound = EnemyQuestion.GetComponent<Transform>();
        InitScale_Fou = TransFound.localScale;


        EnemyMove = GetComponent<SEnemyMove>();
    }

    private void Update()
    {
        IsSearchHologram = EnemyMove.GetIsCollidingHologram();
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

        //Debug.Log(EnemyMove.GetIsReflection());

        // 向きを取得
        // その向きにあったスケールをセット
        if (EnemyMove.GetIsReflection())
        {
            TransQuestion.localScale = new Vector3(-InitScale_Que.x, InitScale_Que.y, 0.0f);
            TransFound.localScale = new Vector3(-InitScale_Fou.x, InitScale_Que.y, 0.0f);
        }
        else
        {
            TransQuestion.localScale = new Vector3(InitScale_Que.x, InitScale_Que.y, 0.0f);
            TransFound.localScale = new Vector3(InitScale_Fou.x, InitScale_Que.y, 0.0f);
        }
    }

    public void AllSetFalse()
    {
        IsSearchTarget = false;
        IsSearchHologram = false;
        IsTargetLost = false;
        EnemyQuestion.SetActive(false);
        EnemyFoundTarget.SetActive(false);

        //Debug.Log("リアクションリセット");
    }

    //敵が消えたら
    void OnDestroy()
    {
        Destroy(EnemyQuestion);
        Destroy(EnemyFoundTarget);
    }
}
