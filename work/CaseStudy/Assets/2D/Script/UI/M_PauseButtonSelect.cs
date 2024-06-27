using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_PauseButtonSelect : MonoBehaviour
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

    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        //M_GameMaster.SetGameClear(false);
        //M_GameMaster.SetGamePlay(false);

        sceneImages[0].GetComponent<Image>().sprite = OnOff[1];

        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (M_GameMaster.GetGamePlay())
        {
            Player.GetComponent<N_ProjecterSympathy>().enabled = true;
            return;
        }

        //int count = 0;

        //foreach (var scene in sceneImages)
        //{
        //    if (currentIndex != count)
        //    {
        //        scene.GetComponent<M_ImageEasing>().Resset();
        //    }
        //    count++;
        //}

        Player.GetComponent<N_ProjecterSympathy>().enabled = false;

        float verticlainput = Input.GetAxisRaw("Vertical"); // �c�����̃X�e�B�b�N���͂��擾

        if (verticlainput > 0.5f && !stickMoved)
        {
            Debug.Log("�X����");
            SelectImage(currentIndex - 1); // �E�����Ɉړ������玟�̃{�^����I��
            stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
        }
        else if (verticlainput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // �������Ɉړ�������O�̃{�^����I��
            stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
        }
        else if (verticlainput == 0)
        {
            stickMoved = false; // �X�e�B�b�N�������ʒu�ɖ߂�����t���O�����Z�b�g
        }

        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // �{�^���������{�^���������ꂽ��I�𒆂̃{�^��������
        }

        if (Input.GetButtonDown("Cancel"))
        {
            this.GetComponent<M_Pause>().PauseOnOff();
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
        //sceneImages[currentIndex].GetComponent<M_ImageEasing>().Resset();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2];

        // �V�����{�^����I��
        currentIndex = newIndex;

        //sceneImages[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        //sceneImages[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        sceneImages[currentIndex].GetComponent<Image>().sprite = OnOff[currentIndex * 2 + 1];
    }

    void PressSelectedButton()
    {
        switch (currentIndex)
        {
            case 0:
                this.GetComponent<M_Pause>().PauseOnOff();
                break;

            case 1:
                tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
                tran.GetComponent<M_TransitionList>().SetRe(true);
                tran.GetComponent<M_TransitionList>().LoadScene();
                break;
            case 2:
                tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);                
                tran.GetComponent<M_TransitionList>().LoadScene();
                break;
            case 3:
                //�I�v�V�����̂��
                break;
        }      
    }
}
