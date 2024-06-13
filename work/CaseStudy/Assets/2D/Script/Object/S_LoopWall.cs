using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LoopWall : MonoBehaviour
{
    [Header("���[�v��̃I�u�W�F�N�g"), SerializeField]
    private GameObject warpObj;

    bool isWarped = false;
    public bool GetisWarped() { return isWarped; }

    [Header("�E���ɏo��H"), SerializeField]
    private bool iswarpRight = false;

    [Header("���[�v���̉�"), SerializeField]
    private AudioClip audioclip = null;

    private AudioSource audioSource = null;

    //���[�v���ɉE���ɏo�邩���擾
    public bool GetiswarpRight() { return iswarpRight; }

    GameObject ColObject = null;
    float speedx = 0;

    /// <summary>
    /// ���x��ۑ����邽�߂̕ϐ�
    /// </summary>
    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        if (!warpObj.GetComponent<S_LoopWall>())
        {
            Debug.LogError("���[�v��ɂ��̃X�N���v�g���Ȃ�����ł�");
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���蔲�����ɕ��̂̑��x��ۑ�

        Vector2 vel2 = collision.GetComponent<Rigidbody2D>().velocity;
        vel = vel2;
        speedx = vel2.x;
        Debug.Log("���ҁ[�[�[�[�[�[�[�[�[��" + speedx);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���[�v�悪���[�v�o�����Ԃ�
        bool OK = warpObj.GetComponent<S_LoopWall>().GetisWarped();

        //���g�����[�v�ł��A���[�v�悪���[�v�ł��A�^�O���v���C���[���G�l�~�[�̎��Ƀ��[�v����
        if (isWarped == false && OK == false &&
            (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy")))
        {
            //��莞�ԃ��[�v�s�̏�Ԃɂ���
            StartCoroutine(CoolTime());
            
            //���������̂��G�Ȃ烏�[�v�����Ƃ�������t�^
            if(collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<SEnemyMove>().SetisWarped(true);
            }

            //���[�v��̈ʒu
            Vector3 newpos = warpObj.transform.position;

            //�E���ɏo�邩�����ɏo�邩�A�ʒu�������
            if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == true)
            {
                newpos.x += 2.0f;
            }
            else if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == false)
            {
                newpos.x -= 2.0f;
            }
            audioSource.PlayOneShot(audioclip);
            //���[�v
            collision.gameObject.transform.position = newpos;
        }
        //�G�ʂ̏ꍇ(���Ԃ��Ă݂�Ε�����K�v������������)
        else if (isWarped == false && OK == false &&
            collision.collider.CompareTag("EnemyBall"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            //speedx = rb.velocity.x;
            //��莞�ԃ��[�v�s�̏�Ԃɂ���
            StartCoroutine(CoolTime());

            //���[�v��̈ʒu�̐ݒ�Ɣ�����
            Vector3 newpos = warpObj.transform.position;
            if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == true)
            {
                newpos.x += 2.0f;
            }
            else if (warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == false)
            {
                newpos.x -= 2.0f;
            }
            audioSource.PlayOneShot(audioclip);
            collision.gameObject.transform.position = newpos;

            //vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�G�ʂ����ꂽ�ꍇ�ɕۑ����Ă������x���Ăї^����
        if (collision.collider.CompareTag("EnemyBall"))
        {
            Debug.Log("������");
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            vel.x = speedx;
            rb.velocity = vel;
            collision.gameObject.GetComponent<S_EnemyBall>().SetisPushing(true);
        }
    }

    IEnumerator CoolTime()
    {
        isWarped = true;
        warpObj.GetComponent<S_LoopWall>().isWarped = true;
        //�w��̃t���[���҂�
        yield return new WaitForSecondsRealtime(0.2f);
        Debug.Log("��������");
        isWarped = false;
        warpObj.GetComponent<S_LoopWall>().isWarped = false;
        //if(ColObject.CompareTag("EnemyBall"))
        //{
        //    Rigidbody2D rb = ColObject.GetComponent<Rigidbody2D>();
        //    Vector2 vel = rb.velocity;
        //    vel.x = speedx;
        //    rb.velocity = vel;
        //}
    }
}
