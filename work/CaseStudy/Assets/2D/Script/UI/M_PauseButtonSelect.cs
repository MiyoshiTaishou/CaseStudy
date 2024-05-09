#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif
using UnityEngine;
using UnityEngine.UI;

public class M_PauseButtonSelect : MonoBehaviour
{
    public Button[] buttons; // UI�{�^���̔z��
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private bool stickMoved = false; // �X�e�B�b�N�����������ǂ����̃t���O

    private void Start()
    {
        // �V�����{�^����I��       
        buttons[currentIndex].Select();
        buttons[currentIndex].image.color = Color.green; // �V�����{�^���̐F��ύX
    }

    void Update()
    {
        if (!M_GameMaster.GetGamePlay())
        {
            float verticalInput = Input.GetAxisRaw("Horizontal"); // �������̃X�e�B�b�N���͂��擾

            if (verticalInput > 0.5f && !stickMoved)
            {
                SelectButton(currentIndex + 1); // �E�����Ɉړ�������O�̃{�^����I��
                stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
            }
            else if (verticalInput < -0.5f && !stickMoved)
            {
                SelectButton(currentIndex - 1); // �������Ɉړ������玟�̃{�^����I��
                stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
            }
            else if (verticalInput == 0)
            {
                stickMoved = false; // �X�e�B�b�N�������ʒu�ɖ߂�����t���O�����Z�b�g
            }

            if (Input.GetButtonDown("SympathyButton"))
            {
                PressSelectedButton(); // �{�^���������{�^���������ꂽ��I�𒆂̃{�^��������
            }
        }
    }

    void SelectButton(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0)
        {
            newIndex = buttons.Length - 1;
        }
        else if (newIndex >= buttons.Length)
        {
            newIndex = 0;
        }

        // ���݂̃{�^���̑I��������
        buttons[currentIndex].image.color = Color.white; // ���݂̃{�^���̐F�����ɖ߂�

        // �V�����{�^����I��
        currentIndex = newIndex;
        buttons[currentIndex].Select();
        buttons[currentIndex].image.color = Color.green; // �V�����{�^���̐F��ύX
    }

    void PressSelectedButton()
    {
        buttons[currentIndex].onClick.Invoke(); // �I�𒆂̃{�^��������
    }
}
