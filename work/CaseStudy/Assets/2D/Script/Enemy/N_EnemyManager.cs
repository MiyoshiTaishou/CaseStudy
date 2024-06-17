using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�̑��񂲂ƂɈ��
// ����̓������Ǘ�����

public class N_EnemyManager : MonoBehaviour
{
    [Header("����ɏ�������G�v���n�u"), SerializeField]
    private List<GameObject> TeamMembers = new List<GameObject>();

    private int iMemberNum = 0;

    public bool IsReflectionX = false;

    // ����̈�тł��J�����Ɉڂ��Ă�����true
    private bool isLook = false;

    private Transform thisTrans;

    public List<SEnemyMove> sEnemyMoves = new List<SEnemyMove>();

    [System.Serializable]
    struct ManagerMoveStatus
    {
        [Header("���b"), SerializeField]
        public float MoveTime;

        [Header("�ړ����x"), SerializeField]
        public float MoveSpeed;

        [Header("�ǐՎ��̈ړ����x"), SerializeField]
        public float ChaseSpeed;

        [Header("�����]�����ҋ@����"), SerializeField]
        public float WaitTime;

        [Header("������ǐՂ܂ł̎���"), SerializeField]
        public float FoundTime;

        [Header("�������ď���ɖ߂鎞��"), SerializeField]
        public float LostSightTime;

    }
    // �G�̈ړ������ς���
    // �w��b�����ʂɈړ�����
    [Header("�ړ��X�e�[�^�X"), SerializeField]
    ManagerMoveStatus managerStatus;

    //�ۑ����ꂽ���񕪗��O�̑ҋ@����
    [Header("���[��ǂ�����"),SerializeField]
    private float OldWaitTime=0.2f;
    // �o�ߎ���
    private float ElapsedTime = 0.0f;
    private float ElapsedWaitTime = 0.0f;
    private float ElapsedFoundTime = 0.0f;
    private float ElapsedLostSightTime = 0.0f;

    private bool IsDitection = false;

    private bool IsRef = false;

    private bool init = false;

    // �}�l�[�W���[�̐�����
    private static int GenerateOrder = 0;
    // ������
    private int GenerateNumber = 0;

    public int GetGenerateNumber()
    {
        return GenerateNumber;
    }

    //�������ꂽ���񂩂ǂ���
    private bool isClone = false;

    // �X�e�[�g�}�V��
    public enum ManagerState
    {
        IDLE,       // �A�C�h��
        PATOROL,    // ����
        WAIT,       // �ҋ@
        ECPULSION,  // ����
        FOUND,      // ����
        CHASEINIT,  // �ǐՏ���
        CHASE,      // �ǐ�
        LOSTSIGHT,  // ��������
        Warped,     // ���[�v����
    }

    [SerializeField]
    private ManagerState managerState = ManagerState.PATOROL;

    // �ǐՑΏۂ̍��W
    private GameObject Target;
    private Transform TargetTrans;

    private N_PostProcess postProcess;

    // �ړ��X�e�[�^�X������
    public void InitMoveStatus()
    {
        managerStatus.MoveTime = 3.0f;
        managerStatus.MoveSpeed = 4.0f;
        managerStatus.ChaseSpeed = 4.0f;
        managerStatus.WaitTime = 0.5f;
        managerStatus.FoundTime = 0.5f;
        managerStatus.LostSightTime = 0.5f;
    }

