using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//�X�e�[�W�̃^�C���}�b�v�̍ŏ��A�ő���W����ɔw�i��ύX�����

public class K_BackGround : MonoBehaviour
{
    [Header("�^�C���}�b�v"), SerializeField]
    private Tilemap tilemap; // �^�C���}�b�v

    [Header("�J�����̕`��͈�"), SerializeField]
    private float fViewSize = 7;

    //�L�����o�X
    GameObject CanvasObj;

    //�g���ϐ���
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        //�L�����o�X�擾
        CanvasObj = transform.GetChild(0).gameObject;

        // �J�����̔����̍����ƕ����v�Z
        camHalfHeight = fViewSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect;
        //Debug.Log(Camera.main.orthographicSize * Camera.main.aspect);
        if (tilemap)
        {
            // �^�C���}�b�v�͈̔͂��v�Z
            CalculateBounds();
        }

        //�T�C�Y�ݒ�
        RectTransform canvasRectTransform = CanvasObj.GetComponentInParent<Canvas>().GetComponent<RectTransform>(); //�L�����o�X���擾
        float CanvasWidth = canvasRectTransform.rect.width;  //�L�����o�X��
        float OnScreenWidth = Mathf.Abs((maxBounds.x + camHalfWidth) - (minBounds.x- camHalfWidth)); //�X�N���[����Őݒ肵������
        float ReductionRatio = OnScreenWidth / CanvasWidth;   //���̊��������߂�
        this.gameObject.transform.localScale=new Vector3(ReductionRatio, ReductionRatio, ReductionRatio);   //�T�C�Y�ύX

        //�}�b�v�̒��S���W�v�Z
        Vector3 pos;
        pos.x = (minBounds.x + maxBounds.x) / 2.0f; //�E�[�ƍ��[�̐^��
        float CanvasHalfHeighth = canvasRectTransform.rect.height * ReductionRatio / 2.0f;  //�L�����o�X����
        pos.y = CanvasHalfHeighth+ minBounds.y;
        pos.z = gameObject.transform.position.z;
        gameObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�^�C���}�b�v�̍ő���W�A�ŏ����W�����߂�
    void CalculateBounds()
    {
        // �����l����ɑ傫��/�����Ȓl�ɐݒ�
        Vector3Int minCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int maxCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        // �^�C���}�b�v�̂��ׂẴZ�����`�F�b�N
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (pos.x < minCell.x) minCell.x = pos.x;
                if (pos.y < minCell.y) minCell.y = pos.y;
                if (pos.z < minCell.z) minCell.z = pos.z;

                if (pos.x > maxCell.x) maxCell.x = pos.x;
                if (pos.y > maxCell.y) maxCell.y = pos.y;
                if (pos.z > maxCell.z) maxCell.z = pos.z;
            }
        }

        // �^�C���}�b�v�̍����[�ƉE��[�̃��[���h���W���v�Z
        Vector3 minWorld = tilemap.CellToWorld(minCell);
        Vector3 maxWorld = tilemap.CellToWorld(maxCell) + tilemap.cellSize;
        minBounds = new Vector2(minWorld.x, minWorld.y);
        maxBounds = new Vector2(maxWorld.x, maxWorld.y);
    }
}
