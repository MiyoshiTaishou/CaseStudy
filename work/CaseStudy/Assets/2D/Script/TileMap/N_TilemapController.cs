using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class N_TilemapController : MonoBehaviour
{
    private Tilemap tilemap;

    private BoundsInt bounds;

    public GameObject prefab;

    bool b = false;

    // Start is called before the first frame update
    void Start()
    {
        // �^�C���}�b�v�̉摜������͈͂̒[�̍��W���擾
        bounds = this.GetComponent<Tilemap>().cellBounds; 
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var pos in bounds.allPositionsWithin)
        //{
        //    // ���o�����ʒu��񂩂�^�C���}�b�v�p�̈ʒu���(�Z�����W)���擾
        //    Vector3Int cellPosition = new Vector3Int(pos.x, pos.y, pos.z);
        //    Debug.Log(cellPosition);

        //    if (b == false)
        //    {
        //        Vector3 Pos = new Vector3(cellPosition.x, cellPosition.y, cellPosition.z);

        //        Instantiate(prefab, Pos, Quaternion.Euler(0f, 0f, 0f));

        //        b = true;
        //    }


        //    break;
        //}

        //if (b == false)
        //{
        //    // ���o�����ʒu��񂩂�^�C���}�b�v�p�̈ʒu���(�Z�����W)���擾
        //    Vector3Int cellPosition = new Vector3Int(bounds.max.x, bounds.max.y, bounds.max.z);
        //    Debug.Log(cellPosition);

        //    Vector3 Pos = new Vector3(cellPosition.x - 0.5f, cellPosition.y - 0.5f, cellPosition.z);

        //    Instantiate(prefab, Pos, Quaternion.Euler(0f, 0f, 0f));

        //    // ���o�����ʒu��񂩂�^�C���}�b�v�p�̈ʒu���(�Z�����W)���擾
        //    cellPosition = new Vector3Int(bounds.min.x, bounds.min.y, bounds.min.z);
        //    Debug.Log(cellPosition);

        //    Pos = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, cellPosition.z);

        //    Instantiate(prefab, Pos, Quaternion.Euler(0f, 0f, 0f));

        //    b = true;
        //}


        //Debug.Log("���W" + bounds.position.x);
        //Debug.Log("���W" + bounds.position.y);
        //Debug.Log("max" + bounds.max.x);
        //Debug.Log("max" + bounds.max.y);
        //Debug.Log("min" + bounds.min.x);
        //Debug.Log("min" + bounds.min.y);
    }
}
