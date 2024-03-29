using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    ///�{�^���������ꂽ���ǂ���
    private bool isButtonDown = false;

    //�G�����E���ɂ��邩�ǂ���
    private bool isSerch = false;

    [Header("KeyType"),SerializeField]
    KeyCode playerChangeKey = KeyCode.C;

    [Header("�ϐg����"), SerializeField]
    float fWaitTime = 10.0f;

    //�ϐg�����ǂ���
    private bool isChanging = false;
    bool GetisChanging() { return isChanging; }

    //�ϐg�O�̌�����(Player)
    private Sprite PlayerSprite;

    //�ϐg��̌�����(Enemy)
    private Sprite EnemySprite;

    private SpriteRenderer srEnemy = null;

    private SpriteRenderer srPlayer = null;

    //�ϐg�̓����蔻����ɂ���G�̃��X�g
    private List<GameObject> colList= new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        srPlayer = transform.root.GetComponent<SpriteRenderer>();
        if(!srPlayer)
        {
            Debug.LogError("SpriteRenderer������܂���");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        //�ϐg�{�^���������ꂽ���ǂ�������
        isButtonDown = Input.GetKeyDown(playerChangeKey);
        Debug.Log("�{�^��������=" + isButtonDown);
        
        if (isButtonDown &&colList.Count != 0) 
        {
            Debug.Log("�ւ񂵂񂵂���");
            //�ϐg�̏����𖞂����Ă���G�̒��ň�ԋ������߂��G��T��
            float distance = 0.0f;
            for(int i=0;i< colList.Count;i++)
            {
                float temp = Vector2.Distance(transform.root.position , colList[i].transform.position);
                if (distance > temp||distance == 0.0f) 
                {
                    distance = temp;
                    srEnemy = colList[i].GetComponent<SpriteRenderer>();
                }
            }
            EnemySprite = srEnemy.sprite;
            StartCoroutine(Change());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //�ڐG�������̂��G���߂���܂���Ԃł���΃��X�g�ɒǉ�
        if(other.tag=="Enemy")
        {
            M_BlindingMove blindingMove = other.GetComponent<M_BlindingMove>();
            isSerch = blindingMove.GetIsBlinding();

            //���X�g�ɒǉ��������Ƃ��Ȃ����̂������X�g�ɒǉ�
            GameObject collidedObject = other.gameObject;
            if (!colList.Contains(collidedObject) && isSerch)
            {
                colList.Add(collidedObject);
                Debug.Log("Object�ǉ�"+colList.Count);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //���X�g�����菜��
        GameObject collidedObject = other.gameObject;
        if(colList.Contains(collidedObject))
        {
            colList.Remove(collidedObject);
        }
    }

    //�R���[�`���B
    IEnumerator Change()
    {
        //�I���܂ő҂��Ăق�������������
        //�ϐg���łȂ���΃v���C���[�ƓG�̌����ڂ����ւ���A�ϐg���ł���Εϐg���Ԃ����Z�b�g���邾��
        if (!isChanging)
        {
            //���݂̏󋵂ɉ����ă^�O�ƌ����ڂ�ύX
            transform.root.tag = "Enemy";
            PlayerSprite = srPlayer.sprite;
            //�����ڂ̕ύX
            srPlayer.sprite = EnemySprite;
            srEnemy.sprite = PlayerSprite;
            isChanging= true;
        }
        //�w��̕b���҂�
        yield return new WaitForSeconds(fWaitTime);
        //�ĊJ���Ă�����s����������������

        //�����ڂ����ɖ߂�
        srPlayer.sprite = PlayerSprite;
        srEnemy.sprite = EnemySprite;
        Debug.Log("�ϐg����");
        transform.root.tag = "Player";
        isChanging= false;
    }
}
