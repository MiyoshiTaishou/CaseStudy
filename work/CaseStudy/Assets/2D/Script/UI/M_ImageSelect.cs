using UnityEngine;
using UnityEngine.UI;

public class M_ImageSelect : MonoBehaviour
{
    public GameObject[] images; // UI�{�^���̔z��
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private bool stickMoved = false; // �X�e�B�b�N�����������ǂ����̃t���O

    private bool once = false;

    [Header("�g�����W�V����"), SerializeField]
    private GameObject tran;

    private void Start()
    {        
        //images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
    }

    void Update()
    {
        if(!once)
        {
            images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();
            once = true;
        }

        float verticalInput = Input.GetAxisRaw("Horizontal"); // �������̃X�e�B�b�N���͂��擾

        if (verticalInput > 0.5f && !stickMoved)
        {
            SelectImage(currentIndex + 1); // �E�����Ɉړ�������O�̃{�^����I��
            stickMoved = true; // �X�e�B�b�N���������t���O�𗧂Ă�
        }
        else if (verticalInput < -0.5f && !stickMoved)
        {
            SelectImage(currentIndex - 1); // �������Ɉړ������玟�̃{�^����I��
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

        //�I������Ă�����̈ȊO
        int count = 0;
        foreach (var image in images)
        {
            if(count != currentIndex)
            {
                image.GetComponent<M_ImageEasing>().Resset();
            }
            count++;
        }
    }

    void SelectImage(int newIndex)
    {
        // �C���f�b�N�X���͈͊O�̏ꍇ�̓C���f�b�N�X�����[�v������
        if (newIndex < 0)
        {
            newIndex = images.Length - 1;
        }
        else if (newIndex >= images.Length)
        {
            newIndex = 0;
        }

        // ���݂̃{�^���̑I��������
        images[currentIndex].GetComponent<M_ImageEasing>().Resset();

        // �V�����{�^����I��
        currentIndex = newIndex;
        images[currentIndex].GetComponent<M_ImageEasing>().EasingOnOff();       
    }

    void PressSelectedButton()
    {
        tran.GetComponent<M_TransitionList>().SetIndex(currentIndex);
        tran.GetComponent<M_TransitionList>().LoadScene();
    }
}
