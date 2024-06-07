using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LoopWall : MonoBehaviour
{
    [Header("���[�v��̃I�u�W�F�N�g"), SerializeField]
    private GameObject warpObj;

    bool isWarped = false;
    public bool GetisWarped() { return isWarped; }

    [Header("�E���ɏo��H"),SerializeField]
    private bool iswarpRight = false;

    public bool GetiswarpRight() { return iswarpRight; }
    // Start is called before the first frame update
    void Start()
    {
        if(!warpObj.GetComponent<S_LoopWall>())
        {
            Debug.LogError("���[�v��ɃX�N���v�g���Ȃ�����ł�");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool OK = warpObj.GetComponent<S_LoopWall>().GetisWarped();
        if( isWarped == false && OK == false &&
            (collision.collider.CompareTag("Player")|| collision.collider.CompareTag("Enemy")))
        {
            StartCoroutine(HitStop());
            Vector3 newpos = warpObj.transform.position;
            if(warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == true)
            {
                newpos.x += 1.0f;
            }
            else if(warpObj.GetComponent<S_LoopWall>().GetiswarpRight() == false)
            {
                newpos.x -= 1.0f;
            }
            collision.gameObject.transform.position=newpos;
        }
    }


    IEnumerator HitStop()
    {   
        isWarped = true;

        //�w��̃t���[���҂�
        yield return new WaitForSecondsRealtime(1);

        isWarped = false;
    }
}
