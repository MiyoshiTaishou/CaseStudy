using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class N_ProjectHologram : MonoBehaviour
{
    // �\������z���O����
    enum HOLOGRAM_MODE
    {
        PLAYER,
        WALL,
        FLOOR,
        TRANS,
    }
    [Header("�v���W�F�N�^�[�̃��[�h"), SerializeField]
    HOLOGRAM_MODE mode = HOLOGRAM_MODE.PLAYER;

    [Header("�z���O�����̓o�^"), SerializeField]
    private GameObject[] gHolograms;

    // �\���������
    enum HOLOGRAM_DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    [Header("�z���O�����𐶐��������"), SerializeField]
    HOLOGRAM_DIRECTION HoloDirection = HOLOGRAM_DIRECTION.UP;

    [Header("�ǂꂭ�炢���ꂽ�ʒu�ɕ\�����邩"), SerializeField]
    private Vector2 AwayDistance = new Vector2(0.0f,0.0f);

    [Header("�����A�˂邩"), SerializeField]
    private int iHowMany = 1;

    [Header("�����Ń}�X�ɍ��킹��"), SerializeField]
    private bool AutoCombine = false;

    [Header("�v���W�F�N�^�[�̃I���I�t"), SerializeField]
    private bool isProjection = false;

    // �\������z���O�����̐ݒ��r���ŕς���
    [Header("�\������z���O�����̐ݒ��r���ŕς���"), SerializeField]
    private bool isReset = false;

    [Header("���G�t�F�N�g"), SerializeField]
    private GameObject MeowingPrefab;
    private GameObject MeowingObj;

    [Header("����"), SerializeField]
    private AudioClip audioclip;

    [Header("�ǂ̍����X�v���C�g"), SerializeField]
    private Sprite rootSprite_1;

    [Header("�ǂ̍����X�v���C�g"), SerializeField]
    private Sprite rootSprite_2;

    [Header("�v���C���[�u���x���X�v���C�g"), SerializeField]
    private Sprite PlayerSprite;

    [Header("�ǃu���x���X�v���C�g"), SerializeField]
    private Sprite WallSprite;

    [Header("���u���x���X�v���C�g"), SerializeField]
    private Sprite FloorSprite;

    //[Header("�v���W�F�N�^�[�N�����ɏo�����o"), SerializeField]
    //private GameObject projectionUI;

    public bool GetProjection() { return isProjection; }

    private bool isActive = false;

    private GameObject Prefab;

    private Transform trans_Projecter;

    // ���������z���O����
    private List<GameObject> Hologram = new List<GameObject>();

    // ��x�̋��ŃI���I�t���؂�ւ��͈̂��
    private bool isAlreadySwitch = false;
    
    /// <summary>
    /// ���Ԍv��by�O�D����
    /// </summary>
    private float fTime = 0.0f;

    private N_SetColliderOffSet sc_col;

    private bool isInit = false;

    //�v���C���[�z���O���������ʒu
    Vector2 InitHoloPos;

    private GameObject Sprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // �g�����X�t�H�[���擾
        trans_Projecter = this.gameObject.transform;

        //// �C���X�y�N�^�[�Ń}�X�ɍ��킹��Ǝw�肵����
        //if (AutoCombine)
        //{
        //    // �u���Ȃ���
        //    Replacement();
        //}

        //sc_col = GetComponent<N_SetColliderOffSet>();

        //// �`��ɕK�v�ȏ����Z�b�g
        //SetInfomation(mode, HoloDirection);

        // �z���O��������
        //GenerateHologram();

        // projectionUI.SetActive(false);
    }

    void Update()
    {

        // ����������ĂȂ����
        if (!isInit)
        {
            Sprite = transform.GetChild(0).gameObject;
            spriteRenderer = Sprite.GetComponent<SpriteRenderer>();

            // �C���X�y�N�^�[�Ń}�X�ɍ��킹��Ǝw�肵����
            if (AutoCombine)
            {
                // �u���Ȃ���
                Replacement();
            } 

            sc_col = GetComponent<N_SetColliderOffSet>();

            // �`��ɕK�v�ȏ����Z�b�g
            SetInfomation(mode, HoloDirection);

            // �z���O��������
            GenerateHologram();

            isInit = true;

            //Debug.Log("�v���W�F�N�^�[������");
        }

        if (isReset)
        {
            // ���������z���O�����폜
            foreach (var obj in Hologram)
            {
                Destroy(obj);
            }
            Hologram.Clear();

            // �`��ɕK�v�ȏ����Z�b�g
            SetInfomation(mode, HoloDirection);

            // �z���O��������
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

                        // �������z���O������������悤�ɂ���
                        obj.GetComponent<N_HoloPlayerDestroy>().OnAlpha();
                    }
                }
                isActive = true;

                // �����蔻��̃I�u�W�F�N�g���A�N�e�B�u��
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
                SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>(true); // �q�I�u�W�F�N�g��SpriteRenderer���擾�itrue���w�肵�Ĕ�A�N�e�B�u�Ȃ��̂��܂߂�j

                foreach (SpriteRenderer renderer in spriteRenderers)
                {
                    Material material = renderer.material; // �q�I�u�W�F�N�g�̃}�e���A�����擾
                    if (material.HasProperty("_Fader")) // �}�e���A����_Fader�v���p�e�B�������Ă��邩�m�F
                    {
                        fTime += Time.deltaTime;
                        fTime = Mathf.Clamp01(fTime); // �l��0����1�͈̔͂ɐ�������
                        material.SetFloat("_Fader", fTime); // _Fader��ݒ�
                                                            // projectionUI.GetComponent<SpriteRenderer>().material.SetFloat("_Fader", fTime); // _Fader��ݒ�
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

                // �����蔻��̃I�u�W�F�N�g���A�N�e�B�u��
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

        // �z���O�����̊J�n�n�_����
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

        // �����蔻��p
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

            // �C���X�^���X����
            GameObject obj = Instantiate(Prefab, newVec, Quaternion.identity);
            // �폜����Ȃ��z���O�����ɂ���
            obj.GetComponent<N_DestroyTimer>().SetBoolDestroy(false);
            // ���g�̎q�I�u�W�F�N�g�ɂ���
            obj.transform.parent = this.gameObject.transform;           
            // �ŏ���\��
            obj.SetActive(false);
            obj.name = Prefab.name;

            InitHoloPos = obj.transform.position;
            // ���X�g�ǉ�
            Hologram.Add(obj);

            
        }

        //�ǂ̏ꍇ���������X�v���C�g�ύX
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

        // �����蔻��n
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

    // �^�C���̃}�X�ɂ����悤�ɍ��W���Z�b�g����
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

    // �K�v�ȏ����Z�b�g����
    private void SetInfomation(HOLOGRAM_MODE _mode,HOLOGRAM_DIRECTION _direction)
    {
        switch (_mode)
        {
            case HOLOGRAM_MODE.PLAYER:
                iHowMany = 1;
                // �����蔻�肪�K�v��
                sc_col.SetIsColliding(false);
                // �v���C���[�̃X�v���C�g�ɂ���
                spriteRenderer.sprite = PlayerSprite;
                Sprite.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
                break;

            case HOLOGRAM_MODE.WALL:
                sc_col.SetIsColliding(true);
                // �Ǘp�̃X�v���C�g�ɂ���
                spriteRenderer.sprite = WallSprite;
                Sprite.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
                break;

            case HOLOGRAM_MODE.FLOOR:
                sc_col.SetIsColliding(true);
                // ���p�̃X�v���C�g�ɂ���
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

    // �v���C���[�̋��͈͓��ɓ��������o��������
    public void CheckAreaSympathy(Vector3 _pos ,float _radius)
    {

        // �v���C���[���玩�g�܂ł̃x�N�g�������߂�
        Vector3 vec = trans_Projecter.position - _pos;

        // �x�N�g���̒��������߂�
        float len = vec.magnitude;

        // ���̔��a�Ɣ�r
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
