using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_ResultSelect : MonoBehaviour
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
  
    // Start is called before the first frame update
    void Start()
    {       
        M_GameMaster.SetGameClear(false);
        M_GameMaster.SetGamePlay(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnce)
        {
            sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();

            isOnce = true;
        }

        int count = 0;

        foreach (var scene in sceneImages)
        {
            if(currentIndex != count)
            {
                scene.GetComponent<M_ImageEasing>().Resset();
            }
            count++;
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
        sceneImages[currentIndex].GetComponent<M_ImageEasing>().Resset();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2];

        // �V�����{�^����I��
        currentIndex = newIndex;

        sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2 + 1];
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);

        if (currentIndex == 0)
        {
            tran.GetComponent<M_TransitionList>().SetRe(true);
        }

        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
