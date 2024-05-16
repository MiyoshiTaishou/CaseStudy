using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemyReaction : MonoBehaviour
{
    [Header("�z���O�������m"), SerializeField]
    private GameObject EnemyReactionPrefab;

    [Header("�ǐՑΏ۔���"), SerializeField]
    private GameObject EnemyFoundPrefab;

    // ��,���z��
    private bool IsSearchHologram=false;

    // �v���C���[,�v���C���[�z��
    private bool IsSearchTarget = false;

    // �^�[�Q�b�g����������
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
        //�v���n�u���̉�
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
        //�z���O�������m������
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

    //�G����������
    void OnDestroy()
    {
        Destroy(EnemyQuestion);
        Destroy(EnemyFoundTarget);
    }
}
