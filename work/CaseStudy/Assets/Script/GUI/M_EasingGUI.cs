using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(M_ImageEasing))]//�g������N���X���w��
public class M_EasingGUI : Editor
{
    /// <summary>
    /// Inspector��GUI���X�V
    /// </summary>
    public override void OnInspectorGUI()
    {
        //����Inspector������\��
        base.OnInspectorGUI();

        //target��ϊ����đΏۂ��擾
        M_ImageEasing ImageEasingScript = target as M_ImageEasing;

        //�{�^����\��
        if (GUILayout.Button("�f�o�b�N�p�C�[�W���O�I���I�t"))
        {
            ImageEasingScript.EasingOnOff();
        }
    }
}
