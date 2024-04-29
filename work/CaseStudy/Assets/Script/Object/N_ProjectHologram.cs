using System.Collections;
using System.Collections.Generic;
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
    [Header("�v���W�F�N�^�[�̌���"), SerializeField]
    HOLOGRAM_DIRECTION direction = HOLOGRAM_DIRECTION.UP;

    [Header("�ǂꂭ�炢���ꂽ�ʒu�ɕ\�����邩"), SerializeField]
    private float fDistance = 1.0f;

    [Header("�����A�˂邩"), SerializeField]
    private int iHowMany = 1;

    [Header("�v���W�F�N�^�[�̃I���I�t"), SerializeField]
    private bool isProjection = false;

    public bool GetProjection() { return isProjection; }

    private bool isActive = false;

    private GameObject Prefab;

    private Transform trans_Projecter;

    // ���������z���O����
    private List<GameObject> Hologram = new List<GameObject>();

    // ��x�̋��ŃI���I�t���؂�ւ��͈̂��
    private bool isAlreadySwitch = false;   

    // Start is called before the first frame update
    void Start()
    {
        // �g�����X�t�H�[���擾
        trans_Projecter = this.gameObject.transform;

        // �u���Ȃ���
        Replacement();

        // �`��ɕK�v�ȏ����Z�b�g
        SetInfomation(mode, direction);

        // �z���O��������
        GenerateHologram();
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
                    obj.SetActive(true);
                }
                isActive = true;
            }
        }
        else
        {
            if(isActive == true)
            {
                foreach (GameObject obj in Hologram)
                {
                    obj.SetActive(false);
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

        vec.x = Mathf.FloorToInt(vec.x) + sca.x / 2.0f;
        vec.y = Mathf.FloorToInt(vec.y) + sca.y / 2.0f;

        trans_Projecter.position = vec;
    }

    // �K�v�ȏ����Z�b�g����
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

        // �p�X�����Ƀv���n�u���擾
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
