using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の隊列ごとに一つ
// 隊列の動きを管理する

public class N_EnemyManager : MonoBehaviour
{
    [Header("隊列に所属する敵プレハブ"), SerializeField]
    private List<GameObject> TeamMembers = new List<GameObject>();

    private int iMemberNum = 0;

    public bool IsReflectionX = false;

    // 隊列の一帯でもカメラに移っていたらtrue
    private bool isLook = false;

    private Transform thisTrans;

    public List<SEnemyMove> sEnemyMoves = new List<SEnemyMove>();

    [System.Serializable]
    struct ManagerMoveStatus
    {
        [Header("何秒"), SerializeField]
        public float MoveTime;

        [Header("移動速度"), SerializeField]
        public float MoveSpeed;

        [Header("追跡時の移動速度"), SerializeField]
        public float ChaseSpeed;

        [Header("方向転換時待機時間"), SerializeField]
        public float WaitTime;

        [Header("発見後追跡までの時間"), SerializeField]
        public float FoundTime;

        [Header("見失って巡回に戻る時間"), SerializeField]
        public float LostSightTime;

    }
    // 敵の移動方式変える
    // 指定秒数正面に移動する
    [Header("移動ステータス"), SerializeField]
    ManagerMoveStatus managerStatus;

    //保存された隊列分裂前の待機時間
    [Header("おーるどたいむ"),SerializeField]
    private float OldWaitTime=0.2f;
    // 経過時間
    private float ElapsedTime = 0.0f;
    private float ElapsedWaitTime = 0.0f;
    private float ElapsedFoundTime = 0.0f;
    private float ElapsedLostSightTime = 0.0f;

    private bool IsDitection = false;

    private bool IsRef = false;

    private bool init = false;

    // マネージャーの生成順
    private static int GenerateOrder = 0;
    // 生成順
    private int GenerateNumber = 0;

    public int GetGenerateNumber()
    {
        return GenerateNumber;
    }

    //複製された隊列かどうか
    private bool isClone = false;

    // ステートマシン
    public enum ManagerState
    {
        IDLE,       // アイドル
        PATOROL,    // 巡回
        WAIT,       // 待機
        ECPULSION,  // 除名
        FOUND,      // 発見
        CHASEINIT,  // 追跡準備
        CHASE,      // 追跡
        LOSTSIGHT,  // 見失った
        Warped,     // ワープした
    }

    [SerializeField]
    private ManagerState managerState = ManagerState.PATOROL;

    // 追跡対象の座標
    private GameObject Target;
    private Transform TargetTrans;

    private N_PostProcess postProcess;

    // 移動ステータス初期化
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

        // 初期化
        //InitMoveStatus();

        // 隊列の人数を計測
        CountMemberNum();
        // 隊列内の敵の移動スクリプトを取得
        SetEnemyMoveScript();

        // 名前でマネージャーを区別できるようにする
        this.gameObject.name = this.gameObject.name + GenerateOrder.ToString();
        GenerateNumber = GenerateOrder;
        GenerateOrder++;

