using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Rendering;
#endif
using UnityEngine;

public class S_EnemyBall : MonoBehaviour
{
    [Header("�����W��"), SerializeField]
    float fBoost = 1.5f;

    [Header("AudioClip"), SerializeField]
    AudioClip audioclip;

    [Header("�q�b�g�X�g�b�v"), SerializeField]
    float fHitStop = 0;

    [Header("�������x(x)"), SerializeField]
    float fLimitSpeedx = 15.0f;

    [Header("��~����"), SerializeField]
    float fStopjudge = 0.0f;

    [Header("�傫���̒i�K�ƕK�v�ȋz����"), SerializeField]
    int[] nGiantNum;

    [Header("�U������"), SerializeField]
    private float fTime = 0.1f;

    //��ɂȂ��Ă��邩�ǂ���
    private bool isBall = false;
    public bool GetisBall() { return isBall; }

    //�v���C���[�ɂ���ĉ�����Ă��邩�ǂ���
    private bool isPushing = false;
    public bool GetisPushing() { return isPushing; }
    public void SetisPushing(bool _flg) { isPushing = _flg; }

    private GameObject ColObject;

    private Vector3 defaultScale;

    //�������Ă����
    private float fStickCnt = 0;

    private Rigidbody2D rb;

    private Vector2 vel;

    public int GetStickCount() 
    {
        int temp = 0;
        temp = Mathf.FloorToInt(fStickCnt);
        return temp; }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale= transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isPushing)
        {
           GetComponent<SEnemyMove>().enabled = false;
           GetComponent<M_BlindingMove>().enabled = false;
           GetComponent<MPlayerSearch>().enabled = false;
            vel = rb.velocity;
        }
        if(isBall&& Mathf.Abs(rb.velocity.x) < fStopjudge) 
        {
            isPushing = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //���������I�u�W�F�N�g���G��������Ă��Ȃ���΋z��
        ColObject= _collision.gameObject;
        if(ColObject.tag=="Enemy")
        {
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing()||
                (ColObject.GetComponent<S_EnemyBall>().GetisPushing()&&fStickCnt > ColObject.GetComponent<S_EnemyBall>().GetStickCount()))
            {
                isBall = true;
                fStickCnt++;
                if(fStickCnt==1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //�z�������G�̐��ɉ����ċ��剻
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x -= GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Destroy(ColObject);
                rb.AddForce(vel*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                GetComponent<AudioSource>().pitch+=0.2f;

                StartCoroutine(M_Utility.GamePadMotor(fTime));
                StartCoroutine(HitStop());
            }
        }
    }
    private void OnCollisionStay2D(Collision2D _collision)
    {
        if(!isPushing)
        {
            return;
        }
        //���������I�u�W�F�N�g���G��������Ă��Ȃ���΋z��
        ColObject= _collision.gameObject;
        if(ColObject.tag=="Enemy")
        {
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing()||
                (ColObject.GetComponent<S_EnemyBall>().GetisPushing()&&fStickCnt > ColObject.GetComponent<S_EnemyBall>().GetStickCount()))
            {
                isBall = true;
                fStickCnt++;
                if (fStickCnt == 1)
                {
                    fStickCnt++;
                    transform.tag = "EnemyBall";
                }
                //�z�������G�̐��ɉ����ċ��剻
                Vector3 nextScale = defaultScale;
                float GiantLv = (float)GetGiantLv();
                nextScale.x -= GiantLv / 2;
                nextScale.y += GiantLv / 2;
                transform.localScale = nextScale;
                Destroy(ColObject);
                rb.AddForce(rb.velocity*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                StartCoroutine(HitStop());
            }
        }
    }
    IEnumerator HitStop()
    {
        //���x��ۑ����A0�ɂ���
        Vector2 vel=rb.velocity;
        if(vel.x>fLimitSpeedx)
        {
            vel.x = fLimitSpeedx;
        }
        else if(vel.x<-fLimitSpeedx)
        {
            vel.x = -fLimitSpeedx;
        }
        rb.velocity = Vector2.zero;
        //�w��̃t���[���҂�
        yield return new WaitForSeconds(fHitStop/60);
        //�ۑ��������x�ōĊJ����
        rb.velocity = vel;
        isPushing = true;
    }

    //���i�K���剻���������擾����֐�
    private int GetGiantLv()
    {
        int temp = Mathf.FloorToInt(fStickCnt);
        int[] array = nGiantNum;
        int lv = 0;
        int i = 0;
        while(i<nGiantNum.Length) 
        {
            temp -= nGiantNum[i];
            if(temp>=0)
            {
                lv++;
            }
            else if(temp<0)
            {
                break;
            }
            i++;
        }
        return lv;
    }
}
