using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �ėp�I�Ȋ֐��������ɂ܂Ƃ߂�
/// </summary>
public static class M_Utility
{   
    public static IEnumerator GamePadMotor(float _time)
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            //Debug.Log("�Q�[���p�b�h���ڑ�");
            yield break;
        }

        //Debug.Log("�����[�^�[�U��");
        //gamepad.SetMotorSpeeds(1.0f, 0.0f);
        //yield return new WaitForSeconds(1.0f);

        //Debug.Log("�E���[�^�[�U��");
        gamepad.SetMotorSpeeds(0.0f, 1.0f);
        yield return new WaitForSeconds(_time);

        //Debug.Log("���[�^�[��~");
        gamepad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
