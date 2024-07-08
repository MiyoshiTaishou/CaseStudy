using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class K_5_4Camera : MonoBehaviour
{
    // ��ʒ[�I�u�W�F�N�g�Z�b�g
    [Header("CameraEnd�@����"), SerializeField]
    private GameObject LeftUp;

    [Header("CameraEnd�@�E��"), SerializeField]
    private GameObject RightDpwn;

    [Header("�ǐՑΏ�"), SerializeField]
    private GameObject Target;

    /// <summary>
    /// �J�����̕`��͈�
    /// </summary>
    [Header("�J�����̕`��͈�"), SerializeField]
    private float fViewSize = 5;


    /// <summary>
    /// �J�����R���|�[�l���g
    /// </summary>
    private Camera MainCamera;

    /// <summary>
    /// �J�����̍��W�����
    /// </summary>
    private Transform CameraTransform;

    /// <summary>
    /// ������W�����
    /// </summary>
    private Transform trans_LeftUp;

    /// <summary>
    /// �E�����W�����
    /// </summary>
    private Transform trans_RightDown;

    /// <summary>
    /// �ǐՑΏۍ��W�����
    /// </summary>
    private Transform trans_Target;

    /// <summary>
    /// ��ʒ[�I�u�W�F�N�g���Z�b�g����Ă��邩
    /// </summary>
    private bool bSet_LeftUp = false;

    /// <summary>
    /// ��ʒ[�I�u�W�F�N�g���Z�b�g����Ă��邩
    /// </summary>
    private bool bSet_RightDown = false;

    /// <summary>
    /// �ǐՕ��@�X�e�[�g�}�V���񋓌^
    /// </summary>
    private enum TrackingMethod
    {
        NORMAL,
        WARP,
    }

    /// <summary>
    /// �ǐՕ��@�X�e�[�g�}�V��
    /// </summary>
    private TrackingMethod _trackingmethod = TrackingMethod.NORMAL;

    /// <summary>
    /// �҂�����
    /// </summary>
    private float waitTime = 0.0f;

    /// <summary>
    /// ���[�v�O���W
    /// </summary>
    private Vector2 BeforeWarpPos;

    /// <summary>
    /// ���[�v����W
    /// </summary>
    private Vector2 AfterWarpPos;

    /// <summary>
    /// ���[�v�ǐՂɂ����鎞��
    /// </summary>
    [Header("���[�v�ǐՂɂ����鎞��"), SerializeField]
    private float warpTrackTime = 0.5f;

    /// <summary>
    /// ���[�v�ǐՌo�ߎ���
    /// </summary>
    private float warpElapsedTime = 0.0f;

    private Vector2 WarpTargetVec;

    private bool isWarp = false;

    [Header("�^�C���}�b�v"), SerializeField]
    public Tilemap tilemap; // �^�C���}�b�v

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    public bool GetisWarp()
    {
        return isWarp;
    }

    public Tilemap GetTilemap() { return tilemap; }

    // =================================================================================
    // Start is called before the first frame update
    void OnEnable()
    {
        // �J�����R���|�[�l���g�擾
        MainCamera = this.gameObject.GetComponent<Camera>();
        // �J�����`��͈͐ݒ�
        MainCamera.orthographicSize = fViewSize;
        // ���W���擾
        CameraTransform = this.gameObject.transform;
        trans_LeftUp = LeftUp.GetComponent<Transform>();
        trans_RightDown = RightDpwn.GetComponent<Transform>();

        // �I�u�W�F�N�g���Z�b�g����Ă��邩�`�F�b�N
        if (Target == null)
        {
            Debug.Log("�ǐՑΏۂƂȂ�I�u�W�F�N�g���Z�b�g���Ă�������");
        }
        // �ǐՑΏۂ̍��W���擾
        trans_Target = Target.GetComponent<Transform>();

        // �I�u�W�F�N�g���Z�b�g����Ă��邩�`�F�b�N
        if (LeftUp != null)
        {
            bSet_LeftUp = true;
        }
        if (RightDpwn != null)
        {
            bSet_RightDown = true;
        }

        // �J�����̔����̍����ƕ����v�Z
        camHalfHeight = Camera.main.orthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;

        if (tilemap)
        {
            // �^�C���}�b�v�͈̔͂��v�Z
            CalculateBounds();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // ��Ԃɂ���ĒǐՕ��@��ύX
        switch (_trackingmethod)
        {
            case TrackingMethod.NORMAL:
                NormalTracking();
                break;

            case TrackingMethod.WARP:
                WarpTracking();
                break;
        }

        float posy = trans_Target.position.y;

        if (-1 < posy && posy < 16)
        {
            //1��
            Debug.Log("111111111111111111111111111");
            float clampedY = Mathf.Clamp(CameraTransform.position.y, 6.0f, 8.0f);
            CameraTransform.position = new Vector3(CameraTransform.position.x, clampedY, CameraTransform.position.z);
        }


        if (20<posy&&posy<40)
        {
            //2��
            Debug.Log("22222222222222222222222222222222");
            float clampedY = Mathf.Clamp(CameraTransform.position.y, 30f, 32f);
            CameraTransform.position = new Vector3(CameraTransform.position.x, clampedY, CameraTransform.position.z);
        }

        if (45 < posy && posy < 65)
        {
            //3��
            Debug.Log("333333333333333333333333333333333");
            float clampedY = Mathf.Clamp(CameraTransform.position.y, 54f, 56f);
            CameraTransform.position = new Vector3(CameraTransform.position.x, clampedY, CameraTransform.position.z);
        }

        if (70 < posy && posy < 90)
        {
            //4��
            Debug.Log("44444444444444444444444444444444");
            float clampedY = Mathf.Clamp(CameraTransform.position.y, 78.0f, 80.0f);
            CameraTransform.position = new Vector3(CameraTransform.position.x, clampedY, CameraTransform.position.z);
        }
    }

    // �ʏ펞�̑Ώےǐ�
    private void NormalTracking()
    {
        // �ǐՑΏۂ̍��W���J�����ɃZ�b�g
        CameraTransform.position = new Vector3(trans_Target.position.x, trans_Target.position.y, CameraTransform.position.z);
        if (tilemap)
        {
            if (CameraTransform.position.y <= minBounds.y + camHalfHeight)
            {
                CameraTransform.position = new Vector3(CameraTransform.position.x, minBounds.y + camHalfHeight, CameraTransform.position.z);
            }
            Vector3 newPosition = CameraTransform.position;
            float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            CameraTransform.position = new Vector3(clampedX, CameraTransform.position.y, CameraTransform.position.z);
        }
    }

    // ���[�v���̑Ώےǐ�
    private void WarpTracking()
    {
        // ����������
        if (warpElapsedTime == 0.0f)
        {
            // �v���C���[���烏�[�v��ւ̃x�N�g���v�Z
            float subX = AfterWarpPos.x - BeforeWarpPos.x;
            float subY = AfterWarpPos.y - BeforeWarpPos.y;

            isWarp = true;

            // �L��
            WarpTargetVec = new Vector2(subX, subY);
        }

        // ���Ԍo��
        warpElapsedTime += Time.deltaTime;

        // �ړ���
        Vector2 moveVec = WarpTargetVec * warpElapsedTime / warpTrackTime;

        // �J�����̈ړ�
        CameraTransform.position = new Vector3(
            BeforeWarpPos.x + moveVec.x,
            BeforeWarpPos.y + moveVec.y,
            CameraTransform.position.z);

        if (tilemap)
        {
            if (CameraTransform.position.y <= minBounds.y + camHalfHeight)
            {
                Debug.Log("caaaaaaaaaaaaaaaaaaaamy" + (minBounds.y + camHalfHeight));
                CameraTransform.position = new Vector3(CameraTransform.position.x, minBounds.y + camHalfHeight, CameraTransform.position.z);
            }
            Vector3 newPosition = CameraTransform.position;
            float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            CameraTransform.position = new Vector3(clampedX, CameraTransform.position.y, CameraTransform.position.z);
        }
        // �I������
        if (warpElapsedTime >= warpTrackTime)
        {
            // �ʏ펞�ǐՏ����Ɉڍs
            _trackingmethod = TrackingMethod.NORMAL;

            // ������
            warpElapsedTime = 0.0f;
            WarpTargetVec = Vector2.zero;
            AfterWarpPos = Vector2.zero;
            BeforeWarpPos = Vector2.zero;
            isWarp = false;
        }
        Debug.Log("caaaaaaaaaaaaaaaaaaaampos" + CameraTransform.position);
    }

    // �O�����烏�[�v�ǐՎ��ɕK�v�ȃI�u�W�F�N�g���Z�b�g
    public void SetWarpInfo(float _waitTime, GameObject _obj)
    {
        // ���[�v�ǐՂɈڍs
        _trackingmethod = TrackingMethod.WARP;

        // ���[�v��̃I�u�W�F�N�g�̍��W�擾
        AfterWarpPos = new Vector2(_obj.transform.position.x, _obj.transform.position.y);

        // �J�����ړ��������Ԃ����[�v�O�̑҂����Ԃ��Z���Ƌ��������������Ȃ邽��
        waitTime = _waitTime;
        if (warpTrackTime < waitTime)
        {
            warpTrackTime = waitTime + 0.1f;
        }

        // ���[�v�O�̍��W���擾
        BeforeWarpPos = new Vector2(CameraTransform.position.x, CameraTransform.position.y);

        // �v���C���[�����[�v����t���[�����珈�����J�n���邽�߂̖������Ăяo��
        WarpTracking();
    }

    // �`��G���A���O��Ȃ��悤�`�F�b�N
    private void AreaCheck()
    {
        const float adjust = 2.25f;

        // ��̐����͈͂���o�悤�Ƃ��Ă�����
        if (CameraTransform.position.y + MainCamera.orthographicSize >= trans_LeftUp.position.y)
        {
            CameraTransform.position = new Vector3(
                CameraTransform.position.x,
                trans_LeftUp.position.y - MainCamera.orthographicSize,
                CameraTransform.position.z);
        }

        // ���̐����͈͂���o�悤�Ƃ��Ă�����
        if (CameraTransform.position.x - MainCamera.orthographicSize * adjust <= trans_LeftUp.position.x)
        {
            CameraTransform.position = new Vector3(
                trans_LeftUp.position.x + MainCamera.orthographicSize * adjust,
                CameraTransform.position.y,
                CameraTransform.position.z);
        }

        // ���̐����͈͂���o�悤�Ƃ��Ă�����
        if (CameraTransform.position.y - MainCamera.orthographicSize <= trans_RightDown.position.y)
        {
            CameraTransform.position = new Vector3(
                CameraTransform.position.x,
                trans_RightDown.position.y + MainCamera.orthographicSize,
                CameraTransform.position.z);
        }

        // �E�̐����͈͂���o�悤�Ƃ��Ă�����
        if (CameraTransform.position.x + MainCamera.orthographicSize * adjust >= trans_RightDown.position.x)
        {
            CameraTransform.position = new Vector3(
                trans_RightDown.position.x - MainCamera.orthographicSize * adjust,
                CameraTransform.position.y,
                CameraTransform.position.z);
        }
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