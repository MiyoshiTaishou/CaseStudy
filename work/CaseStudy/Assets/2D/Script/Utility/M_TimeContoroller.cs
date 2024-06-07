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

    private GameObject panel;

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

    /// <summary>
    /// �v���C���[
    /// </summary>
    private GameObject PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera"); 
        cameraCom = camera.GetComponent<Camera>();

        panel = GameObject.Find("Tutorial_Panel");

        time = 0.0f;

        initZoom = cameraCom.orthographicSize;

        PlayerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //����̍s���������Ƃ��ɏ���
        if(Input.GetAxis(actionName) > 0.5 && isTouch)
        {
            Time.timeScale = 1.0f;
            time = 0.0f;

            panel.GetComponent<M_ControllerAnimation>().SetPushBool(true);
            isReverse = true;

            PlayerObj.GetComponent<M_PlayerMove>().enabled = true;
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().enabled = true;

            Destroy(this.gameObject);            
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

            panel.GetComponent<M_ControllerAnimation>().SetPushBool(false);

            PlayerObj.GetComponent<M_PlayerMove>().enabled = false;
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().enabled = false;
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
