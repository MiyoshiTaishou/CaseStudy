using UnityEngine;
using UnityEngine.UI;

public class K_UIManager : MonoBehaviour
{
    [Header("UI Text指定用"), SerializeField]
    private Text EnemyCountTextFrame;

    // 表示する変数
    private int nEnemyNum;

    // Use this for initialization
    void Start()
    {
        nEnemyNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCountTextFrame.text = string.Format("巻き込んだ敵：{0:0} ", nEnemyNum);
    }

    public void SetEnemyNum(int _nEnemyNum)
    {
        nEnemyNum = _nEnemyNum;
    }
}