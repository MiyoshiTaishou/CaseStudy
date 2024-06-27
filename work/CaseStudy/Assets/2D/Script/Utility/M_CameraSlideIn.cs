using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Tilemaps;
public class M_CameraSlideIn : MonoBehaviour
{
    [Header("�J�����ړ��̃C�[�W���O�֐�"),SerializeField]
    private M_Easing.Ease easeCamMove;

    [Header("�J�n�n�_�̃I�u�W�F�N�g"), SerializeField]
    private GameObject StartObj;

    [Header("�I���n�_�̃I�u�W�F�N�g"), SerializeField]
    private GameObject EndObj;

    [Header("�ړ��ɂ����鎞��")]
    [SerializeField] private float durationMove = 1.0f;

    [Header("�J�����Y�[���A�E�g�̃C�[�W���O�֐�"), SerializeField]
    private M_Easing.Ease easeCamOut;

    [Header("�ǂꂾ���Y�[���A�E�g���邩"), SerializeField]
    private float outDis;

    [Header("�Y�[���A�E�g�ɂ����鎞��")]
    [SerializeField] private float durationOut = 1.0f;

    /// <summary>
    /// �ړ��̌v������
    /// </summary>
    private float fTimeMove;

    /// <summary>
    /// �ړ��̌v������
    /// </summary>
    private float fTimeOut;

    /// <summary>
    /// �J������Z���W
    /// </summary>
    private float camPosZ;

    /// <summary>
    /// �J�����R���|�[�l���g
    /// </summary>
    private Camera cam;

    /// <summary>
    /// �J�n���̋���
    /// </summary>
    private float camZoom;

    private bool isOnce = false;
    private bool isOnce2 = false;

    [Header("�^�C���}�b�v"), SerializeField]
    public Tilemap tilemap; // �^�C���}�b�v

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    Vector3 startPos;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<N_TrackingPlayer>().enabled = false;

        camPosZ = this.transform.position.z;
        cam = GetComponent<Camera>();
        camZoom = cam.orthographicSize;

        M_GameMaster.SetGamePlay(false);


        if (tilemap)
        {
            // �^�C���}�b�v�͈̔͂��v�Z
            CalculateBounds();
        }

        startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);

        this.transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {      
        Debug.Log(M_GameMaster.GetGamePlay());
        //�A�E�g����
        fTimeOut += Time.deltaTime;
        if(fTimeOut > durationOut)
        {
            fTimeOut = durationOut;

            //�ړ�����
            fTimeMove += Time.deltaTime;
            if (fTimeMove > durationMove && !isOnce)
            {
                Debug.Log("�����Ƃ�H");
                fTimeMove = durationMove;
                GetComponent<N_TrackingPlayer>().enabled = true;
                if (!isOnce2)
                {
                    M_GameMaster.SetGamePlay(true);
                }
                isOnce2 = true;

                GetComponent<M_CameraSlideIn>().enabled = false;

            }

            EasingMove();
        }
        else
        {
            EasingOut();
        }

        if (fTimeMove > durationMove && !isOnce)
        {
            Debug.Log("���ԃI�[�o�[");
            if(!M_GameMaster.GetGamePlay() && !isOnce)
            {
                fTimeMove = durationMove;
                GetComponent<N_TrackingPlayer>().enabled = true;
                M_GameMaster.SetGamePlay(true);
                isOnce = true;
                //cam.orthographicSize = camZoom;
                GetComponent<M_CameraSlideIn>().enabled = false;
                cam.orthographicSize = camZoom;
                Debug.Log("������I");
            }
        }
        if(camZoom < cam.orthographicSize)
        {
            cam.orthographicSize = camZoom;
        }

        if (tilemap)
        {
            camHalfHeight = Camera.main.orthographicSize;
            camHalfWidth = camHalfHeight * Camera.main.aspect;
            if (this.transform.position.y <= minBounds.y + camHalfHeight)
            {
                this.transform.position = new Vector3(this.transform.position.x, minBounds.y + camHalfHeight, this.transform.position.z);
            }
            Vector3 newPosition = this.transform.position;
            float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            this.transform.position = new Vector3(clampedX, this.transform.position.y, this.transform.position.z);
        }
    }

    //�ړ��̃C�[�W���O
    private void EasingMove()
    {
        float t = Mathf.Clamp01(fTimeMove / durationMove);
             
        var func = M_Easing.GetEasingMethod(easeCamMove);

        //Vector3 startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        //Vector3 endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);

        this.transform.position = startPos + (endPos - startPos) * func(t);
    }

    //�A�E�g�C�[�W���O
    private void EasingOut()
    {
        float t = Mathf.Clamp01(fTimeOut / durationOut);

        var func = M_Easing.GetEasingMethod(easeCamOut);

        cam.orthographicSize = camZoom - outDis * func(t);
    }

    void CalculateBounds()
    {
        // �����l����ɑ傫��/�����Ȓl�ɐݒ�
        Vector3Int minCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int maxCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        // �^�C���}�b�v�̂��ׂẴZ�����`�F�b�N
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (pos.x < minCell.x) minCell.x = pos.x;
                if (pos.y < minCell.y) minCell.y = pos.y;
                if (pos.z < minCell.z) minCell.z = pos.z;

                if (pos.x > maxCell.x) maxCell.x = pos.x;
                if (pos.y > maxCell.y) maxCell.y = pos.y;
                if (pos.z > maxCell.z) maxCell.z = pos.z;
            }
        }

        // �^�C���}�b�v�̍����[�ƉE��[�̃��[���h���W���v�Z
        Vector3 minWorld = tilemap.CellToWorld(minCell);
        Vector3 maxWorld = tilemap.CellToWorld(maxCell) + tilemap.cellSize;

        // �I�t�Z�b�g��ǉ����ăJ�������X�e�[�W�O���f���Ȃ��悤�ɂ���
        minBounds = new Vector2(minWorld.x, minWorld.y);
        maxBounds = new Vector2(maxWorld.x, maxWorld.y);
    }
}
