using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    ///B�{�^���������ꂽ���ǂ���
    private bool isBDown = false;

    //�G�����E���ɂ��邩�ǂ���
    private bool isSerch = false;

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

    private Collider2D colEnemy;
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
        isBDown = Input.GetKeyDown(KeyCode.C);//�����ŃL�[�w�肵�Ă�
        Debug.Log("�{�^��������=" + isBDown);
        if (isBDown&&isSerch) 
        {
            srEnemy = colEnemy.GetComponent<SpriteRenderer>();
            EnemySprite = srEnemy.sprite;
            StartCoroutine(Change());
        }
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        //�G�ɐڐG���Ă���΂��̓G�̖Ӗڏ�Ԃƌ����ڂ̏����擾
        if(_collision.tag=="Enemy")
        {
            M_BlindingMove blindingMove =_collision.GetComponent<M_BlindingMove>();
            isSerch=blindingMove.GetIsBlinding();
            colEnemy = _collision;
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        isSerch = false;
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
