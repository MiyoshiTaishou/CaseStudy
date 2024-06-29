using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerSearch : MonoBehaviour
{
    public N_EnemyManager enemyManager;
    private SEnemyMove enemyMove;
    private S_EnemyBall enemyBall;

    private GameObject Parent;

    [Header("����������(�R���C�_�[�̃T�C�Y�ȏ�)"), SerializeField]
    private float LostSightDistance = 6.0f;

    private Transform transTarget;
    private GameObject Target;

    [SerializeField]
    public bool isSearch = false;

    public bool isRaycast = false;

    public bool isCheck = false;

    public float elapsedTime = 0.0f;

    private bool init = false;

    [Header("���C���[�}�X�N�ݒ�"), SerializeField]
    private LayerMask layerMask;

    public bool GetIsSearch() { return isSearch; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            // �e�I�u�W�F�N�g�擾
            Parent = transform.parent.gameObject;
            enemyMove = Parent.GetComponent<SEnemyMove>();
            enemyBall = Parent.GetComponent<S_EnemyBall>();
            enemyManager = enemyMove.GetManager();

            init = true;
        }

        //�ʏ�ԂȂ�ǐՏ�Ԃ�����
        if (enemyBall.GetisBall())
        {
            isSearch = false;
            isRaycast = false;
            elapsedTime = 0.0f;
            enemyManager = null;
        }
        else
        {
            if (enemyManager == null)
            {
                enemyManager = enemyMove.GetManager();
            }

            // �ǐՑΏۂ�����͈͓��ɓ�������
            // �ǂ�����ł��邩�𔻒肷��
            if (isRaycast /*&& !isCheck*/)
            {
                RayCastCheck();
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
                    isRaycast = false;
                    elapsedTime = 0.0f;
                    Debug.Log("gomi");
                }

                if(Target.GetComponent<BoxCollider2D>().enabled == false)
                {
                    isSearch = false;
                    isRaycast = false;
                    elapsedTime = 0.0f;
                    isCheck = false;
                    Debug.Log("kuso");

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRaycast || isSearch)
        {
            return;
        }
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                Target = collision.gameObject;
                transTarget = Target.transform;

                //Debug.Log("�G���^�[");

                isRaycast = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (isRaycast || isSearch)
        {
            return;
        }
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                Target = collision.gameObject;
                transTarget = Target.transform;

                //Debug.Log("�G���^�[");

                isRaycast = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyManager != null)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Decoy"))
            {
                isRaycast = false;
                elapsedTime = 0.0f;

                isCheck = false;
            }
        }
    }

    // �v���C���[�����E�ɓ��������ɂ��̊Ԃɕǂ����邩�ǂ������f���郌�C���΂�
    private void RayCastCheck()
    {
        //Debug.Log("��ƃo�X");

        Vector3 startPoint = gameObject.transform.position;
        Vector2 direction = Vector2.right;
        if (enemyMove.GetIsReflection())
        {
            direction = Vector2.left;
        }
        float distance = elapsedTime * 30.0f;
        elapsedTime += Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction,distance,layerMask);

        Debug.DrawRay(startPoint, direction * distance, Color.black, 0.0f, false);

        // ��ɓG�ɓ���������ǐ�
        // �ǂɓ���������Ȃɂ��Ȃ�
        if (hit.collider != null)
        {
            Debug.Log("�q�b�g����");
            elapsedTime = 0.0f;
            isCheck = true;

            if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Decoy"))
            {
                Debug.Log("�ړG");
                isSearch = true;
                enemyManager.SetTarget(Target);
                //isRaycast = false;
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    S_Respawn.SetIsFounded(true);
                    //Debug.Log("�݂�����");
                }
                else
                {
                    S_Respawn.SetIsFounded(false);
                    //Debug.Log("�݂����ĂȂ�");
                }
            }
            else if(hit.collider.gameObject.CompareTag("Ground"))
            {
                Debug.Log("�ǌ��m");

                isSearch = false;
                //isRaycast = false;
                S_Respawn.SetIsFounded(false);
                //Debug.Log("�݂����ĂȂ�");
            }
            else
            {
                Debug.Log("�Ȃ񂩈Ⴄ����");
            }
        }
        else
        {
            Debug.Log("�q�b�g�Ȃ�");
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
