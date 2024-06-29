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
    /// �ړ������ǂ���
    /// </summary>
    private bool isMove = false;

    /// <summary>
    /// �������u��
    /// </summary>
    private bool isDuctIn = false;

    public bool isNowDuct = false;

    public bool GetNowDuct()
    {
        return isNowDuct;
    }

    public void SetIsMove(bool _move) { isMove = _move; }

    public bool GetIsMove() { return isMove; }

    public void PlayDuctInSE() { GetComponent<AudioSource>().Play(); }

    /// <summary>
    /// �v���C���[
    /// </summary>
    private GameObject PlayerObj;
    private GameObject PlayerRes;

    /// <summary>
    /// �v���C���[�������Ă��郌���_��
    /// </summary>
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    private Animator animator;
    private bool init = false;
    private bool warp = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        PlayerRes = GameObject.Find("PlayerRespawn");

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
        //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
        PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
        PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
        PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

        isNowDuct = false;
    }

    private void Update()
    {
        if (!init)
        {
            animator = PlayerObj.transform.GetChild(3).GetComponent<Animator>();

            init = false;
        }

        GameObject ductObj = GetObjectInDictionary();
        bool isRockDuct = false;
        if (GetObjectInDictionary() != null)
        {
            //Debug.Log("null����Ȃ�");
            isRockDuct = ductObj.GetComponent<M_DuctWarp>().GetisRock();

        }

        if (ductDictionary.ContainsValue(true) && Input.GetButtonDown("Duct") && !isMove && !isDuctIn && !isRockDuct && !animator.GetBool("duct"))
        {           
            Debug.Log("�_�N�g���o��");

            GetComponent<AudioSource>().Play();

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
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

            animator.SetBool("ductFinish", true);

            isNowDuct = false;

        }

        // �_�N�g�����b�N����Ă�����
        if (ductDictionary.ContainsValue(true) && Input.GetButtonDown("Duct") && !isMove && !isDuctIn && !isRockDuct)
        {
            // se�Đ�

        }

        if (ductDictionary.ContainsValue(true))
        {
            // �_�N�g�ɓ�������v���C���[�̃��C���[��ύX
            PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (isDuctIn && !warp)
            {
                animator.SetBool("duct", true);

            }

            if (!animator.GetBool("duct") && !isDuctIn)
            {
                //�����Ȃ��悤�ɂ���
                for (int i = 0; i < renderers.Count; i++)
                {
                    renderers[i].enabled = false;
                }
            }
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerMove>().SetIsNowDahs(false);
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(false);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = false;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = false;

        }

        warp = false;
        isDuctIn = false;
    }
    //M_DuctWarp����Ă΂�Ďw�肵���Ƃ���܂Ńv���C���[���΂�
    public void DuctWarp(GameObject _duct1, GameObject _duct2)
    {
        PlayerObj.transform.position = _duct1.transform.position;

        warp = true;

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

            //GetComponent<AudioSource>().Play();

            // �_�N�g�ɓ�������v���C���[�̃��C���[��ύX
            PlayerObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            Vector3 vecPos = duct.transform.position;
            PlayerObj.transform.position = new Vector3(vecPos.x, vecPos.y, -1.0f);
            PlayerObj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            ////�����Ȃ��悤�ɂ���
            //for (int i = 0; i < renderers.Count; i++)
            //{
            //    renderers[i].enabled = false;
            //}
            PlayerObj.GetComponent<M_PlayerMove>().FullStamina();
            PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(false);
            PlayerObj.GetComponent<M_PlayerMove>().SetIsNowDahs(false);
            //PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(false);
            PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(false);
            PlayerObj.GetComponent<BoxCollider2D>().enabled = false;
            PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            PlayerRes.GetComponent<BoxCollider2D>().enabled = false;

            //Debug.Log(PlayerObj.GetComponent<M_PlayerMove>().GetIsMove());

            isNowDuct = true;

            isDuctIn = true;
        }
        else
        {
            //Debug.Log("�_�N�g���o��");

            //// �_�N�g����o����v���C���[�̌��̃��C���[�ɖ߂�
            //PlayerObj.layer = LayerMask.NameToLayer("PlayerLayer");            

            ////������悤�ɂ���
            //for (int i = 0; i < renderers.Count; i++)
            //{
            //    renderers[i].enabled = true;
            //}
            //PlayerObj.GetComponent<M_PlayerMove>().SetIsMove(true);
            ////PlayerObj.GetComponent<M_PlayerThrow>().SetIsThrow(true);
            //PlayerObj.GetComponent<N_ProjecterSympathy>().SetIsPossible(true);
            //PlayerObj.GetComponent<BoxCollider2D>().enabled = true;
            //PlayerObj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            //PlayerRes.GetComponent<BoxCollider2D>().enabled = true;

            //isNowDuct = false;
        }
    }

    // true�ɂȂ��Ă���_�N�g�̃I�u�W�F�N�g��Ԃ�
    public GameObject GetObjectInDictionary()
    {
        // �_�N�g��������true�ɂȂ��Ă���I�u�W�F�N�g��T���ĕԂ�
        foreach(var obj in ductDictionary)
        {
            // bool���擾
            bool InDuct = obj.Value;

            // �L�[���擾
            if (InDuct)
            {
                return obj.Key;
            }
        }

        return null;
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

    public bool GetIsDuctIn()
    {
        return isDuctIn;
    }
}
