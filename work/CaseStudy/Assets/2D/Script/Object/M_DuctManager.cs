using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_�N�g���Ǘ�����
/// </summary>

public class M_DuctManager : MonoBehaviour
{
    // GameObject ���L�[�Ƃ��Abool ��l�Ƃ��鎫��
    private static Dictionary<GameObject, bool> ductDictionary = new Dictionary<GameObject, bool>();

    /// <summary>
    /// �v���C���[
    /// </summary>
    private GameObject PlayerObj;

    /// <summary>
    /// �v���C���[�������Ă��郌���_��
    /// </summary>
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();


    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");

        if (!PlayerObj)
        {
            Debug.Log("�v���C���[��������܂���");
        }

        // "Duct" �^�O���t���Ă��邷�ׂĂ� GameObject ���擾���Ď����ɒǉ�
        GameObject[] ducts = GameObject.FindGameObjectsWithTag("Duct");
        foreach (GameObject duct in ducts)
        {
            // �e GameObject ���L�[�Ƃ��āA�ŏ��� false �Őݒ肵����Ԃ������ɒǉ�
            ductDictionary.Add(duct, false);
        }

        // �v���C���[�������Ă���SpriteRenderer��S�Ď擾
        SpriteRenderer[] spriteRenderers = PlayerObj.GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length == 0)
        {
            Debug.Log("�����Ă܂���");
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            renderers.Add(spriteRenderer);
        }
    }

    private void Update()
    {
        if(ductDictionary.ContainsValue(true) && Input.GetButtonDown("Duct"))
        {           
            Debug.Log("�_�N�g���o��");

            // �����̃L�[�̃R���N�V�������擾
            var keys = new List<GameObject>(ductDictionary.Keys);

            // �L�[�̃R���N�V������Ŕ����������s���A�l��ύX
            foreach (var key in keys)
            {
                ductDictionary[key] = false;
            }

            // �_�N�g����o����v���C���[�̌��̃��C���[�ɖ߂�
            PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");

            //������悤�ɂ���
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = true;
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            PlayerObj.GetComponent<CircleCollider2D>().enabled = true;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }
    }
    //M_DuctWarp����Ă΂�Ďw�肵���Ƃ���܂Ńv���C���[���΂�
    public void DuctWarp(GameObject _duct1, GameObject _duct2)
    {
        PlayerObj.transform.position = _duct1.transform.position;

        // _duct1 �� true �ɁA_duct2 �� false �ɐݒ�
        SetContains(_duct2, false);
        SetContains(_duct1, true);       
    }

    // �_�N�g�̏�Ԃ��X�V���郁�\�b�h
    public void SetContains(GameObject duct, bool state)
    {
        ductDictionary[duct] = state;

        // �_�N�g�̏�Ԃ��ς������Ƀv���C���[�̋����𐧌䂷��
        if (ContainsTrueValue())
        {
            Debug.Log("�_�N�g�ɓ�����");

            // �_�N�g�ɓ�������v���C���[�̃��C���[��ύX
            PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            PlayerObj.transform.position = duct.transform.position;
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //�����Ȃ��悤�ɂ���
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = false;
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<CircleCollider2D>().enabled = false;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 0.0f;          
        }
        else
        {
            Debug.Log("�_�N�g���o��");

            // �_�N�g����o����v���C���[�̌��̃��C���[�ɖ߂�
            PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");

            //������悤�ɂ���
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].enabled = true;
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            PlayerObj.GetComponent<CircleCollider2D>().enabled = true;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }
    }

    // �������� true �̒l�����݂��邩���m�F���郁�\�b�h
    public bool ContainsTrueValue()
    {
        // �����̒l�̃R���N�V������ true ���܂܂�Ă��邩���m�F
        return ductDictionary.ContainsValue(true);
    }

    public bool GetValue(GameObject _duct)
    {
        return ductDictionary[_duct];
    }
}
