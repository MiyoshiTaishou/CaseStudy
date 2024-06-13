using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneImages
{
    public List<GameObject> images; // �V�[�����Ƃ̃{�^���̃��X�g
}

public class M_ImageSelect : MonoBehaviour
{
    [Header("�V�[�����Ƃɕ\������摜")]
    public List<SceneImages> sceneImages; // �V�[�����Ƃ̃{�^���̃��X�g
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int sceneIndex = 0; // ���ݑI�𒆂̃V�[���̃C���f�b�N�X
    private int slideIndex = 0;//�X���C�h�̃C���f�b�N�X
    private bool stickMoved = false; // �X�e�B�b�N�����������ǂ����̃t���O

    private bool once = false;

    [Header("�g�����W�V����"), SerializeField]
    private GameObject tran;

    [Header("�X���C�h"), SerializeField]
    private GameObject sla;

    private void Start()
    {
        slideIndex = sceneImages.Count - 1;
    }

    void Update()
    {
        if (!once)
        {
            sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            once = true;
        }

        if(sla.GetComponent<M_SelectSlide>().GetIsSlide())
        {
            return;
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

        if (Input.GetButtonDown("LButton"))
        {
            if (slideIndex > 0)
            {
                sla.GetComponent<M_SelectSlide>().Sub();
                SelectScene(sceneIndex + 1); // L�{�^���ŃV�[���C���f�b�N�X������
                slideIndex--;
            }            
        }

        if (Input.GetButtonDown("RButton"))
        {
            if (slideIndex < sceneImages.Count - 1)
            {
                sla.GetComponent<M_SelectSlide>().Add();
                SelectScene(sceneIndex - 1); // R�{�^���ŃV�[���C���f�b�N�X�𑝉�

                slideIndex++;
            }           
        }

        Debug.Log(slideIndex);

        // �I������Ă�����̈ȊO�����Z�b�g
        for (int i = 0; i < sceneImages.Count; i++)
        {
            for (int j = 0; j < sceneImages[i].images.Count; j++)
            {
                if (i != sceneIndex || j != currentIndex)
                {
                    Debug.Log(sceneImages[i].images[j]);
                    sceneImages[i].images[j].GetComponent<M_ImageEasing>().Resset();
                }
            }
        }
    }

    void SelectImage(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0)
        {
            newIndex = sceneImages[sceneIndex].images.Count - 1;
        }
        else if (newIndex >= sceneImages[sceneIndex].images.Count)
        {
            newIndex = 0;
        }       

        // ���݂̃{�^���̑I��������
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // �V�����{�^����I��
        currentIndex = newIndex;
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
    }

    void SelectScene(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0)
        {
            return;
        }
        else if (newIndex >= sceneImages.Count)
        {
            return;
        }

        // ���݂̃V�[���̃{�^���̑I��������
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // �V�����V�[����I��
        sceneIndex = newIndex;
        currentIndex = 0; // �V�����V�[���ł͍ŏ��̃{�^����I��
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();

        tran.GetComponent<M_TransitionList>().SetSceneIndex(sceneIndex);
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
