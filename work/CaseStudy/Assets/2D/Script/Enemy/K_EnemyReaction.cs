using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyReaction : MonoBehaviour
{
    [Header("�G����"), SerializeField]
    private GameObject EnemyReactionPrefab;

    private bool IsSearch=false;

    private GameObject EnemyReaction;

    private SEnemyMove EnemyMove;

    // Start is called before the first frame update
    void Start()
    {
        //�v���n�u���̉�
        EnemyReaction = Instantiate(EnemyReactionPrefab, transform.position, Quaternion.identity);
        EnemyReaction.SetActive(false);

        EnemyMove = GetComponent<SEnemyMove>();
    }

    private void Update()
    {
        IsSearch = EnemyMove.GetIsCollidingHologram();
        //�z���O�������m������
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

    //�G����������
    void OnDestroy()
    {
        Destroy(EnemyReaction);
    }
}
