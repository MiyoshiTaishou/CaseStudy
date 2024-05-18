using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������ꂽ�Ƃ���BGM�ύX
/// ��ʉ��o
/// </summary>
public class M_PlayerWarning3DK : MonoBehaviour
{
    [Header("���f����Image"), SerializeField]
    private Image img;

    [Header("�����������̉�ʂ̐F"), SerializeField]
    private Color color;

    [Header("�F�̕ς�鑬�x"), SerializeField]
    private float fColorSpeed;
   
    /// <summary>
    /// ��������Ă��邩
    /// </summary>
    private bool isFound = false;

    /// <summary>
    /// �w�肵���F�ɂȂ�����
    /// </summary>
    private bool isColor = false;

    /// <summary>
    /// ���Ԍv��
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// �͈͓��ɂ���G�l�~�[�̃��X�g
    /// </summary>
    private List<GameObject> EnemyList = new List<GameObject>();

    /// <summary>
    /// BGM�؂�ւ��I�u�W�F�N�g
    /// </summary>
    private GameObject BGM;

    /// <summary>
    /// BGM�؂�ւ�����
    /// </summary>
    private bool isSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        //�����͓����ɂ���
        img.color = Color.clear;

        BGM = GameObject.Find("SwitchBGM");
    }

    // Update is called once per frame
    void Update()
    {
        List<int> array = new List<int>();
        int i = 0;
        //�͈͓��̃G�l�~�[�Ɍ������Ă��邩�ǂ����`�F�b�N������
        foreach (GameObject obj in EnemyList)
        {
            if(obj == null)
            {
                array.Add(i);

                // ���̃��[�v�փX�L�b�v
                continue;
            }
            //if(obj.GetComponent<MPlayerSearch>().GetIsSearch())
            if(obj.GetComponent<N_PlayerSearch>().GetIsSearch())
            {
                //���������ꍇ��true�ɂ��Ĕ�����
                isFound = true;

                //Debug.Log("����������");
                break;
            }
            else
            {
                isFound = false;
            }
        }

        // �폜���ꂽ�I�u�W�F�N�g�����X�g���珜�O
        foreach(var num in array)
        {
            EnemyList.RemoveAt(num);
        }
      
        if(isFound)
        {
            Found();

            //Debug.Log("��������");

            BGM.GetComponent<M_SwitchBGM>().ChangeBGM(true);           
        }
        else
        {           
            //�������ĂȂ����͓���
            img.color = Color.clear;
                  
            BGM.GetComponent<M_SwitchBGM>().ChangeBGM(false);      
        }      
    }

    /// <summary>
    /// �������Ă��鎞�̉��o
    /// </summary>
    void Found()
    {
        fTime += fColorSpeed * Time.deltaTime;

        //�F��ς��鏈��
        if(isColor)
        {
            img.color = Color.Lerp(color, Color.clear, fTime);
        }
        else
        {           
            img.color = Color.Lerp(Color.clear, color, fTime);
        }
        
        if(fTime > 1.0f)
        {
            fTime = 0.0f;
            isColor = !isColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //�܂����X�g�ɂȂ���������
            if (!EnemyList.Contains(collision.gameObject))
            {
                EnemyList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //�܂����X�g�ɂȂ���������
            if (EnemyList.Contains(collision.gameObject))
            {
                EnemyList.Remove(collision.gameObject);
            }
        }
    }  
}
