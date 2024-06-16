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
        //�v���n�u���̉�
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

        //Debug.Log(EnemyMove.GetIsReflection());

        // �������擾
        // ���̌����ɂ������X�P�[�����Z�b�g
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

        //Debug.Log("���A�N�V�������Z�b�g");
    }

    //�G����������
    void OnDestroy()
    {
        Destroy(EnemyQuestion);
        Destroy(EnemyFoundTarget);
    }
}
