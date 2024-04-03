using UnityEngine;
using UnityEngine.UI;

public class K_UIManager : MonoBehaviour
{
    [Header("UI Text�w��p"), SerializeField]
    private Text EnemyCountTextFrame;

    // �\������ϐ�
    private int nEnemyNum;

    // Use this for initialization
    void Start()
    {
        nEnemyNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCountTextFrame.text = string.Format("�������񂾓G�F{0:0} ", nEnemyNum);
    }

    public void SetEnemyNum(int _nEnemyNum)
    {
        nEnemyNum = _nEnemyNum;
    }
}