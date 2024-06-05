using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TimeContoroller : MonoBehaviour
{
    [Header("�x������l"), SerializeField]
    private float slowTime = 0.1f;

    [Header("���������̃A�N�V������"), SerializeField]
    private string actionName = "EnemyPush";

    private GameObject camera;
    private Camera cameraCom;

    [Header("�J�����̃Y�[������"), SerializeField]
    private float camZoom = 1.0f;

    private float time = 0.0f;

    //�Y�[�����邩�t���O
    private bool isZoom = false;
    private bool isReverse = false;

    //�G��Ă��邩
    private bool isTouch = false;

    //�����J�����Y�[������
    private float initZoom;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera"); 
        cameraCom = camera.GetComponent<Camera>();

        time = 0.0f;

        initZoom = cameraCom.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        //����̍s���������Ƃ��ɏ���
        if(Input.GetAxis(actionName) > 0.5 && isTouch)
        {
            Time.timeScale = 1.0f;
            time = 0.0f;

            Destroy(this.gameObject);
            isReverse = true;
        }

        //�Y�[������
        if(isZoom)
        {
            if (camZoom < time)
            {
                time = camZoom;
                isZoom = false;                 
            }

            cameraCom.orthographicSize = cameraCom.orthographicSize - time;
            time += Time.deltaTime;            
        }       

        //���]����
        if(isReverse)
        {
            if (camZoom < time || initZoom > cameraCom.orthographicSize)
            {
                time = camZoom;
                isReverse = false;
                cameraCom.orthographicSize = initZoom;
            }

            cameraCom.orthographicSize = cameraCom.orthographicSize + time;
            time += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = slowTime;                               

            isZoom = true;
            isTouch = true;

            initZoom = cameraCom.orthographicSize;
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 1.0f;
            isReverse = true;
            time = 0.0f;
        }
    }
}