    void SetMoveStatus(ManagerMoveStatus _sta)
    {
        managerStatus = _sta;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisTrans = this.transform;

        // ������
        //InitMoveStatus();

        // ����̐l�����v��
        CountMemberNum();
        // ������̓G�̈ړ��X�N���v�g���擾
        SetEnemyMoveScript();

        // ���O�Ń}�l�[�W���[����ʂł���悤�ɂ���
        this.gameObject.name = this.gameObject.name + GenerateOrder.ToString();
        GenerateNumber = GenerateOrder;
        GenerateOrder++;

        if (!isClone)
        {
            OldWaitTime = managerStatus.WaitTime;
            Debug.Log("�I�[���h�^�C��" + OldWaitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!init)
        {
            // ����̓G�ɔԍ���t�^
            SetInfomation(true);

            postProcess = GameObject.Find("Volume").GetComponent<N_PostProcess>();

            init = true;
        }

        if (iMemberNum == 0)
        {
            Destroy(gameObject);
            return;
        }

        switch (managerState)
        {
            case ManagerState.IDLE:
                break;

            case ManagerState.PATOROL:
                Patrol();
                break;

            case ManagerState.WAIT:
                Wait();
                break;

            case ManagerState.ECPULSION:
                //Ecpulsion();
                break;

            case ManagerState.FOUND:
                FoundTarget();
                break;

            case ManagerState.CHASEINIT:
                ChaseInit();
                break;

            case ManagerState.CHASE:
                Chase();
                break;

            case ManagerState.LOSTSIGHT:
                LostSight();
                break;
            case ManagerState.Warped:
                Warped();
                break;
        }
    }

    // ���������ȏ㗣�ꂽ��`�[���𕪂���
    public void PartitionTeamHeight()
    {
        // �}�l�[�W���[����
        GameObject manager = new GameObject();
        manager.transform.parent = thisTrans.parent.gameObject.transform;
        manager.name = "EnemyManager";
        N_EnemyManager sc_mana = manager.AddComponent<N_EnemyManager>();
        sc_mana.SetMoveStatus(managerStatus);
        sc_mana.ChangeManagerState(ManagerState.PATOROL);

        Vector3 pos = Vector3.zero;
        int order = 0;
        // �G�̍������擾
        foreach(var obj in TeamMembers)
        {
            if (IsMemberWarped() == null || (IsMemberWarped()!=null &&  IsMemberWarped() == obj))
            {
                Debug.Log("�����������`" + obj.name+"�Ȃ�ł�I�H"+obj.GetComponent<SEnemyMove>().GetIsWarped());
                // �ŏ��̓G��Y���W�����ɂ���
                if (order == 0)
                {
                    pos = obj.transform.position;

                    EcpulsionMember(obj.GetComponent<SEnemyMove>().GetTeamNumber(), managerState);

                    //���[�v��̑���z���Ɋւ��鏈���A�擪�̓G����񂩂番�􂳂���
                    sc_mana.TeamAddEnemy(obj);

                    if (obj.GetComponent<SEnemyMove>().GetWarp() && IsReflectionX == obj.GetComponent<SEnemyMove>().GetWarp().GetiswarpRight())
                    {
                        sc_mana.IsReflectionX = !IsReflectionX;

                        managerState = ManagerState.Warped;
                    }

                    if (obj.GetComponent<SEnemyMove>().GetIsWarped() == true)
                    {
                        sc_mana.isClone = true;
                        sc_mana.OldWaitTime = OldWaitTime;
                        //sc_mana.managerStatus.WaitTime = 0.001f;
                        sc_mana.managerStatus.WaitTime = OldWaitTime;
                    }
                    return;
                }

                // ������x�͈͓̔��̓G�̓`�[���ɒǉ�
                if (obj.transform.position.y <= pos.y + 0.1f && obj.transform.position.y >= pos.y - 0.1f)
                {
                    EcpulsionMember(obj.GetComponent<SEnemyMove>().GetTeamNumber(), managerState);
                    if (IsReflectionX == obj.GetComponent<SEnemyMove>().GetWarp().GetiswarpRight())
                    {
                        sc_mana.IsReflectionX = !IsReflectionX;
                        managerState = ManagerState.Warped;
                    }
                    // �V�`�[���Ɉړ�
                    sc_mana.TeamAddEnemy(obj);
                    Debug.Log("�����̈Ⴄ�V�`�[��");
                }

                order++;
            }
        }
    }

    // �z���O�����ɂ���ă`�[���𕪊����ꂽ��
    void PartitionTeam(int _num)
    {
        // �����̕K�v���Ȃ�
        if(iMemberNum <= 1)
        {
            return;
        }

        // �V�����}�l�[�W���[�I�u�W�F�N�g����
        GameObject manager = new GameObject();
        manager.transform.parent = thisTrans.parent.gameObject.transform;
        manager.name = "EnemyManager";
        N_EnemyManager sc_mana = manager.AddComponent<N_EnemyManager>();
        sc_mana.SetMoveStatus(managerStatus);

        int i = 0;
        // ����
        List<GameObject> otherTeam = new List<GameObject>();
        foreach(var obj in TeamMembers)
        {
            // �E�i�s��
            if (!IsReflectionX)
            {
                // ���m�����G����ɓo�^���ꂽ�G
                if (i > _num)
                {
                    otherTeam.Add(obj);
                }
            }
            // ���i�s��
            else
            {
                // ���m�����G�܂ߌ�ɓo�^���ꂽ�G
                if (i >= _num)
                {
                    otherTeam.Add(obj);
                }
            }
            i++;
        }

        // ������O�ꂽ�G�����X�g����폜
        // �E�i�s��
        if (!IsReflectionX)
        {
            // �G�Ƃ��������X�N���v�g�����X�g����폜
            TeamMembers.RemoveRange(_num + 1, iMemberNum - _num - 1);
            sEnemyMoves.RemoveRange(_num + 1, iMemberNum - _num - 1);

        }
        // ���i�s��
        else
        {
            TeamMembers.RemoveRange(_num, iMemberNum - _num);
            sEnemyMoves.RemoveRange(_num, iMemberNum - _num);
        }

        // ����̐l�����v��
        CountMemberNum();
        // ���g�̑���ɔԍ��ĕt�^
        SetInfomation(false);

        // �V�����}�l�[�W���[�̑���ɓG���Q��������
        sc_mana.TeamAddEnemys(manager,otherTeam);

        // �ǔ����҂�������̑����҂���Ԃɂ���
        if (!IsReflectionX)
        {
            ChangeManagerState(ManagerState.WAIT);
        }
        else
        {
            sc_mana.ChangeManagerState(ManagerState.WAIT);
        }
    }

    public void UnionTeam(N_EnemyManager _manager)
    {
        // �����̑���ɉ�����
        TeamAddEnemys(this.gameObject, _manager.GetTeamMember());

        // X���W�̏��������ɕ��ёւ�
        SortMember();

        sEnemyMoves.Clear();
        SetEnemyMoveScript();

        // ���g�̑���ɔԍ��ĕt�^
        SetInfomation(true);

        // �҂����Ԃɂ���
        bool  flg = false;
        foreach (var obj in TeamMembers) 
        {
            if(obj.GetComponent<SEnemyMove>().GetIsWarped()==true)
            {
                flg = true;
                //obj.GetComponent<SEnemyMove>().SetisWarped(false);
                Debug.Log("���������c");
            }
        }
        if(!flg) 
        {
            Debug.Log("��ׂ������I");
            managerState = ManagerState.WAIT;
        }
        else
        {
            managerStatus.WaitTime = OldWaitTime;
        }
        IsRef = true;

        // �G�̂��Ȃ��Ȃ����}�l�[�W���[�폜
        Destroy(_manager.gameObject);
    }

    // X���W���������ɕ��ёւ�
    private void SortMember()
    {
        List<GameObject> list = new List<GameObject>();

        for(int i = 0;i < iMemberNum; i++)
        {
            if (i == 0) {
                list.Add(TeamMembers[i]);
                //Debug.Log(TeamMembers[i].gameObject.name);

                continue;
            }

            for(int j = 0;j < i; j++)
            {
                // ���ɂ�����}��
                if(TeamMembers[i].transform.position.x < list[j].transform.position.x)
                {
                    list.Insert(j, TeamMembers[i]);
                    //Debug.Log(TeamMembers[i].gameObject.name);
                    break;
                }
                else
                {
                    // �V���X�g�ɍŌ�܂Œǉ�����Ȃ����
                    if(j == i - 1)
                    {
                        // �����ɒǉ�
                        list.Add(TeamMembers[i]);
                        //Debug.Log(TeamMembers[i].gameObject.name);
                    }
                }
            }
        }

        TeamMembers.Clear();
        TeamMembers.AddRange(list);
    }

    public void ChangeManagerState(ManagerState _state)
    {
        managerState = _state;
    }

    private void Patrol()
    {
        if(ElapsedTime == 0.0f)
        {
            // �S���A�N�V������\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().AllSetFalse();
                obj.transform.GetChild(2).GetComponent<Animator>().SetBool("dash", true);
            }
        }

        float dir = 1.0f;
        if (IsReflectionX == true)
        {
            dir *= -1.0f;
        }
        // ���̃t���[���ňړ����鋗��
        float distance = dir * managerStatus.MoveSpeed * Time.deltaTime;

        ElapsedTime += Time.deltaTime;

        // ��莞�Ԍo��
        if (ElapsedTime >= managerStatus.MoveTime)
        {
            ElapsedTime = 0.0f;
            // �҂���ԂɑJ��
            managerState = ManagerState.WAIT;
        }

        // ����̓G���ړ�������
        foreach (var enemy in sEnemyMoves)
        {
            enemy.EnemyMove(distance, IsReflectionX);
        }
    }

