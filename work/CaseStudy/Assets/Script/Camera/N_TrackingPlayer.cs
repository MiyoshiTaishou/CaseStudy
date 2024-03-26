using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_TrackingPlayer : MonoBehaviour
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

    // =================================================================================

    // Start is called before the first frame update
    void Start()
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
        if(LeftUp != null)
        {
            bSet_LeftUp = true;
        }
        if (RightDpwn != null)
        {
            bSet_RightDown = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // �ǐՑΏۂ̍��W���J�����ɃZ�b�g
        CameraTransform.position = new Vector3(trans_Target.position.x, trans_Target.position.y, CameraTransform.position.z);

        // ��ʒ[�������Z�b�g����Ă�����
        if (bSet_LeftUp && bSet_RightDown)
        {
            // �͈̓`�F�b�N
            AreaCheck();
        }
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
}
