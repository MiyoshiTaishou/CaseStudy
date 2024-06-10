using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//�v���C���[�̈ړ��֘A�̏���
public class M_PlayerMove : MonoBehaviour
{
    [Header("�ړ����x"), SerializeField]
    private float fMoveSpeed = 10.0f;

    enum DASHMODE
    {
        DASH,     // �����`��
        TELEPORT, // �u�Ԉړ��`��
    }

    [Header("�_�b�V���̃��[�h"), SerializeField]
     DASHMODE DashMode = DASHMODE.DASH;

    [Header("�_�b�V�����x"), SerializeField]
    private float fDashSpeed = 10.0f;

    [Header("���t���[�������ăe���|�[�g���邩"), SerializeField]
    private int iTeleportFlame = 5;

    [Header("�e���|�[�g����"), SerializeField]
    private float fTeleportDistance = 3.0f;

    private int NowTeleportFlame = 0;
    private bool isNowTeleport = false;
    private bool ButtonTrigger = false;

    private float fStamina;

    [Header("�X�^�~�i�֘A")]
    [Header("�X�^�~�i�ő�l"),SerializeField] private float fStaminaMax = 100.0f;
    [Header("�X�^�~�i�����"),SerializeField] private float fUseDashStamina = 1.0f;
    [Header("�X�^�~�i�񕜑��x"),SerializeField] private float fRecoverySpeed = 2.0f;
    [Header("�ҋ@���X�^�~�i�񕜑��x"), SerializeField] private float fStayRecoverySpeed = 4.0f;
    [Header("�X�^�~�i���g���؂�����̉񕜑��x"), SerializeField] private float fUsedStaminaRecoverySpeed = 1.0f;

    [Header("�X�^�~�iUI�֘A")]
    [Header("�g���؂����Ƃ��̐F"), SerializeField] Color UseColor;
    [Header("���ӐF"), SerializeField] Color CautionColor;
    [Range(0, 100), Header("�ǂ̊������璍�ӐF�ɂ��邩"), SerializeField] private float CautionParcent = 40.0f;
    [Header("�x���F"), SerializeField] Color WarningColor;
    [Range(0, 100), Header("�ǂ̊�������x���F�ɂ��邩"), SerializeField] private float WarningParcent = 20.0f;

    /// <summary>
    /// �X�^�~�iUI�֘A
    /// </summary>
    private Image StaminaImage;

    /// <summary>
    /// �����̐F
    /// </summary>
    private Color StaminaColor;

    /// <summary>
    /// �X�^�~�i���񕜂����������ǂ���
    /// </summary>
    private bool isStamina = true;

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private bool isDash = true;
  

    private Rigidbody2D rbPlayer;
   
    /// <summary>
    /// �ǂ���������Ă��邩
    /// </summary>
    private Vector3 vecDir;

    public Vector3 GetDir() { return vecDir; }
    public void SetDir(Vector3 _dir) { vecDir = _dir; }

    /// <summary>
    /// �ړ��\��
    /// </summary>
    private bool isMove = true;

    /// <summary>
    /// �_�b�V�����Ă��邩
    /// </summary>
    private bool isNowDash = true;

    public void SetIsNowDahs(bool _dash) { isNowDash = _dash; }

    private Transform PlayerTrans;

    public bool GetIsDash() { return isNowDash; }

    public bool GetIsMove() { return isMove; }
    public void SetIsMove(bool _move) { isMove = _move; }

    /// <summary>
    ///�A�j���[�V�����֘A
    /// </summary>
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        vecDir = transform.right;

        PlayerTrans = gameObject.transform;

        //�X�^�~�i���ő�ɂ���
        fStamina = fStaminaMax;

        //UI��T��
        StaminaImage = GameObject.Find("StaminaBar").GetComponent<Image>();
        StaminaColor = StaminaImage.color;

        // �q�I�u�W�F�N�g�̒�����A�A�j���[�^�[���擾
        animator = gameObject.transform.GetChild(3).GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {        
        if(!isMove || !M_GameMaster.GetGamePlay())
        {
            isDash = false;
            return;
        }

        // �L�[�{�[�h���͂��󂯎��
        float fHorizontalInput = Input.GetAxis("Horizontal");
       
        //�_�b�V���{�^���������Ă��邩
        //if(Input.GetButton("DashButton"))
        if(Input.GetAxis("DashButton") < 0)
        {
            if (!isNowTeleport && !ButtonTrigger)
            {
                // �e���|�[�g�p�ϐ�
                isNowTeleport = true;
                ButtonTrigger = true;
            }

            // �_�b�V���p�ϐ�
            isDash = true;
        }
        else
        {
            isDash = false;
            ButtonTrigger = false;
        }

        //�ړ��������Ă�
        switch (DashMode)
        {
            case DASHMODE.DASH:
                MoveUpdate(fHorizontalInput);
                //UI�������Ă�
                StaminaUIUpdate();
                break;

            case DASHMODE.TELEPORT:
                Teleport(fHorizontalInput);
                break;
        }
    }

