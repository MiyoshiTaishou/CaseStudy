using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_PlayerCreateFallacies : MonoBehaviour
{
    //���C�̒���(���u��)
    private const float fRayLength = 0.5f;
    //�v���C���[���a(���u��)
    private const float fPlayerRad = 1.0f;

    //�v���C���[�̌���(-1�͍��A1�͉E�������Ă���)
    private int iPlayreDirection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̌��������߂�
        float fHorizontalInput = Input.GetAxis("Horizontal");
        if(fHorizontalInput<0.0f)
        {
            iPlayreDirection = -1;
        }
        if(0.0f< fHorizontalInput)
        {
            iPlayreDirection = 1;
        }

        //F�������ꂽ��
        if(Input.GetKeyDown(KeyCode.F))
        {
            //���C��΂�
            Vector3 RayPos = new Vector3(transform.position.x, transform.position.y - fPlayerRad, transform.position.z);
            RaycastHit2D raycastHit = Physics2D.Raycast(RayPos, Vector2.down, fRayLength);
            if (raycastHit)//����������
            {
                //�ڐG���Ă���I�u�W�F�N�g�̖��O���擾
                string objectName = raycastHit.collider.gameObject.name;

                //�擾�������O�ƌ�������ɏ����ׂ��������߂�

                //�����^�C���̔ԍ�
                int index = GetIndexFromObjectName(objectName) + iPlayreDirection;
       
                //�����ׂ����̖��O
                string selectedObjectName = "Tile" + index.ToString();

                // �w�肵�����O�̃I�u�W�F�N�g���������A���݂���ꍇ�͔j������
                GameObject objectToDelete = GameObject.Find(selectedObjectName);
                if (objectToDelete != null)
                {
                    Destroy(objectToDelete);
                    //���O�o��(debug)
                    Debug.Log("�ڐG���Ă��鏰�@: " + objectName);
                    Debug.Log("���������ԍ��@: " + index);
                }
            }
        }
    }

    // �I�u�W�F�N�g�̖��O����C���f�b�N�X���擾���郆�[�e�B���e�B���\�b�h
    int GetIndexFromObjectName(string objectName)
    {
        string indexString = objectName.Substring("Tile".Length);
        return int.Parse(indexString);
    }
}
