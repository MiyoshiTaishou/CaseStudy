using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(M_ImageEasing))]//拡張するクラスを指定
public class M_EasingGUI : Editor
{
    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        //targetを変換して対象を取得
        M_ImageEasing ImageEasingScript = target as M_ImageEasing;

        //ボタンを表示
        if (GUILayout.Button("デバック用イージングオンオフ"))
        {
            ImageEasingScript.EasingOnOff();
        }
    }
}
