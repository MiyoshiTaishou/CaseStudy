using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_SetColliderOffSet : MonoBehaviour
{

    [Header("�����蔻���S���I�u�W�F"), SerializeField]
    private GameObject ColObj;

    // �{�b�N�X�R���C�_�[
    private BoxCollider2D BoxCol;

    // �{�b�N�X�ł̓����蔻�肪�K�v��
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