        if (!isClone)
        {
            OldWaitTime = managerStatus.WaitTime;
            Debug.Log("オールドタイム" + OldWaitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!init)
        {
            // 隊列の敵に番号を付与
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

    // 高さが一定以上離れたらチームを分ける
    public void PartitionTeamHeight()
    {
        // マネージャー生成
        GameObject manager = new GameObject();
        manager.transform.parent = thisTrans.parent.gameObject.transform;
        manager.name = "EnemyManager";
        N_EnemyManager sc_mana = manager.AddComponent<N_EnemyManager>();
        sc_mana.SetMoveStatus(managerStatus);
        sc_mana.ChangeManagerState(ManagerState.PATOROL);

        Vector3 pos = Vector3.zero;
        int order = 0;
        // 敵の高さを取得
        foreach(var obj in TeamMembers)
        {
            if (IsMemberWarped() == null || (IsMemberWarped()!=null &&  IsMemberWarped() == obj))
            {
                Debug.Log("こいつううう～" + obj.name+"なんでや！？"+obj.GetComponent<SEnemyMove>().GetIsWarped());
                // 最初の敵のY座標を元にする
                if (order == 0)
                {
                    pos = obj.transform.position;

                    EcpulsionMember(obj.GetComponent<SEnemyMove>().GetTeamNumber(), managerState);

                    //ワープ後の隊列配備に関する処理、先頭の敵を隊列から分裂させる
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

                // ある程度の範囲内の敵はチームに追加
                if (obj.transform.position.y <= pos.y + 0.1f && obj.transform.position.y >= pos.y - 0.1f)
                {
                    EcpulsionMember(obj.GetComponent<SEnemyMove>().GetTeamNumber(), managerState);
                    if (IsReflectionX == obj.GetComponent<SEnemyMove>().GetWarp().GetiswarpRight())
                    {
                        sc_mana.IsReflectionX = !IsReflectionX;
                        managerState = ManagerState.Warped;
                    }
                    // 新チームに移動
                    sc_mana.TeamAddEnemy(obj);
                    Debug.Log("高さの違う新チーム");
                }

                order++;
            }
        }
    }

    // ホログラムによってチームを分割された時
    void PartitionTeam(int _num)
    {
        // 分割の必要がない
        if(iMemberNum <= 1)
        {
            return;
        }

        // 新しいマネージャーオブジェクト生成
        GameObject manager = new GameObject();
        manager.transform.parent = thisTrans.parent.gameObject.transform;
        manager.name = "EnemyManager";
        N_EnemyManager sc_mana = manager.AddComponent<N_EnemyManager>();
        sc_mana.SetMoveStatus(managerStatus);

        int i = 0;
        // 分割
        List<GameObject> otherTeam = new List<GameObject>();
        foreach(var obj in TeamMembers)
        {
            // 右進行中
            if (!IsReflectionX)
            {
                // 検知した敵より後に登録された敵
                if (i > _num)
                {
                    otherTeam.Add(obj);
                }
            }
            // 左進行中
            else
            {
                // 検知した敵含め後に登録された敵
                if (i >= _num)
                {
                    otherTeam.Add(obj);
                }
            }
            i++;
        }

        // 隊列を外れた敵をリストから削除
        // 右進行中
        if (!IsReflectionX)
        {
            // 敵とそいつが持つスクリプトをリストから削除
            TeamMembers.RemoveRange(_num + 1, iMemberNum - _num - 1);
            sEnemyMoves.RemoveRange(_num + 1, iMemberNum - _num - 1);

        }
        // 左進行中
        else
        {
            TeamMembers.RemoveRange(_num, iMemberNum - _num);
            sEnemyMoves.RemoveRange(_num, iMemberNum - _num);
        }

        // 隊列の人数を計測
        CountMemberNum();
        // 自身の隊列に番号再付与
        SetInfomation(false);

        // 新しいマネージャーの隊列に敵を参加させる
        sc_mana.TeamAddEnemys(manager,otherTeam);

        // 壁発見者がいる方の隊列を待ち状態にする
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
        // 自分の隊列に加える
        TeamAddEnemys(this.gameObject, _manager.GetTeamMember());

        // X座標の小さい順に並び替え
        SortMember();

        sEnemyMoves.Clear();
        SetEnemyMoveScript();

        // 自身の隊列に番号再付与
        SetInfomation(true);

        // 待ち時間にする
        bool  flg = false;
        foreach (var obj in TeamMembers) 
        {
            if(obj.GetComponent<SEnemyMove>().GetIsWarped()==true)
            {
                flg = true;
                //obj.GetComponent<SEnemyMove>().SetisWarped(false);
                Debug.Log("あああ遠田");
            }
        }
        if(!flg) 
        {
            Debug.Log("やべえっす！");
            managerState = ManagerState.WAIT;
        }
        else
        {
            managerStatus.WaitTime = OldWaitTime;
        }
        IsRef = true;

        // 敵のいなくなったマネージャー削除
        Destroy(_manager.gameObject);
    }

    // X座標小さい順に並び替え
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
                // 左にいたら挿入
                if(TeamMembers[i].transform.position.x < list[j].transform.position.x)
                {
                    list.Insert(j, TeamMembers[i]);
                    //Debug.Log(TeamMembers[i].gameObject.name);
                    break;
                }
                else
                {
                    // 新リストに最後まで追加されなければ
                    if(j == i - 1)
                    {
                        // 末尾に追加
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
            // 全リアクション非表示
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
        // このフレームで移動する距離
        float distance = dir * managerStatus.MoveSpeed * Time.deltaTime;

        ElapsedTime += Time.deltaTime;

        // 一定時間経過
        if (ElapsedTime >= managerStatus.MoveTime)
        {
            ElapsedTime = 0.0f;
            // 待ち状態に遷移
            managerState = ManagerState.WAIT;
        }

        // 隊列の敵を移動させる
        foreach (var enemy in sEnemyMoves)
        {
            enemy.EnemyMove(distance, IsReflectionX);
        }
    }

    // 待ち状態
    private void Wait()
    {
        if(ElapsedWaitTime == 0.0f)
        {
            // 全リアクション非表示
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

        // 待つ
        if (ElapsedWaitTime >= managerStatus.WaitTime)
        {
            ElapsedWaitTime = 0.0f;
            // 巡回状態
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

            // 向きセット
            foreach (var obj in TeamMembers)
            {
                if (TargetTrans.position.x < obj.transform.position.x)
                {
                    // 向き変更
                    sEnemyMoves[num].EnemyMove(0.0f, true);
                }
                if (TargetTrans.position.x > obj.transform.position.x)
                {
                    // 向き変更
                    sEnemyMoves[num].EnemyMove(0.0f, false);
                }
                num++;
            }

            // ビックリマーク表示
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsSearchTarget(true);
                obj.transform.GetChild(2).GetComponent<Animator>().SetTrigger("sight");
            }
        }

        // 敵を発見後一定時間指さし確認
        ElapsedFoundTime += Time.deltaTime;

        // 追跡状態に遷移
        if(ElapsedFoundTime >= managerStatus.FoundTime)
        {
            // 初期化系
            ElapsedFoundTime = 0.0f;

            managerState = ManagerState.CHASEINIT;

            // ビックリマーク非表示
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

                // このフレームで移動する距離
                float distance = dir * managerStatus.ChaseSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            if(TargetTrans.position.x > obj.transform.position.x)
            {
                float dir = 1.0f;

                // このフレームで移動する距離
                float distance = dir * managerStatus.ChaseSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            num++;
        }

        // 追跡対象を見失ったら状態遷移
        // 右端と左端の敵が見失った = 隊列が見失った
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
            // クエスチョンマーク表示
            foreach (var obj in TeamMembers)
            {
                obj.GetComponent<K_EnemyReaction>().SetIsLostTarget(true);
                obj.transform.GetChild(2).GetComponent<Animator>().SetBool("sight", true);

            }
        }

        ElapsedLostSightTime += Time.deltaTime;

        if(ElapsedLostSightTime >= managerStatus.LostSightTime)
        {
            // 初期化
            Target = null;
            managerState = ManagerState.PATOROL;
            ElapsedLostSightTime = 0.0f;

            postProcess.SubSerachNum();

            // クエスチョンマーク非表示
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

        // 待つ
        if (ElapsedWaitTime >= 0.2f)
        {
            ElapsedWaitTime = 0.0f;
            // 巡回状態
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

    // 除名
    private void Ecpulsion()
    {
        // 何もせず待機
    }

    // ボールになった敵をリストから削除
    public void EcpulsionMember(int _number,ManagerState _state) 
    {
        // マネージャー管理を外す
        sEnemyMoves[_number].NullEnemyManager();

        TeamMembers.RemoveAt(_number);
        sEnemyMoves.RemoveAt(_number);
        // 隊列の人数を計測
        CountMemberNum();
        // 自身の隊列に番号再付与
        SetInfomation(false);

        managerState = _state;
    }

    // 隊列に敵複数を追加する
    private void TeamAddEnemys(GameObject _parent,List<GameObject> _others)
    {
        // リストに追加
        TeamMembers.AddRange(_others);
        foreach(var obj in TeamMembers)
        {
            // 親オブジェクト変更
            obj.transform.parent = _parent.transform;
        }
        CountMemberNum();
    }

    public void TeamAddEnemy(GameObject _obj)
    {
        // リストに追加
        TeamMembers.Add(_obj);
        foreach(var obj in TeamMembers)
        {
            // 親オブジェクト変更
            obj.transform.parent = gameObject.transform;
        }
        CountMemberNum();

        // スクリプトをいったんリセット
        sEnemyMoves.Clear();

        // 隊列内の敵の移動スクリプトを取得
        SetEnemyMoveScript();

        SetInfomation(false);
    }

    // 敵情報
    private void SetInfomation(bool _isNewMana)
    {
        for(int i = 0; i < iMemberNum; i++)
        {
            SEnemyMove move = sEnemyMoves[i];
            
            // 隊列内の番号付与
            move.SetNumber(i);
            if (_isNewMana)
            {
                // 新しいマネージャーを敵に登録
                move.SetEnemyManager();
                move.EnemyMove(0.0f, IsReflectionX);
            }
            // 隊列内の敵の向きを取得
            IsReflectionX = move.GetIsReflection();
        }
    }

    // ついでにやり取りするスクリプトもリストに追加
    private void SetEnemyMoveScript()
    {
        foreach(var member in TeamMembers)
        {
            SEnemyMove move = member.GetComponent<SEnemyMove>();
            // リストに要素が無ければ追加
            if (!sEnemyMoves.Contains(move))
            {
                // スクリプトを追加
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

    // 隊列の敵が一体でもカメラ内に移ったら隊列内の敵は移動開始
    public void IsLook(bool _isLook)
    {
        if (_isLook)
        {
            foreach (var sc in sEnemyMoves)
            {
                // 移動等の処理開始
                sc.StartMove();
            }
        }
    }

    // 隊列の誰かが切り返し依頼をする時呼び出し
    public void RequestRefletion()
    {
        //if (managerState == ManagerState.PATOROL)
        //{
        //    managerState = ManagerState.WAIT;
        //}
        managerState = ManagerState.WAIT;

    }

    // ホログラムの壁を検知した時呼び出し
    public void DetectionHologram(int _number)
    {
        if (IsDitection || managerState == ManagerState.WAIT)
        {
            //Debug.Log("でてけ");
            return;
        }
        IsDitection = true;

        // 右に進行中
        if (!IsReflectionX)
        {
            // 一番右の敵がホログラムの壁を検知
            if (_number == iMemberNum - 1)
            {
                managerState = ManagerState.WAIT;

                // クエスチョンマーク表示
                foreach (var obj in TeamMembers)
                {
                    obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(true);
                }

            }
            else/* if(_number != 0)*/
            {
                // 隊列分割
                PartitionTeam(_number);
            }
        }
        // 左に進行中
        else
        {
            // 一番左の敵がホログラムの壁を検知
            if (_number == 0)
            {
                managerState = ManagerState.WAIT;
                // クエスチョンマーク表示
                foreach (var obj in TeamMembers)
                {
                    obj.GetComponent<K_EnemyReaction>().SetIsSearchHologram(true);
                }
            }
            else/* if(_number != 0)*/
            {
                // 隊列分割
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

    //隊列内にワープしたやつがいるかどうか
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