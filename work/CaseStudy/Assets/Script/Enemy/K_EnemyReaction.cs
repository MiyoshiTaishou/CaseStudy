using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyReaction : MonoBehaviour
{
    [Header("敵反応"), SerializeField]
    private GameObject EnemyReactionPrefab;

    private bool IsSearch=false;

    private GameObject EnemyReaction;

    private SEnemyMove EnemyMove;

    // Start is called before the first frame update
    void Start()
    {
        //プレハブ実体化
        EnemyReaction = Instantiate(EnemyReactionPrefab, transform.position, Quaternion.identity);
        EnemyReaction.SetActive(false);

        EnemyMove = GetComponent<SEnemyMove>();
    }

    private void Update()
    {
        IsSearch = EnemyMove.GetIsCollidingHologram();
        //ホログラム検知したら
        if (IsSearch)
        {
            EnemyReaction.SetActive(true);
            Vector3 ReactionPos = this.transform.position;
            ReactionPos.y += 3.0f;
            EnemyReaction.transform.position = ReactionPos;
        }
        else
        {
            EnemyReaction.SetActive(false);
        }
    }

    //敵が消えたら
    void OnDestroy()
    {
        Destroy(EnemyReaction);
    }
}
