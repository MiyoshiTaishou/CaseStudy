using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class N_ProjectHologram : MonoBehaviour
{
    // 表示するホログラム
    enum HOLOGRAM_MODE
    {
        PLAYER,
        WALL,
        FLOOR,
        TRANS,
    }
    [Header("プロジェクターのモード"), SerializeField]
    HOLOGRAM_MODE mode = HOLOGRAM_MODE.PLAYER;

    [Header("ホログラムの登録"), SerializeField]
    private GameObject[] gHolograms;

    // 表示する方向
    enum HOLOGRAM_DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    [Header("ホログラムを生成する方向"), SerializeField]
    HOLOGRAM_DIRECTION HoloDirection = HOLOGRAM_DIRECTION.UP;

    [Header("どれくらい離れた位置に表示するか"), SerializeField]
    private Vector2 AwayDistance = new Vector2(0.0f,0.0f);

    [Header("いくつ連ねるか"), SerializeField]
    private int iHowMany = 1;

    [Header("自動でマスに合わせる"), SerializeField]
    private bool AutoCombine = false;

    [Header("プロジェクターのオンオフ"), SerializeField]
    private bool isProjection = false;

    // 表示するホログラムの設定を途中で変える
    [Header("表示するホログラムの設定を途中で変える"), SerializeField]
    private bool isReset = false;

    [Header("共鳴エフェクト"), SerializeField]
    private GameObject MeowingPrefab;
    private GameObject MeowingObj;

    [Header("共鳴音"), SerializeField]
    private AudioClip audioclip;

    [Header("壁の根元スプライト"), SerializeField]
    private Sprite rootSprite_1;

    [Header("壁の根元スプライト"), SerializeField]
    private Sprite rootSprite_2;

    [Header("プレイヤーブラベリスプライト"), SerializeField]
    private Sprite PlayerSprite;

    [Header("壁ブラベリスプライト"), SerializeField]
    private Sprite WallSprite;

    [Header("床ブラベリスプライト"), SerializeField]
    private Sprite FloorSprite;

    //[Header("プロジェクター起動時に出す演出"), SerializeField]
    //private GameObject projectionUI;

    public bool GetProjection() { return isProjection; }

    private bool isActive = false;

    private GameObject Prefab;

    private Transform trans_Projecter;

    // 生成したホログラム
    private List<GameObject> Hologram = new List<GameObject>();

    // 一度の共鳴でオンオフが切り替わるのは一回
    private bool isAlreadySwitch = false;
    
    /// <summary>
    /// 時間計測by三好大翔
    /// </summary>
    private float fTime = 0.0f;

    private N_SetColliderOffSet sc_col;

    private bool isInit = false;

    //プレイヤーホログラム初期位置
    Vector2 InitHoloPos;

    private GameObject Sprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // トランスフォーム取得
        trans_Projecter = this.gameObject.transform;

        //// インスペクターでマスに合わせると指定した時
        //if (AutoCombine)
        //{
        //    // 置きなおし
        //    Replacement();
        //}

        //sc_col = GetComponent<N_SetColliderOffSet>();

        //// 描画に必要な情報をセット
        //SetInfomation(mode, HoloDirection);

        // ホログラム生成
        //GenerateHologram();

        // projectionUI.SetActive(false);
    }

    void Update()
    {

        // 初期化されてなければ
        if (!isInit)
        {
            Sprite = transform.GetChild(0).gameObject;
            spriteRenderer = Sprite.GetComponent<SpriteRenderer>();

            // インスペクターでマスに合わせると指定した時
            if (AutoCombine)
            {
                // 置きなおし
                Replacement();
            } 

            sc_col = GetComponent<N_SetColliderOffSet>();

            // 描画に必要な情報をセット
            SetInfomation(mode, HoloDirection);

            // ホログラム生成
            GenerateHologram();

            isInit = true;

            //Debug.Log("プロジェクター初期化");
        }

        if (isReset)
        {
            // 生成したホログラム削除
            foreach (var obj in Hologram)
            {
                Destroy(obj);
            }
            Hologram.Clear();

            // 描画に必要な情報をセット
            SetInfomation(mode, HoloDirection);

            // ホログラム生成
            GenerateHologram();


            isActive = false;
            isReset = false;
        }
        if (isProjection)
        {
            if (isActive == false)
            {
                foreach (GameObject obj in Hologram)
                {
                    fTime = 0.0f;
                    obj.SetActive(true);

                    if(mode==HOLOGRAM_MODE.PLAYER)
                    {
                        obj.transform.position = InitHoloPos;

                        // 消えたホログラムを見えるようにする
                        obj.GetComponent<N_HoloPlayerDestroy>().OnAlpha();
                    }
                }
                isActive = true;

                // 当たり判定のオブジェクトをアクティブに
                sc_col.SetActive(true);

                if (MeowingPrefab)
                {
                    MeowingObj = Instantiate(MeowingPrefab, Sprite.transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
            }

            foreach (GameObject obj in Hologram)
            {
                SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>(true); // 子オブジェクトのSpriteRendererを取得（trueを指定して非アクティブなものも含める）

                foreach (SpriteRenderer renderer in spriteRenderers)
                {
                    Material material = renderer.material; // 子オブジェクトのマテリアルを取得
                    if (material.HasProperty("_Fader")) // マテリアルが_Faderプロパティを持っているか確認
                    {
                        fTime += Time.deltaTime;
                        fTime = Mathf.Clamp01(fTime); // 値を0から1の範囲に制限する
                        material.SetFloat("_Fader", fTime); // _Faderを設定
                                                            // projectionUI.GetComponent<SpriteRenderer>().material.SetFloat("_Fader", fTime); // _Faderを設定
                    }
                }
            }
        }
        else
        {
            if (isActive == true)
            {
                foreach (GameObject obj in Hologram)
                {
                    obj.SetActive(false);
                    // projectionUI.SetActive(false);
                }
                if (MeowingPrefab)
                {
                    MeowingObj = Instantiate(MeowingPrefab, Sprite.transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
                }
                isActive = false;

                // 当たり判定のオブジェクトを非アクティブに
                sc_col.SetActive(false);
            }
        }
    }



    private void GenerateHologram()
    {
        Vector3 vec = trans_Projecter.position;
        float dirX = 1.0f;
        float dirY = 1.0f;

        Vector3 sca = Prefab.transform.localScale;    

        // ホログラムの開始地点調整
        switch (HoloDirection)
        {
            case HOLOGRAM_DIRECTION.DOWN:
                dirY = -dirY;
                break;

            case HOLOGRAM_DIRECTION.LEFT:
                dirX = -dirX;
                break;
        }
        vec.x = vec.x + AwayDistance.x * dirX;
        vec.y = vec.y + AwayDistance.y * dirY;

        // 当たり判定用
        Vector3 size = Vector3.zero;
        
        switch (HoloDirection)
        {
            case HOLOGRAM_DIRECTION.UP:
            case HOLOGRAM_DIRECTION.DOWN:
                dirX = 0.0f;

                break;

            case HOLOGRAM_DIRECTION.LEFT:
            case HOLOGRAM_DIRECTION.RIGHT:
                dirY = 0.0f;

                break;
        }

        for (int i = 0; i < iHowMany ; i++) 
        {
            Vector3 newVec = new Vector3(
                vec.x + dirX * sca.x * i,
                vec.y + dirY * sca.y * i,
                -1.0f
                );

            //Debug.Log(newVec);

            // インスタンス生成
            GameObject obj = Instantiate(Prefab, newVec, Quaternion.identity);
            // 削除されないホログラムにする
            obj.GetComponent<N_DestroyTimer>().SetBoolDestroy(false);
            // 自身の子オブジェクトにする
            obj.transform.parent = this.gameObject.transform;           
            // 最初非表示
            obj.SetActive(false);
            obj.name = Prefab.name;

            InitHoloPos = obj.transform.position;
            // リスト追加
            Hologram.Add(obj);

            
        }

        //壁の場合根元だけスプライト変更
        SpriteRenderer renderer;
        switch (mode)
        {
            case HOLOGRAM_MODE.WALL:
                switch (HoloDirection)
                {
                    case HOLOGRAM_DIRECTION.UP:
                        renderer = Hologram[0].GetComponent<SpriteRenderer>();
                        renderer.sprite = rootSprite_1;
                        renderer = Hologram[1].GetComponent<SpriteRenderer>();
                        renderer.sprite = rootSprite_2;
                        break;

                    case HOLOGRAM_DIRECTION.DOWN:
                        renderer = Hologram[iHowMany-1].GetComponent<SpriteRenderer>();
                        renderer.sprite = rootSprite_1;
                        renderer = Hologram[iHowMany - 2].GetComponent<SpriteRenderer>();
                        renderer.sprite = rootSprite_2;

                        break;
                }
                break;
        }

        // 当たり判定系
        Vector2 offset = Vector2.zero;

        sca.x /= transform.localScale.x;
        sca.y /= transform.localScale.y;
        sca.z /= transform.localScale.z;

        Vector2 away = AwayDistance;
        away.x /= transform.localScale.x;
        away.y /= transform.localScale.y;

        switch (HoloDirection)
        {
            case HOLOGRAM_DIRECTION.UP:
                size = new Vector3(sca.x, sca.y * iHowMany, sca.z);
                offset.x += away.x;
                offset.y += away.y + sca.y * iHowMany / 2 - sca.y / 2;
                break;

            case HOLOGRAM_DIRECTION.DOWN:
                size = new Vector3(sca.x, sca.y * iHowMany, sca.z);
                offset.x += away.x;
                offset.y -= away.y + sca.y * iHowMany / 2 - sca.y / 2;
                break;

            case HOLOGRAM_DIRECTION.LEFT:
                size = new Vector3(sca.x * iHowMany, sca.y, sca.z);
                offset.x += away.x + sca.x * iHowMany / 2 + sca.x / 2 - sca.x;
                offset.y += away.y;
                break;

            case HOLOGRAM_DIRECTION.RIGHT:
                size = new Vector3(sca.x * iHowMany, sca.y, sca.z);
                offset.x += away.x + sca.x * iHowMany / 2 + sca.x / 2 - sca.x;
                offset.y += away.y;
                break;
        }

        sc_col.SetOffSet(size, offset);

        //N_DebugDisplay.pos = sc_col.transform.position;
        //N_DebugDisplay.size = size;
        //N_DebugDisplay.offset = offset;

    }

    // タイルのマスにあうように座標をセットする
    private void Replacement()
    {
        Vector3 vec = trans_Projecter.position;
        Vector2 sca = trans_Projecter.localScale;

        //Debug.Log(gameObject.name);
        //Debug.Log(vec);

        vec.x = Mathf.FloorToInt(vec.x) + 0.5f/*sca.x / 2.0f*/;
        vec.y = Mathf.FloorToInt(vec.y) + 0.5f/*sca.y / 2.0f*/;

        trans_Projecter.position = vec;

        //Debug.Log(vec);
    }

    // 必要な情報をセットする
    private void SetInfomation(HOLOGRAM_MODE _mode,HOLOGRAM_DIRECTION _direction)
    {
        switch (_mode)
        {
            case HOLOGRAM_MODE.PLAYER:
                iHowMany = 1;
                // 当たり判定が必要か
                sc_col.SetIsColliding(false);
                // プレイヤーのスプライトにする
                spriteRenderer.sprite = PlayerSprite;
                Sprite.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
                break;

            case HOLOGRAM_MODE.WALL:
                sc_col.SetIsColliding(true);
                // 壁用のスプライトにする
                spriteRenderer.sprite = WallSprite;
                Sprite.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
                break;

            case HOLOGRAM_MODE.FLOOR:
                sc_col.SetIsColliding(true);
                // 床用のスプライトにする
                spriteRenderer.sprite = FloorSprite;
                Sprite.transform.localPosition = new Vector3(0.55f, 0.3f, 0.0f);
                break;

            case HOLOGRAM_MODE.TRANS:
                sc_col.SetIsColliding(false);
                break;
        }

        sc_col.SetActive(isProjection);

        Prefab = gHolograms[(int)_mode];

        switch (_direction)
        {
            case HOLOGRAM_DIRECTION.LEFT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                break;

            case HOLOGRAM_DIRECTION.RIGHT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                spriteRenderer.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                break;
        }
    }

    // プレイヤーの共鳴範囲内に入ったか出たか判定
    public void CheckAreaSympathy(Vector3 _pos ,float _radius)
    {

        // プレイヤーから自身までのベクトルを求める
        Vector3 vec = trans_Projecter.position - _pos;

        // ベクトルの長さを求める
        float len = vec.magnitude;

        // 共鳴の半径と比較
        if(len < _radius && isAlreadySwitch == false)
        {
            isProjection = !isProjection;
            isAlreadySwitch = true;
        }
    }

    public void SetOnOff(bool _OnOff)
    {
        isProjection = _OnOff;
    }

    public void Initialize()
    {
        isAlreadySwitch = false;
    }
}