    // �u�Ԉړ�
    private void Teleport(float _fhorizontal)
    {
        // �e���|�[�g��
        if (isNowTeleport && NowTeleportFlame < iTeleportFlame)
        {
            // ���̃t���[���ł̈ړ�����
            float moveDis = fTeleportDistance / iTeleportFlame;

            if (vecDir.x > 0)
            {
                PlayerTrans.position += new Vector3(moveDis, 0.0f, 0.0f);
            }
            else if (vecDir.x < 0)
            {
                PlayerTrans.position -= new Vector3(moveDis, 0.0f, 0.0f);
            }

            NowTeleportFlame++;

            // �e���|�[�g�p
            // �w��t���[�����o������
            if (NowTeleportFlame >= iTeleportFlame)
            {
                // �e���|�[�g�I��
                isNowTeleport = false;
                NowTeleportFlame = 0;
            }
        }
        else
        {
            // ���͂Ɋ�Â��Ĉړ�����
            Vector2 vecMoveDirection = new Vector2(_fhorizontal * fMoveSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;
            animator.SetBool("run", true);

            if (_fhorizontal > 0.0f)
            {
                vecDir = new Vector3(1.0f,0.0f,0.0f);
                transform.eulerAngles = Vector3.zero;
            }
            else if (_fhorizontal < 0.0f)
            {
                vecDir = new Vector3(-1.0f,0.0f,0.0f);
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
        }
    }

    //�ړ��֘A�̏���������
    private void MoveUpdate(float _forizontal)
    {
        //�X�^�~�i���g���؂��Ă��Ȃ��đ���{�^���������Ă��鎞
        if (isDash && isStamina)
        {
                    
            // ���͂Ɋ�Â��Ĉړ�����
            Vector2 vecMoveDirection = new Vector2(_forizontal * fDashSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;            

            if (_forizontal > 0.0f)
            {
                vecDir = transform.right;
                transform.eulerAngles = Vector3.zero;
            }
            else if (_forizontal < 0.0f)
            {
                vecDir = -transform.right;
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }

            //�ړ����Ă���ꍇ�̓X�^�~�i�������
            if (_forizontal != 0.0f)
            {
                fStamina -= fUseDashStamina * Time.deltaTime;
                isNowDash = true;
                animator.SetBool("glider", true);
                animator.SetBool("run", false);
            }
            else
            {
                fStamina += fStayRecoverySpeed * Time.deltaTime;
                isNowDash = false;
                animator.SetBool("glider", false);
                animator.SetBool("run", false);

            }
        }
        else
        {
            // ���͂Ɋ�Â��Ĉړ�����
            Vector2 vecMoveDirection = new Vector2(_forizontal * fMoveSpeed, rbPlayer.velocity.y);
            rbPlayer.velocity = vecMoveDirection;
            animator.SetBool("run", true);
            animator.SetBool("glider", false);

            if (_forizontal > 0.0f)
            {
                vecDir = transform.right;
                transform.eulerAngles = Vector3.zero;
            }
            else if (_forizontal < 0.0f)
            {
                vecDir = -transform.right;
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }

            //�g�����������ҋ@���Ă��邩�����Ă��邩�ŉ񕜑��x��ς���
            if (isStamina)
            {
                if (_forizontal == 0.0f)
                {                    
                    fStamina += fStayRecoverySpeed * Time.deltaTime;
                    animator.SetBool("run", false);
                    animator.SetBool("glider", false);
                }
                else
                {                    
                    fStamina += fRecoverySpeed * Time.deltaTime;
                }
            }
            else
            {               
                fStamina += fUsedStaminaRecoverySpeed * Time.deltaTime;
            }

            isNowDash = false;
        }

        //�X�^�~�i���ő�܂ŉ񕜂�����
        if (fStamina > fStaminaMax)
        {
            fStamina = fStaminaMax;
            isStamina = true;
        }

        //�X�^�~�i���g���؂�����
        if (fStamina < 0)
        {
            fStamina = 0;
            isStamina = false;
        }

        if (_forizontal == 0.0f && Input.GetAxis("DashButton") == 0)
        {            
            animator.SetBool("run", false);
        }       
    }

    //UI�֘A�̏���������
    private void StaminaUIUpdate()
    {
        //UI�̐L�т��v�Z
        StaminaImage.fillAmount = fStamina / fStaminaMax;

        //���ɂ���
        float StamineParcent = (fStamina / fStaminaMax) * 100;

        //�X�^�~�i���g���؂������͐F��ς���
        if(!isStamina)
        {
            StaminaImage.color = UseColor;

            return;
        }

        if(StamineParcent > CautionParcent)
        {
            StaminaImage.color = StaminaColor;

            return;
        }

        //�x�����C����肵���Ȃ�F��ς���
        if (StamineParcent < WarningParcent)
        {
            StaminaImage.color = WarningColor;

            return;
        }

        //���Ӄ��C����肵���Ȃ�F��ς���
        if (StamineParcent < CautionParcent)
        {
            StaminaImage.color = CautionColor;

            return;
        }      
    }
}
