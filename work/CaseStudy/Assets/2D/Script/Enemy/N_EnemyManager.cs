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

    private bool IsReflectionX = false;

    // 隊列の一帯でもカメラに移っていたらtrue
    private bool isLook = false;

    private Transform thisTrans;

    public List<SEnemyMove> sEnemyMoves = new List<SEnemyMove>();

    // 敵の移動方式変える
    // 指定秒数正面に移動する
    [Header("何秒"), SerializeField]
    private float MoveTime = 3.0f;

    [Header("移動速度"), SerializeField]
    private float MoveSpeed = 4.0f;

    [Header("追跡時の移動速度"), SerializeField]
    private float ChaseSpeed = 4.0f;

    [Header("方向転換時待機時間"), SerializeField]
    private float WaitTime = 0.5f;

    // 経過時間
    private float ElapsedTime = 0.0f;
    private float ElapsedWaitTime = 0.0f;

    private bool IsDitection = false;

    // マネージャーの生成順
    private static int GenerateOrder = 0;

    // ステートマシン
    public enum ManagerState
    {
        IDLE,       // アイドル
        PATOROL,    // 巡回
        WAIT,       // 待機
        ECPULSION,  // 除名
        CHASE,      // 追跡
    }

    private ManagerState managerState = ManagerState.PATOROL;

    // 追跡対象の座標
    private GameObject Target;
    private Transform TargetTrans;

    // Start is called before the first frame update
    void Start()
    {
        thisTrans = this.transform;

        // 隊列の人数を計測
        CountMemberNum();
        // 隊列内の敵の移動スクリプトを取得
        SetEnemyMoveScript();
        // 隊列の敵に番号を付与
        SetInfomation(true);

        // 名前でマネージャーを区別できるようにする
        this.gameObject.name = this.gameObject.name + GenerateOrder.ToString();
        GenerateOrder++;
    }

    // Update is called once per frame
    void Update()
    {
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

            case ManagerState.CHASE:
                Chase();
                break;
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
        sc_mana.TeamAddEnemy(manager,otherTeam);

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

    public void ChangeManagerState(ManagerState _state)
    {
        managerState = _state;
    }

    private void Patrol()
    {
        float dir = 1.0f;
        if (IsReflectionX == true)
        {
            dir *= -1.0f;
        }
        // このフレームで移動する距離
        float distance = dir * MoveSpeed * Time.deltaTime;

        ElapsedTime += Time.deltaTime;

        // 一定時間経過
        if (ElapsedTime >= MoveTime)
        {
            // 初期化
            Init();
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
        ElapsedWaitTime += Time.deltaTime;

        // 待つ
        if (ElapsedWaitTime >= WaitTime)
        {
            ElapsedWaitTime = 0.0f;
            // 巡回状態
            managerState = ManagerState.PATOROL;
        }
        else
        {
            foreach (var enemy in sEnemyMoves)
            {
                enemy.EnemyMove(0.0f, IsReflectionX);
            }
        }
    }

    private void Chase()
    {
        int num = 0;
        foreach (var obj in TeamMembers) {
            if (TargetTrans.position.x < obj.transform.position.x)
            {
                float dir = -1.0f;

                // このフレームで移動する距離
                float distance = dir * MoveSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            if(TargetTrans.position.x > obj.transform.position.x)
            {
                float dir = 1.0f;

                // このフレームで移動する距離
                float distance = dir * MoveSpeed * Time.deltaTime;

                sEnemyMoves[num].ChaseTarget(distance);
            }
            num++;
        }
    }

    public void SetTarget(GameObject _obj)
    {
        if (Target == null || Target != _obj)
        {
            Target = _obj;
            TargetTrans = Target.GetComponent<Transform>();
            managerState = ManagerState.CHASE;
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
        TeamMembers.RemoveAt(_number);
        sEnemyMoves.RemoveAt(_number);
        // 隊列の人数を計測
        CountMemberNum();
        // 自身の隊列に番号再付与
        SetInfomation(false);

        managerState = _state;
    }

    // 隊列に敵を追加する
    private void TeamAddEnemy(GameObject _parent,List<GameObject> _others)
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
            sEnemyMoves.Add(member.GetComponent<SEnemyMove>());
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

    private void Init()
    {
        ElapsedTime = 0.0f;
        IsReflectionX = !IsReflectionX;
    }

    // 隊列の誰かが切り返し依頼をする時呼び出し
    public void RequestRefletion()
    {
        Init();
        managerState = ManagerState.WAIT;
    }

    // ホログラムの壁を検知した時呼び出し
    public void DetectionHologram(int _number)
    {
        if (IsDitection)
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
                //Debug.Log(_number.ToString() + "検知 + 切り返し");
                Init();
                managerState = ManagerState.WAIT;
            }
            else/* if(_number != 0)*/
            {
                // 隊列分割
                PartitionTeam(_number);
                //Debug.Log(gameObject.name + _number.ToString() + "検知 + 分割右" + IsReflectionX);

            }
        }
        // 左に進行中
        else
        {
            // 一番左の敵がホログラムの壁を検知
            if (_number == 0)
            {
                //Debug.Log(_number.ToString() + "検知 + 切り返し");
                Init();
                managerState = ManagerState.WAIT;
            }
            else/* if(_number != 0)*/
            {
                // 隊列分割
                PartitionTeam(_number);
                //Debug.Log(gameObject.name + _number.ToString() + "検知 + 分割左" + IsReflectionX);

            }
        }
        IsDitection = false;
    }

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }

    public N_EnemyManager.ManagerState GetState()
    {
        return managerState;
    }

    public bool GetIsReflection()
    {
        return IsReflectionX;
    }
}