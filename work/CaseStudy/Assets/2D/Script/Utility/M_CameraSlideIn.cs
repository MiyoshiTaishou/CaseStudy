using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

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

    // Start is called before the first frame update
    void Start()
    {
        camPosZ = this.transform.position.z;
        cam = GetComponent<Camera>();
        camZoom = cam.orthographicSize;

        this.transform.position = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        //�A�E�g����
        fTimeOut += Time.deltaTime;
        if(fTimeOut > durationOut)
        {
            fTimeOut = durationOut;

            //�ړ�����
            fTimeMove += Time.deltaTime;
            if (fTimeMove > durationMove)
            {
                fTimeMove = durationMove;
            }
            EasingMove();
        }
        else
        {
            EasingOut();
        }             
    }

    //�ړ��̃C�[�W���O
    private void EasingMove()
    {
        float t = Mathf.Clamp01(fTimeMove / durationMove);
             
        var func = M_Easing.GetEasingMethod(easeCamMove);

        Vector3 startPos = new Vector3(StartObj.transform.position.x, StartObj.transform.position.y, camPosZ);
        Vector3 endPos = new Vector3(EndObj.transform.position.x, EndObj.transform.position.y, camPosZ);
        this.transform.position = startPos + (endPos - startPos) * func(t);
    }

    //�A�E�g�C�[�W���O
    private void EasingOut()
    {
        float t = Mathf.Clamp01(fTimeOut / durationOut);

        var func = M_Easing.GetEasingMethod(easeCamOut);

        cam.orthographicSize = camZoom - outDis * func(t);
    }
}
