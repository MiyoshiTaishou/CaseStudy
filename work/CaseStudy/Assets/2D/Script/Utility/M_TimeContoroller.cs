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

    [Header("カメラのズーム時間"), SerializeField]
    private float camZoom = 1.0f;

    [Header("ズーム倍率"), SerializeField]
    private float zoomRatio = 1.0f;

    public float time = 0.0f;

    //ズームするかフラグ
    private bool isZoom = false;
    private bool isReverse = false;

    //触れているか
    private bool isTouch = false;

    //初期カメラズーム距離
    public float initZoom;

    /// <summary>
    /// プレイヤー
    /// </summary>
    private GameObject PlayerObj;

    private bool init = false;

    private bool isFunction = true;

    private bool isFinish = false;

    private bool wasPushButtonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            camera = GameObject.FindWithTag("MainCamera");
            cameraCom = camera.GetComponent<Camera>();

            panel = GameObject.Find("Tutorial_Panel");

            time = 0.0f;

            initZoom = cameraCom.orthographicSize;

            PlayerObj = GameObject.Find("Player");
            PlayerObj.transform.GetChild(0).GetComponent<M_PlayerPush>().SetOKPush(false);

            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            init = true;
        }
        bool isPushButtonPressed = Input.GetAxis(actionName) > 0.5f;

        if (!isFinish && isTouch)
        {
            bool pushOK = panel.GetComponent<M_ControllerAnimation>().GetStartPush();

            // チュートリアルのパネルがloopになっているか
            if (pushOK)
            {
                // 風を生み出せるようにする
                PlayerObj.transform.GetChild(0).GetComponent<M_PlayerPush>().SetOKPush(true);
            }

            //特定の行動をしたときに消す
            if (isPushButtonPressed && !wasPushButtonPressed && isTouch && pushOK)
            {
                Time.timeScale = 1.0f;
                isReverse = true;
                time = 0.0f;

                isTouch = false;
                // スロウの機能をoffにする
                isFunction = false;

                panel.GetComponent<M_ControllerAnimation>().SetPushBool(true);
                isReverse = true;

                PlayerObj.GetComponent<M_PlayerMove>().enabled = true;
                //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
                PlayerObj.GetComponent<N_ProjecterSympathy>().enabled = true;

                isFinish = true;           
            }
        }

        //ズーム処理
        if(isZoom)
        {
            if (time > camZoom)
            {
                time = 0.0f;

                //cameraCom.orthographicSize = initZoom - camZoom;
                isZoom = false;                 
            }

            cameraCom.orthographicSize = cameraCom.orthographicSize - Time.deltaTime * zoomRatio;
            time += Time.deltaTime;
        }       

        //反転処理
        if(isReverse)
        {
            cameraCom.orthographicSize = cameraCom.orthographicSize + Time.deltaTime * zoomRatio;
            time += Time.deltaTime;
            if (time > camZoom)
            {
                time = 0.0f;
                cameraCom.orthographicSize = initZoom;
                isReverse = false;
            }
        }

        wasPushButtonPressed = isPushButtonPressed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFunction)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            Time.timeScale = slowTime;                               

            isZoom = true;
            isTouch = true;

            initZoom = cameraCom.orthographicSize;

            panel.GetComponent<M_ControllerAnimation>().SetPushBool(false);
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;  // 動きとめる
            PlayerObj.GetComponent<M_PlayerMove>().enabled = false;
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().enabled = false;
        }
    }
}
