using System.Collections;
using System.Collections.Generic;
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
    [Header("プロジェクターの向き"), SerializeField]
    HOLOGRAM_DIRECTION direction = HOLOGRAM_DIRECTION.UP;

    [Header("どれくらい離れた位置に表示するか"), SerializeField]
    private float fDistance = 1.0f;

    [Header("いくつ連ねるか"), SerializeField]
    private int iHowMany = 1;

    [Header("プロジェクターのオンオフ"), SerializeField]
    private bool isProjection = false;

    [Header("プロジェクター起動時に出す演出"), SerializeField]
    private GameObject projectionUI;

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

    // Start is called before the first frame update
    void Start()
    {
        // トランスフォーム取得
        trans_Projecter = this.gameObject.transform;

        // 置きなおし
        Replacement();

        // 描画に必要な情報をセット
        SetInfomation(mode, direction);

        // ホログラム生成
        GenerateHologram();

        projectionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isProjection)
        {
            if(isActive == false)
            {
                foreach (GameObject obj in Hologram)
                {
                    fTime = 0.0f;
                    obj.SetActive(true); 
                    projectionUI.SetActive(true);
                }
                isActive = true;
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
                    }
                }
            }

            projectionUI.GetComponent<SpriteRenderer>().material.SetFloat("_Fader", fTime); // _Faderを設定
        }
        else
        {
            if(isActive == true)
            {
                foreach (GameObject obj in Hologram)
                {                   
                    obj.SetActive(false);  
                    projectionUI.SetActive(false);
                }
                isActive = false;
            }
        }
    }



    private void GenerateHologram()
    {
        Vector3 vec = trans_Projecter.position;
        float dirX = 1.0f;
        float dirY = 1.0f;
        Vector3 sca = Prefab.transform.localScale;

        switch (direction)
        {
            case HOLOGRAM_DIRECTION.UP:
                vec.y = vec.y + fDistance;
                dirX = 0.0f;
                break;

            case HOLOGRAM_DIRECTION.DOWN:
                vec.y = vec.y - fDistance;
                dirX = 0.0f;
                dirY = -dirY;
                break;

            case HOLOGRAM_DIRECTION.LEFT:
                vec.x = vec.x - fDistance;
                dirX = -dirX;
                dirY = 0.0f;
                break;

            case HOLOGRAM_DIRECTION.RIGHT:
                vec.x = vec.x + fDistance;
                dirY = 0.0f;
                break;
        }

        for (int i = 0; i < iHowMany ; i++) 
        {
            Vector3 newVec = new Vector3(
                vec.x + dirX * sca.x * i,
                vec.y + dirY * sca.y * i,
                vec.z
                );

            // インスタンス生成
            GameObject obj = Instantiate(Prefab, newVec, Quaternion.identity);
            // 削除されないホログラムにする
            obj.GetComponent<N_DestroyTimer>().SetBoolDestroy(false);
            // 自身の子オブジェクトにする
            obj.transform.parent = this.gameObject.transform;
            // 最初非表示
            obj.SetActive(false);
            // リスト追加
            Hologram.Add(obj);          
        }

    }

    // タイルのマスにあうように座標をセットする
    private void Replacement()
    {
        Vector2 vec = trans_Projecter.position;
        Vector2 sca = trans_Projecter.localScale;

        vec.x = Mathf.FloorToInt(vec.x) + sca.x / 2.0f;
        vec.y = Mathf.FloorToInt(vec.y) + sca.y / 2.0f;

        trans_Projecter.position = vec;
    }

    // 必要な情報をセットする
    private void SetInfomation(HOLOGRAM_MODE _mode,HOLOGRAM_DIRECTION _direction)
    {
        string address = "";

        switch (_mode)
        {
            case HOLOGRAM_MODE.PLAYER:
                //address = "Assets/Object/Field/Hologram/Holo_Player.prefab";
                iHowMany = 1;
                break;

            case HOLOGRAM_MODE.WALL:
                //address = "Assets/Object/Field/Hologram/Holo_Wall.prefab";
                break;

            case HOLOGRAM_MODE.FLOOR:
                //address = "Assets/Object/Field/Hologram/Holo_Floor.prefab";
                break;
        }
#if UNITY_EDITOR

        // パスを元にプレハブを取得
        //Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(address);
#endif
        Prefab = gHolograms[(int)_mode];

        switch (_direction)
        {
            case HOLOGRAM_DIRECTION.UP:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                break;

            case HOLOGRAM_DIRECTION.DOWN:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                break;

            case HOLOGRAM_DIRECTION.LEFT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                break;

            case HOLOGRAM_DIRECTION.RIGHT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
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

    public void Initialize()
    {
        isAlreadySwitch = false;
    }
}
