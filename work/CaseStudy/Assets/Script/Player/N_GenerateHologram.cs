using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �z���O���������
// �E�N���b�N�����ꏊ�Ƀv���C���[�̃z���O����
// ���N���b�N���Ȃ���I�������͈͓���
// �^�C���}�b�v�̃}�X���Ƃɓ����蔻��̂Ȃ��ǁA���̃z���O����

public class N_GenerateHologram : MonoBehaviour
{
    // ��������I�u�W�F�N�g
    [Header("�z���v���C���["), SerializeField]
    private GameObject Holo_Player;

    [Header("�z����"), SerializeField]
    private GameObject Holo_Wall;

    [Header("�z����"), SerializeField]
    private GameObject Holo_Floor;

    [Header("�^�C���̃T�C�Y"), SerializeField]
    private float fTileScale = 1.0f;

    [Header("�������@"), SerializeField]
    private bool bGenerationMethosd = true;

    /// <summary>
    /// �}�E�X���������܂ꂽ�ʒu
    /// </summary>
    private Vector3 StartPos = Vector3.zero;

    /// <summary>
    /// �}�E�X���������܂ꂽ�ʒu
    /// </summary>
    private Vector3 EndPos = Vector3.zero;

    /// <summary>
    /// �{�^�������ꂽ��
    /// </summary>
    private bool isRelease = false;

    // Update is called once per frame
    void Update()
    {
        //-----------------------------------------------------------
        // �}�E�X���͎擾(0���A1�E�A2�z�C�[��)
        // ���{�^���������ꂽ�u��
        if (Input.GetMouseButtonDown(0))
        {
            // �X�N���[�����W�����[���h���W�ɕϊ����Ċi�[
            StartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        // ���{�^���������ꂽ�u��
        if (Input.GetMouseButtonUp(0))
        {
            EndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isRelease = true;
        }

        //-----------------------------------------------------------
        // �{�^���������ꂽ�u�Ԃ�������
        if (isRelease == true)
        {
            // �}�X���ƂɃz���O������ݒu����

            // �v���C���[�z���O����
            // ��̃^�C�����ɂ����܂��Ă�����
            //if(Mathf.Abs(StartPos.x - EndPos.x) < fTileScale && Mathf.Abs(StartPos.y - EndPos.y) < fTileScale)


            // start�̕����������l�ɂȂ�悤�ɂ���
            if(StartPos.x > EndPos.x)
            {
                (StartPos.x, EndPos.x) = (EndPos.x, StartPos.x);
            }
            if (StartPos.y > EndPos.y)
            {
                (StartPos.y, EndPos.y) = (EndPos.y, StartPos.y);
            }

            // �����_�؂�̂�
            Vector3Int IntStartPos = new Vector3Int(Mathf.FloorToInt(StartPos.x), Mathf.FloorToInt(StartPos.y), Mathf.FloorToInt(StartPos.z));
            Vector3Int IntEndPos = new Vector3Int(Mathf.FloorToInt(EndPos.x), Mathf.FloorToInt(EndPos.y), Mathf.FloorToInt(EndPos.z));
            // �^�C���T�C�Y�̔���
            float HalfScale = fTileScale / 2.0f;

            int SubVertical = Mathf.Abs(IntStartPos.y - IntEndPos.y); // �c�̍���
            int SubHorizontal = Mathf.Abs(IntStartPos.x - IntEndPos.x); // ���̍���

            if (IntStartPos.x == IntEndPos.x && IntStartPos.y == IntEndPos.y)
            {
                // �����ʒu
                Vector3 SpawnPos = new Vector3(IntStartPos.x + HalfScale, IntStartPos.y + HalfScale, 0.0f);

                // �v���C���[�z���O��������
                Instantiate(Holo_Player, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
            }

            // �c�̍����Ɖ��̍������r�A���̕�������������΃z�����𐶐��A���Əc�̍����������Ȃ�ǗD��ɂȂ�
            else if(SubHorizontal > SubVertical) // �� > �c
            {
                Vector3 SpawnPos;

                // �z��������
                for (int numX = 0; numX < SubHorizontal + 1; numX++)
                {
                    if (bGenerationMethosd)
                    {
                        for (int numY = 0; numY < SubVertical + 1; numY++)
                        {
                            SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale + numY, 0.0f);
                            // ���z���O��������
                            Instantiate(Holo_Floor, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    else
                    {
                        SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale, 0.0f);
                        // ���z���O��������
                        Instantiate(Holo_Floor, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }
            else
            {
                // �z���ǐ���
                Vector3 SpawnPos;

                // �z��������
                for (int numY = 0; numY < SubVertical + 1; numY++)
                {
                    if (bGenerationMethosd)
                    {
                        for (int numX = 0; numX < SubHorizontal + 1; numX++)
                        {
                            SpawnPos = new Vector3(IntStartPos.x + HalfScale + numX, IntStartPos.y + HalfScale + numY, 0.0f);
                            // ���z���O��������
                            Instantiate(Holo_Wall, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    else
                    {
                        SpawnPos = new Vector3(IntStartPos.x + HalfScale, IntStartPos.y + HalfScale + numY, 0.0f);

                        // ���z���O��������
                        Instantiate(Holo_Wall, SpawnPos, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }

            isRelease = false;
        }

    }


}
