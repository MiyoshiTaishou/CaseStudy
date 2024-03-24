using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class K_EnemyStan : MonoBehaviour
{
    [Header("���}�X�ȏォ��̗����ŃX�^�������邩"), SerializeField]
    private int iStunDistance = 2;

    [Header("�^�C���}�b�v�̃I�u�W�F�N�g��"), SerializeField]
    private string sTilemapObjectName = "Tilemap";

    [Header("�X�^������(�b)"), SerializeField]
    private float fStunTime = 2.0f;

    private  Tilemap tTilemap; // �^�C���}�b�v

    private bool IsStunned = false; // �X�^����Ԃ������t���O

    private Vector3 GroundPos;//���O�t���[���ɐڐG���Ă����n�ʂ̍��W

    private float fElapsedTime = 0f;//�o�ߎ���

    Vector2 Vec2DefaultSpeed;

    [Header("���^�C���̖��O(�ڒn����擾�ɕK�v)"), SerializeField]
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
        tTilemap = tilemapObject.GetComponent<Tilemap>();

        //tilemapObjectName�Ɠ����̃I�u�W�F�N�g��������Ȃ�������G���[���O�o��
        if (tTilemap == null)
        {
            Debug.LogError("Tilemap not found on object: " + sTilemapObjectName);
        }

        //�ڒn���̍��W�������ʒu��
        GroundPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �G�̈ʒu���擾����
        Vector3Int playerCellPosition = tTilemap.WorldToCell(transform.position);
        // �G�̉��ɂ���^�C���̈ʒu���v�Z����
        Vector3Int floorCellPosition = playerCellPosition + Vector3Int.down;

        // �G�̉��ɂ���^�C�����擾
        TileBase tile = tTilemap.GetTile(floorCellPosition);

        //�ڒn���������f����
        for (int i = 0; i < sFloorTileNames.Length; i++)
        {
            // �^�C�������݂��A���^�C���ł����
            if (tile != null && tile.name == sFloorTileNames[i])
            {//�ڒn���Ă���

                //���O�t���[���ɐڐG���Ă������W����iStunDistance�ȏ�Ⴂ�l�ł����
                if (GroundPos.y - transform.position.y >= iStunDistance - 1)//-1���Ă���̂̓��C�͈̔͂��l����������
                {
                    IsStunned = true;
                    Debug.Log(this.gameObject.name + "�X�^��");
                    fElapsedTime = 0f;
                }
                //���O�t���[���ɐڐG���Ă����n�ʂ̍��W���X�V
                GroundPos = transform.position;
            }
        }
        
        //�X�^����
        if(IsStunned==true)
        {
            // �o�ߎ��Ԃ����Z����
            fElapsedTime += Time.deltaTime;

            //�X�^������
            if(fElapsedTime>fStunTime)
            {
                Debug.Log(this.gameObject.name + "�X�^������");
                IsStunned = false;
            }
        }
    }

    //�X�^����Ԃڂ������[
    public bool GetIsStunned()
    {
        return IsStunned;
    }
}
