using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;

public class S_EnemyBall : MonoBehaviour
{
    [Header("�����W��"), SerializeField]
    float fBoost = 1.5f;

    [Header("AudioClip"), SerializeField]
    AudioClip audioclip;

    [Header("�q�b�g�X�g�b�v"), SerializeField]
    float fHitStop = 0;

    //������Ă��邩�ǂ���
    private bool isPushing = false;
    public bool GetisPushing() { return isPushing; }
    public void SetisPushing(bool _flg) { isPushing = _flg; }
    private GameObject ColObject;

    private float nStickCnt = 0;

    private Vector3 defaultScale;



    // Start is called before the first frame update
    void Start()
    {
        defaultScale= transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //�z�������G�̐��ɉ����ċ��剻
        Vector3 temp= defaultScale;
        temp.x -= nStickCnt / 5;
        temp.y += nStickCnt / 5;
        transform.localScale = temp;
        if(isPushing)
        {
            GetComponent<SEnemyMove>().enabled = false;
            GetComponent<M_BlindingMove>().enabled = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D _collision)
    {
        //���������I�u�W�F�N�g���G��������Ă��Ȃ���΋z��
        ColObject= _collision.gameObject;
        if(ColObject.tag=="Enemy")
        {
            if (!ColObject.GetComponent<S_EnemyBall>().GetisPushing())
            {
                nStickCnt++;
                Destroy(ColObject);
                GetComponent<Rigidbody2D>().AddForce(GetComponent<Rigidbody2D>().velocity*fBoost, ForceMode2D.Impulse);
                GetComponent<AudioSource>().PlayOneShot(audioclip);
                StartCoroutine(HitStop());
            }
        }
    }
    IEnumerator HitStop()
    {
        //���x��ۑ����A0�ɂ���
        Vector2 vel=GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //�w��̃t���[���҂�
        yield return new WaitForSeconds(fHitStop/60);
        //�ۑ��������x�ōĊJ����
        GetComponent<Rigidbody2D>().velocity = vel;
    }
}
