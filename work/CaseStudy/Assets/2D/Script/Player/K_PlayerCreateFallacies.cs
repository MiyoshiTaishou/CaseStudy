using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class K_PlayerCreateFallacies : MonoBehaviour
{
    //���C�̒���(���u��)
    private const float fRayLength = 0.5f;
    //�v���C���[���a(���u��)
    private const float fPlayerRad = 1.0f;
    //�v���C���[�̌���(-1�͍��A1�͉E�������Ă���)
    private int iPlayreDirection;

    // �^�C���}�b�v�I�u�W�F�N�g
    private Tilemap Tilemap;

    [Header("�^�C���}�b�v�̃I�u�W�F�N�g��"), SerializeField]
    public string sTilemapObjectName = "Tilemap";

    [Header("���^�C���̖��O(�^�C�������w�肵�Ȃ��Ə����Ȃ�)"), SerializeField]
    public string[] sFloorTileNames =
    {
        "douro_0",
        "douro_1",
        "douro_2",
        "douro_3",
        "douro_4",
    };

    void Start()
    {
        // �V�[�����̃^�C���}�b�v�I�u�W�F�N�g���擾
        GameObject tilemapObject = GameObject.Find(sTilemapObjectName);

        // �^�C���}�b�v�I�u�W�F�N�g����Tilemap�R���|�[�l���g���擾
        Tilemap = tilemapObject.GetComponent<Tilemap>();

        //tilemapObjectName�Ɠ����̃I�u�W�F�N�g��������Ȃ�������G���[���O�o��
        if (Tilemap == null)
        {
            Debug.LogError("Tilemap not found on object: " + sTilemapObjectName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C���}�b�v��������Ȃ��ꍇ�͏����𒆎~
        if (Tilemap == null)
        {
            return;
        }

        //�v���C���[�̌��������߂�
        float fHorizontalInput = Input.GetAxis("Horizontal");
        if (fHorizontalInput < 0.0f)
        {
            iPlayreDirection = -1;
        }
        if (0.0f < fHorizontalInput)
        {
            iPlayreDirection = 1;
        }

        //F�������ꂽ��
        if (Input.GetKeyDown(KeyCode.F))
        {
            // �v���C���[�̈ʒu���擾����
            Vector3Int playerCellPosition = Tilemap.WorldToCell(transform.position);
            // �v���C���[�̉��ɂ���^�C���̈ʒu���v�Z����
            Vector3Int floorCellPosition = playerCellPosition + Vector3Int.down;

            // �v���C���[�̉��ɂ���^�C�����擾
            TileBase tile = Tilemap.GetTile(floorCellPosition);

            //�v���C���[�̉��ɂ���^�C�������^�C�������f����
            for (int i = 0; i < sFloorTileNames.Length; i++)
            {
                // �^�C�������݂��A���^�C���ł����
                if (tile != null && tile.name == sFloorTileNames[i])
                {
                    // �v���C���[�̉��ɂ��鏰�ׂ̗̏��̈ʒu���v�Z����
                    Vector3Int neighborFloorCellPosition = floorCellPosition + new Vector3Int(iPlayreDirection, 0, 0);

                    // �v���C���[�̉��ɂ��鏰�ׂ̗̏����擾
                    tile = Tilemap.GetTile(neighborFloorCellPosition);

                    //���ۂɏ�����������
                    for (int j = 0; j < sFloorTileNames.Length; j++)
                    {
                        // �^�C�������݂��A�w�肵�����O�̃^�C���ł��邩���m�F���ď����폜����
                        if (tile != null && tile.name == sFloorTileNames[i])
                        {
                            Tilemap.SetTile(neighborFloorCellPosition, null);
                        }
                    }
                }
            }
        }
    }
}