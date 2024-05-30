using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch : MonoBehaviour
{
    private N_EnemyManager enemyManager;
    private SEnemyMove enemyMove;
    private S_EnemyBall enemyBall;

    private GameObject Parent;

    [Header("����������(�R���C�_�[�̃T�C�Y�ȏ�)"), SerializeField]
    private float LostSightDistance = 6.0f;

    private Transform transTarget;

    [SerializeField]
    public bool isSearch = false;
    public bool GetIsSearch() { return isSearch; }

    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g�擾
        Parent = transform.parent.gameObject;
        enemyMove = Parent.GetComponent<SEnemyMove>();
        enemyBall = Parent.GetComponent<S_EnemyBall>();
        enemyManager = enemyMove.GetManager();
    }

    // Update is called once per frame
    void Update()
    {
        //�ʏ�ԂȂ�ǐՏ�Ԃ�����
        if(enemyBall.GetisBall())
        {
            isSearch = false;
            enemyManager = null;
        }
        else
        {
            if (enemyManager == null)
            {
                enemyManager = enemyMove.GetManager();
            }

            // �����Ă���Ƃ��̂݌��������߂̌v�Z���s
            if (isSearch)
            {
                // �^�[�Q�b�g�Ƃ̋������v�Z
                Vector2 vec = transform.position - transTarget.position;

                float dis = vec.x * vec.x + vec.y * vec.y;

                if (dis > LostSightDistance * LostSightDistance)
                {
                    isSearch = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("����");
                transTarget = collision.gameObject.transform;
                enemyManager.SetTarget(collision.gameObject);
                isSearch = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                //Debug.Log("�ǐ�");
                enemyManager.SetTarget(collision.gameObject);

                isSearch = true;
            }
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Decoy"))
    //    {
    //        //enemyManager.ChangeManagerState(N_EnemyManager.ManagerState.PATOROL);
    //        isSearch = false;
    //        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!");
    //    }
    //}

    //private void OnDestroy()
    //{
    //    //Debug.Log("���ꂽ�I");
    //    isSearch = false;
    //}
}
