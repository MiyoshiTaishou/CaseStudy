using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_Rotation : MonoBehaviour
{
    enum ROTATIONAXIS
    {
        AXIS_X,
        AXIS_Y,
        AXIS_Z
    }

    [Header("�󂷂̂ɕK�v�ȓG�̐�"), SerializeField]
    private ROTATIONAXIS rotAxis = ROTATIONAXIS.AXIS_X;

    [Header("���b�ň��]���邩"), SerializeField]
    private float fRotateSpeed = 5.0f;

    [Header("��]���Z�b�g"), SerializeField]
    private bool isReset = false;

    private Transform trans; 

    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = 360f * Time.deltaTime / fRotateSpeed;

        switch (rotAxis)
        {
            case ROTATIONAXIS.AXIS_X:
                trans.Rotate(rot, 0.0f, 0.0f);
                break;

            case ROTATIONAXIS.AXIS_Y:
                trans.Rotate(0.0f, rot, 0.0f);
                break;

            case ROTATIONAXIS.AXIS_Z:
                trans.Rotate(0.0f, 0.0f, rot);
                break;
        }

        if (isReset)
        {
            ResetRotate();
            isReset = false;
        }
    }

    private void ResetRotate()
    {
        trans.rotation = Quaternion.identity;
    }
}
