using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TimeContoroller : MonoBehaviour
{
    [Header("遅くする値"), SerializeField]
    private float slowTime = 0.1f;

    [Header("解除条件のアクション名"), SerializeField]
    private string actionName = "EnemyPush";

    private GameObject camera;
    private Camera cameraCom;

    private GameObject panel;

    [Header("カメラのズーム距離"), SerializeField]
    private float camZoom = 1.0f;

    private float time = 0.0f;

    //ズームするかフラグ
    private bool isZoom = false;
    private bool isReverse = false;

    //触れているか
    private bool isTouch = false;

    //初期カメラズーム距離
    private float initZoom;

    /// <summary>
    /// プレイヤー
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
        //特定の行動をしたときに消す
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

        //ズーム処理
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

        //反転処理
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