    // �҂����
    private void Wait()
    {
        if(ElapsedWaitTime == 0.0f)
        {
            // �S���A�N�V������\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().AllSetFalse();
                //obj.transform.GetChild(2).GetComponent<Animator>().SetBool("dash", true);
            }
        }


        ElapsedTime = 0.0f;
        ElapsedWaitTime += Time.deltaTime;
        //Debug.Log(ElapsedWaitTime);

        if(ElapsedWaitTime + Time.deltaTime >= managerStatus.WaitTime)
        {
            if (!IsRef)
            {
                IsReflectionX = !IsReflectionX;
                IsRef = true;
            }
        }

        // �҂�
        if (ElapsedWaitTime >= managerStatus.WaitTime)
        {
            ElapsedWaitTime = 0.0f;
            // ������
            managerState = ManagerState.PATOROL;
            IsRef = false;

            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(false);
            }
        }
        else
        {
            foreach (var enemy in sEnemyMoves)
            {
                enemy.EnemyMove(0.0f, IsReflectionX);
            }
        }
    }

    private void FoundTarget()
    {
        if(ElapsedFoundTime == 0.0f)
        {
            int num = 0;

            // �����Z�b�g
            foreach (var obj in TeamMembers)
            {
                if (TargetTrans.position.x < obj.transform.position.x)
                {
                    // �����ύX
                    sEnemyMoves[num].EnemyMove(0.0f, true);
                }
                if (TargetTrans.position.x > obj.transform.position.x)
                {
                    // �����ύX
                    sEnemyMoves[num].EnemyMove(0.0f, false);
                }
                num++;
            }

            // �r�b�N���}�[�N�\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsSearchTarget(true);
                obj.transform.GetChild(2).GetComponent<Animator>().SetTrigger("sight");
            }
        }

        // �G�𔭌����莞�Ԏw�����m�F
        ElapsedFoundTime += Time.deltaTime;

        // �ǐՏ�ԂɑJ��
        if(ElapsedFoundTime >= managerStatus.FoundTime)
        {
            // �������n
            ElapsedFoundTime = 0.0f;

            managerState = ManagerState.CHASEINIT;

            // �r�b�N���}�[�N��\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsSearchTarget(false);
            }
        }
    }

    private void ChaseInit()
    {
        postProcess.AddSerachNum();

        managerState = ManagerState.CHASE;
    }

    private void Chase()
    {
        int num = 0;

        float dis = 0.0f;

        foreach (var obj in TeamMembers) {

            float temp = Mathf.Abs(TargetTrans.position.x - obj.transform.position.x);
            if (dis == 0.0f || temp < 1.0f)
            {
                dis = temp;
            }
            if (TargetTrans.position.x < obj.transform.position.x)
            {
                float dir = -1.0f;

                // ���̃t���[���ňړ����鋗��
                float distance = dir * managerStatus.ChaseSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            if(TargetTrans.position.x > obj.transform.position.x)
            {
                float dir = 1.0f;

                // ���̃t���[���ňړ����鋗��
                float distance = dir * managerStatus.ChaseSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            num++;
        }

        // �ǐՑΏۂ������������ԑJ��
        // �E�[�ƍ��[�̓G���������� = ���񂪌�������
        if(TeamMembers[0].transform.GetChild(0).GetComponent<N_PlayerSearch>().GetIsSearch() == false &&
            TeamMembers[iMemberNum - 1].transform.GetChild(0).GetComponent<N_PlayerSearch>().GetIsSearch() == false)
        {
            managerState = ManagerState.LOSTSIGHT;
        }
    }

    private void LostSight()
    {
        if (ElapsedLostSightTime == 0.0f)
        {
            // �N�G�X�`�����}�[�N�\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsLostTarget(true);
                obj.transform.GetChild(2).GetComponent<Animator>().SetBool("sight", true);

            }
        }

        ElapsedLostSightTime += Time.deltaTime;

        if(ElapsedLostSightTime >= managerStatus.LostSightTime)
        {
            // ������
            Target = null;
            managerState = ManagerState.PATOROL;
            ElapsedLostSightTime = 0.0f;

            postProcess.SubSerachNum();

            // �N�G�X�`�����}�[�N��\��
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsLostTarget(false);
                obj.transform.GetChild(2).GetComponent<Animator>().SetBool("sight", false);

            }
        }
    }

    private void Warped()
    {
        
        ElapsedTime = 0.0f;
        ElapsedWaitTime += Time.deltaTime;

        // �҂�
        if (ElapsedWaitTime >= 0.2f)
        {
            ElapsedWaitTime = 0.0f;
            // ������
            managerState = ManagerState.PATOROL;

            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(false);
            }
        }
    }
    public void SetTarget(GameObject _obj)
    {
        if (Target == null || Target != _obj)
        {
            Target = _obj;
            TargetTrans = Target.GetComponent<Transform>();
            managerState = ManagerState.FOUND;
        }
    }

    // ����
    private void Ecpulsion()
    {
        // ���������ҋ@
    }

    // �{�[���ɂȂ����G�����X�g����폜
    public void EcpulsionMember(int _number,ManagerState _state) 
    {
        // �}�l�[�W���[�Ǘ����O��
        sEnemyMoves[_number].NullEnemyManager();

        TeamMembers.RemoveAt(_number);
        sEnemyMoves.RemoveAt(_number);
        // ����̐l�����v��
        CountMemberNum();
        // ���g�̑���ɔԍ��ĕt�^
        SetInfomation(false);

        managerState = _state;
    }

    // ����ɓG������ǉ�����
    private void TeamAddEnemys(GameObject _parent,List<GameObject> _others)
    {
        // ���X�g�ɒǉ�
        TeamMembers.AddRange(_others);
        foreach(var obj in TeamMembers)
        {
            // �e�I�u�W�F�N�g�ύX
            obj.transform.parent = _parent.transform;
        }
        CountMemberNum();
    }

    public void TeamAddEnemy(GameObject _obj)
    {
        // ���X�g�ɒǉ�
        TeamMembers.Add(_obj);
        foreach(var obj in TeamMembers)
        {
            // �e�I�u�W�F�N�g�ύX
            obj.transform.parent = gameObject.transform;
        }
        CountMemberNum();

        // �X�N���v�g���������񃊃Z�b�g
        sEnemyMoves.Clear();

        // ������̓G�̈ړ��X�N���v�g���擾
        SetEnemyMoveScript();

        SetInfomation(false);
    }

    // �G���
    private void SetInfomation(bool _isNewMana)
    {
        for(int i = 0; i < iMemberNum; i++)
        {
            SEnemyMove move = sEnemyMoves[i];
            
            // ������̔ԍ��t�^
            move.SetNumber(i);
            if (_isNewMana)
            {
                // �V�����}�l�[�W���[��G�ɓo�^
                move.SetEnemyManager();
                move.EnemyMove(0.0f, IsReflectionX);
            }
            // ������̓G�̌������擾
            IsReflectionX = move.GetIsReflection();
        }
    }

    // ���łɂ���肷��X�N���v�g�����X�g�ɒǉ�
    private void SetEnemyMoveScript()
    {
        foreach(var member in TeamMembers)
        {
            SEnemyMove move = member.GetComponent<SEnemyMove>();
            // ���X�g�ɗv�f��������Βǉ�
            if (!sEnemyMoves.Contains(move))
            {
                // �X�N���v�g��ǉ�
                sEnemyMoves.Add(move);
            } 
        }
    }

    private void CountMemberNum()
    {
        iMemberNum = 0;
        foreach (var obj in TeamMembers)
        {
            iMemberNum++;
        }
    }

    // ����̓G����̂ł��J�������Ɉڂ����������̓G�͈ړ��J�n
    public void IsLook(bool _isLook)
    {
        if (_isLook)
        {
            foreach (var sc in sEnemyMoves)
            {
                // �ړ����̏����J�n
                sc.StartMove();
            }
        }
    }

    // ����̒N�����؂�Ԃ��˗������鎞�Ăяo��
    public void RequestRefletion()
    {
        //if (managerState == ManagerState.PATOROL)
        //{
        //    managerState = ManagerState.WAIT;
        //}
        managerState = ManagerState.WAIT;

    }

    // �z���O�����̕ǂ����m�������Ăяo��
    public void DetectionHologram(int _number)
    {
        if (IsDitection || managerState == ManagerState.WAIT)
        {
            //Debug.Log("�łĂ�");
            return;
        }
        IsDitection = true;

        // �E�ɐi�s��
        if (!IsReflectionX)
        {
            // ��ԉE�̓G���z���O�����̕ǂ����m
            if (_number == iMemberNum - 1)
            {
                managerState = ManagerState.WAIT;

                // �N�G�X�`�����}�[�N�\��
                foreach (var obj in TeamMembers)
                {
                    obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(true);
                }

            }
            else/* if(_number != 0)*/
            {
                // ���񕪊�
                PartitionTeam(_number);
            }
        }
        // ���ɐi�s��
        else
        {
            // ��ԍ��̓G���z���O�����̕ǂ����m
            if (_number == 0)
            {
                managerState = ManagerState.WAIT;
                // �N�G�X�`�����}�[�N�\��
                foreach (var obj in TeamMembers)
                {
                    obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(true);
                }
            }
            else/* if(_number != 0)*/
            {
                // ���񕪊�
                PartitionTeam(_number);
            }
        }
        IsDitection = false;
    }

    public float GetMoveSpeed()
    {
        return managerStatus.MoveSpeed;
    }

    public N_EnemyManager.ManagerState GetState()
    {
        return managerState;
    }

    public bool GetIsReflection()
    {
        return IsReflectionX;
    }

    private List<GameObject> GetTeamMember()
    {
        return TeamMembers;
    }

    //������Ƀ��[�v����������邩�ǂ���
    private GameObject IsMemberWarped() 
    {
        foreach(var obj in TeamMembers)
        {
            if(obj.GetComponent<SEnemyMove>().GetIsWarped())
            {
                return obj;
            }
        }
        return null; 
    }
}