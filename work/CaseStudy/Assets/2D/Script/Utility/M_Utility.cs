using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 汎用的な関数をここにまとめる
/// </summary>
public static class M_Utility
{   
    public static IEnumerator GamePadMotor(float _time)
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            //Debug.Log("ゲームパッド未接続");
            yield break;
        }

        //Debug.Log("左モーター振動");
        //gamepad.SetMotorSpeeds(1.0f, 0.0f);
        //yield return new WaitForSeconds(1.0f);

        //Debug.Log("右モーター振動");
        gamepad.SetMotorSpeeds(0.0f, 1.0f);
        yield return new WaitForSeconds(_time);

        //Debug.Log("モーター停止");
        gamepad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
