using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_SetColliderOffSet : MonoBehaviour
{

    [Header("当たり判定を担うオブジェ"), SerializeField]
    private GameObject ColObj;

    // ボックスコライダー
    private BoxCollider2D BoxCol;

    // ボックスでの当たり判定が必要か
    private bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        BoxCol = ColObj.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActive(bool _active)
    {
        if (isColliding)
        {
            ColObj.SetActive(_active);
        }
    }

    public void SetIsColliding(bool _isCol)
    {
        isColliding = _isCol;
    }

    public void SetOffSet(Vector3 _size,Vector2 _offset)
    {
        BoxCol.size = _size;
        BoxCol.offset = _offset;
    }
}
