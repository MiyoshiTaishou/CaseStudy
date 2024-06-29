using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_TitleSelect : MonoBehaviour
{
    [Header("�V�[�����Ƃɕ\������摜")]
    public GameObject[] sceneImages; // �V�[�����Ƃ̃{�^���̃��X�g

    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X

    [Header("�g�����W�V����"), SerializeField]
    private GameObject tran;

    private bool stickMoved = false; // �X�e�B�b�N�����������ǂ����̃t���O

    [Header("�I���I�t�̉摜������"), SerializeField]
    private Sprite[] OnOff;

    bool isOnce = false;

    private N_PlaySound sound;

    // Start is called before the first frame update
    void Start()
    {
        sceneImages[0].GetComponent<Image>().sprite = OnOff[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce)
        {           
            sound = GameObject.Find("Sound").GetComponent<N_PlaySound>();

            isOnce = true;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal"); // �������̃X�e�B�b�N���͂��擾

        if (horizontalInput > 0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // �E�����Ɉړ������玟�̃{�^����I��
            stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
        }
        else if (horizontalInput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex - 1); // �������Ɉړ�������O�̃{�^����I��
            stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
        }
        else if (horizontalInput == 0)
        {
            stickMoved = false; // �X�e�B�b�N�������ʒu�ɖ߂�����t���O�����Z�b�g
        }

        if (horizontalInput != 0 && !stickMoved)
        {
            sound.PlaySound(N_PlaySound.SEName.CursorMove);
        }

        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // �{�^���������{�^���������ꂽ��I�𒆂̃{�^��������
        }
    }

    void SelectImage(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0)
        {
            return;
        }
        else if (newIndex >= sceneImages.Length)
        {
            return;
        }

        // ���݂̃{�^���̑I��������        
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2];

        // �V�����{�^����I��
        currentIndex = newIndex;
      
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2 + 1];
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);      

        if(currentIndex == 1)
        {
            tran.GetComponent<M_TransitionList>().SetEnd(true);
        }

        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
