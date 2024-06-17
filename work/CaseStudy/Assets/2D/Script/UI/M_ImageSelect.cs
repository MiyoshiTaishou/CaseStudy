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

    [Header("�V�[�����Ƃɕ\�����鏵�ҏ�摜")]
    public List<SceneImages> ChallengeImages; // �V�[�����Ƃ̃{�^���̃��X�g

    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int sceneIndex = 0; // ���ݑI�𒆂̃V�[���̃C���f�b�N�X
    private int slideIndex = 0; // �X���C�h�̃C���f�b�N�X
    private bool stickMoved = false; // �X�e�B�b�N�����������ǂ����̃t���O
    private bool once = false;

    [Header("�g�����W�V����"), SerializeField]
    private GameObject tran;

    [Header("�X���C�h"), SerializeField]
    private GameObject sla;

    // ���̈ʒu��ۑ����邽�߂̎���
    private Dictionary<GameObject, int> originalSiblingIndices = new Dictionary<GameObject, int>();

    //���ҏ�J���Ă��邩�H
    private bool isChallenge = false;

    private void Start()
    {
        slideIndex = sceneImages.Count - 1;

        // ���̈ʒu��ۑ�
        foreach (var scene in sceneImages)
        {
            foreach (var image in scene.images)
            {
                originalSiblingIndices[image] = image.transform.GetSiblingIndex();
            }
        }
    }

    void Update()
    {
       if(isChallenge)
        {
            ChallengeUpdate();
        }
       else
        {
            SelectUpdate();
        }
    }

    void ChallengeUpdate()
    {
        if (Input.GetButtonDown("SympathyButton"))
        {
            PressSelectedButton(); // �{�^���������{�^���������ꂽ��I�𒆂̃{�^��������
        }

        if(Input.GetButtonDown("Cancel"))
        {
            Debug.Log("��������");
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().SetReverse(true);
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();           
            isChallenge = false;
        }
    }

    void SelectUpdate()
    {
        if (!once)
        {
            sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            MoveToFront(sceneImages[sceneIndex].images[currentIndex]);
            once = true;
        }

        if (sla.GetComponent<M_SelectSlide>().GetIsSlide())
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
           isChallenge = true;

            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().SetReverse(false);
            ChallengeImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        }

        if (Input.GetButtonDown("RButton"))
        {
            if (slideIndex > 0)
            {
                sla.GetComponent<M_SelectSlide>().Sub();
                SelectScene(sceneIndex + 1); // L�{�^���ŃV�[���C���f�b�N�X������
                slideIndex--;
            }
        }

        if (Input.GetButtonDown("LButton"))
        {
            if (slideIndex < sceneImages.Count - 1)
            {
                sla.GetComponent<M_SelectSlide>().Add();
                SelectScene(sceneIndex - 1); // R�{�^���ŃV�[���C���f�b�N�X�𑝉�
                slideIndex++;
            }
        }

        // �I������Ă�����̈ȊO�����Z�b�g
        for (int i = 0; i < sceneImages.Count; i++)
        {
            for (int j = 0; j < sceneImages[i].images.Count; j++)
            {
                if (i != sceneIndex || j != currentIndex)
                {
                    sceneImages[i].images[j].GetComponent<M_ImageEasing>().Resset();
                    sceneImages[i].images[j].GetComponent<M_OutLine>().OutLineOff();
                    ResetPosition(sceneImages[i].images[j]); // ���̈ʒu�ɖ߂�                    
                }
            }
        }

        Debug.Log(sceneIndex);
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
        ResetPosition(sceneImages[sceneIndex].images[currentIndex]); // ���̈ʒu�ɖ߂�

        // �V�����{�^����I��
        currentIndex = newIndex;
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        MoveToFront(sceneImages[sceneIndex].images[currentIndex]);

        Debug.Log(sceneImages[sceneIndex].images[currentIndex].transform.GetSiblingIndex());
    }

    void SelectScene(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0 || newIndex >= sceneImages.Count)
        {
            return;
        }

        // ���݂̃V�[���̃{�^���̑I��������
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().Resset();
        ResetPosition(sceneImages[sceneIndex].images[currentIndex]); // ���̈ʒu�ɖ߂�

        // �V�����V�[����I��
        sceneIndex = newIndex;
        currentIndex = 0; // �V�����V�[���ł͍ŏ��̃{�^����I��
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
        sceneImages[sceneIndex].images[currentIndex].GetComponent<M_OutLine>().OutLineOn();
        MoveToFront(sceneImages[sceneIndex].images[currentIndex]);

        tran.GetComponent<M_TransitionList>().SetSceneIndex(sceneIndex);
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }

    // �I�u�W�F�N�g���őO��Ɉړ������郁�\�b�h
    void MoveToFront(GameObject obj)
    {
        int count = obj.transform.parent.childCount; // �e�̎q�I�u�W�F�N�g�̐����擾
        Debug.Log($"Moving {obj.name} to front. Parent has {count} children.");
        obj.transform.SetAsLastSibling(); // �őO��Ɉړ�
        Debug.Log($"{obj.name} new sibling index: {obj.transform.GetSiblingIndex()}");
    }

    // �I�u�W�F�N�g�����̈ʒu�ɖ߂����\�b�h
    void ResetPosition(GameObject obj)
    {
        if (originalSiblingIndices.ContainsKey(obj))
        {
            obj.transform.SetSiblingIndex(originalSiblingIndices[obj]);
            Debug.Log($"{obj.name} reset to original index: {originalSiblingIndices[obj]}");
        }
    }
}
