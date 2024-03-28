using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڂ���܂�����
public class M_PlayerThrow : MonoBehaviour
{
    [Header("������I�u�W�F�N�g"), SerializeField]
    private GameObject BlindingObj;

    [Header("�������"), SerializeField]
    private float fThrowPower = 5.0f;    

    /// <summary>
    /// ������邩
    /// </summary>
    private bool isThrow = true;

    public bool GetIsThrow() { return isThrow; }

    public void SetIsThrow(bool isThrow) {  this.isThrow = isThrow; }
   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isThrow)
        {
            Throw();
        }
    }

    //�����鏈��
    void Throw()
    {
        Vector3 vecInstacePos = new Vector3(transform.position.x + GetComponent<M_PlayerMove>().GetDir().x, transform.position.y, transform.position.z);
        //�v���n�u���琶������
        GameObject blinding = Instantiate(BlindingObj, vecInstacePos, Quaternion.identity);

        Rigidbody2D rb = blinding.GetComponent<Rigidbody2D>();

        // �΂ߏ�ɗ͂�������
        rb.AddForce(new Vector2(GetComponent<M_PlayerMove>().GetDir().x,1.0f) * fThrowPower, ForceMode2D.Impulse);
    }
}
