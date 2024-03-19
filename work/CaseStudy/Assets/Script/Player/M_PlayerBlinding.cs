using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڂ���܂�����
public class M_PlayerBlinding : MonoBehaviour
{
    [Header("������I�u�W�F�N�g"), SerializeField]
    private GameObject BlindingObj;

    [Header("�������"), SerializeField]
    private float fThrowPower = 5.0f;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }

    //�����鏈��
    void Throw()
    {
        //�v���n�u���琶������
        GameObject blinding = Instantiate(BlindingObj, transform.position, Quaternion.identity);

        Rigidbody2D rb = blinding.GetComponent<Rigidbody2D>();

        // �΂ߏ�ɗ͂�������
        rb.AddForce(this.transform.up * fThrowPower, ForceMode2D.Impulse);
    }
}
