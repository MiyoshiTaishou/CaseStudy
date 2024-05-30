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

    // Start is called before the first frame update
    void Start()
    {
        // �g�����X�t�H�[���擾
        trans_Projecter = this.gameObject.transform;

        // �C���X�y�N�^�[�Ń}�X�ɍ��킹��Ǝw�肵����
        if (AutoCombine)
        {
            // �u���Ȃ���
            Replacement();
        }

        // �`��ɕK�v�ȏ����Z�b�g
        SetInfomation(mode, HoloDirection);

        // �z���O��������
        GenerateHologram();       

        // projectionUI.SetActive(false);
    }

    void Update()
    {
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
                }
                isActive = true;
                if (MeowingPrefab)
                {
                    MeowingObj = Instantiate(MeowingPrefab, transform.position, Quaternion.identity);
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
                    MeowingObj = Instantiate(MeowingPrefab, transform.position, Quaternion.identity);
                }
                if (audioclip)
                {
                    AudioSource.PlayClipAtPoint(audioclip, transform.position);
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
                vec.z
                );

            Debug.Log(newVec);

            // �C���X�^���X����
            GameObject obj = Instantiate(Prefab, newVec, Quaternion.identity);
            // �폜����Ȃ��z���O�����ɂ���
            obj.GetComponent<N_DestroyTimer>().SetBoolDestroy(false);
            // ���g�̎q�I�u�W�F�N�g�ɂ���
            obj.transform.parent = this.gameObject.transform;           
            // �ŏ���\��
            obj.SetActive(false);
            // ���X�g�ǉ�
            Hologram.Add(obj);            
        }

    }

    // �^�C���̃}�X�ɂ����悤�ɍ��W���Z�b�g����
    private void Replacement()
    {
        Vector2 vec = trans_Projecter.position;
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
                break;

            case HOLOGRAM_MODE.WALL:
                break;

            case HOLOGRAM_MODE.FLOOR:
                break;
            case HOLOGRAM_MODE.TRANS:
                break;
        }

        Prefab = gHolograms[(int)_mode];

        switch (_direction)
        {
            case HOLOGRAM_DIRECTION.LEFT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                break;

            case HOLOGRAM_DIRECTION.RIGHT:
                trans_Projecter.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
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

    public void Initialize()
    {
        isAlreadySwitch = false;
    }
}
