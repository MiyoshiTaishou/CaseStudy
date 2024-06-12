using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GroundCheck : MonoBehaviour
{
    [Header("�n�ʂɓ������Ă邩"), SerializeField]
    private bool isGround = false;

    public List<GameObject> colList = new List<GameObject>();

    public bool GroundCheck()
    {
        return isGround;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround)
        {
            //Debug.Log("������" + gameObject.transform.parent.gameObject.name);
        }
        else
        {
            //Debug.Log("�͂���" + gameObject.transform.parent.gameObject.name);

        }

        // �R���C�_�[���X�g�̗v�f���������炠��Βn�ʂƐڐG���Ă��邱�ƂɂȂ�
        if(colList.Count > 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        foreach(var a in colList)
        {
            Debug.Log(a.name);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ڐG���肵�����^�O
        // �R���C�_�[���X�g�ɓo�^����Ă��Ȃ����
        if(/*(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Hologram")) &&*/
            !colList.Contains(collision.gameObject))
        {
            // ���X�g�ɓo�^
            colList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �ڐG���肵�����^�O
        // �R���C�_�[���X�g�ɓo�^����Ă��Ȃ����
        if (/*(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Hologram")) &&*/
            colList.Contains(collision.gameObject))
        {
            // ���X�g����폜
            colList.Remove(collision.gameObject);
        }
    }
}