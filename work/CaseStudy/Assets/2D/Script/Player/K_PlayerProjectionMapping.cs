using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//�z���O�������s�����߂̃R�[�h�ł��B
//�}�E�X��D&D���������ɉ����ē����蔻��̂Ȃ��ǂ܂��͏�����������܂��B�܂��A�E�N���b�N����ƃv���C���[�̃z���O��������������܂��B

public class K_PlayerProjectionMapping : MonoBehaviour
{
    [Header("�v���W�F�N�V�����}�b�s���O�p�^�C���}�b�v"), SerializeField]
    private Tilemap ProjectionMappingTileMap;

    [Header("�ǃ^�C��"), SerializeField]
    private TileBase wall;

    [Header("���^�C��"), SerializeField]
    private TileBase floor;

    [Header("�v���W�F�N�V�����}�b�s���O�����L�["), SerializeField]
    private KeyCode ResetKey;

    [Header("�z���O�����̎����i�b�j"), SerializeField]
    private float fTileLifetime = 5f;

    [Header("�v���C���[�z���O������Prefab"), SerializeField]
    private GameObject SpritePrefab;


    private Vector3Int startTilemapPos; // �}�E�X��������n�߂��ʒu

    private Dictionary<Vector3Int, float> activeTiles = new Dictionary<Vector3Int, float>(); // �`�撆�̃^�C���Ƃ��̎���

    void Start()
    {
        ProjectionMappingTileMap.ClearAllTiles();
    }

    void Update()
    {
        // �^�C���̎���������������
        foreach (var key in new List<Vector3Int>(activeTiles.Keys))
        {
            activeTiles[key] -= Time.deltaTime;
            if (activeTiles[key] <= 0)
            {
                ProjectionMappingTileMap.SetTile(key, null); // �^�C��������
                activeTiles.Remove(key); // activeTiles����폜
            }
        }

        if (Input.GetMouseButtonDown(0)) // �}�E�X�̍��{�^���������ꂽ�u�Ԃ����o
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // �}�E�X�̈ʒu�����[���h���W�ɕϊ�
            startTilemapPos = ProjectionMappingTileMap.WorldToCell(mouseWorldPos); // ���[���h���W����^�C���}�b�v�̍��W�ɕϊ�
        }
        else if (Input.GetMouseButtonUp(0)) // �}�E�X�̍��{�^���������ꂽ�u�Ԃ����o
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // �}�E�X�̈ʒu�����[���h���W�ɕϊ�
            Vector3Int endTilemapPos = ProjectionMappingTileMap.WorldToCell(mouseWorldPos); // ���[���h���W����^�C���}�b�v�̍��W�ɕϊ�

            // ������n�߂��ʒu���痣���ꂽ�ʒu�܂ł̃^�C����`��
            DrawTiles(ProjectionMappingTileMap,startTilemapPos, endTilemapPos); 
        }

        if (Input.GetKeyDown(ResetKey)) // �����L�[�����͂��ꂽ��
        {//�S������
            ProjectionMappingTileMap.ClearAllTiles();
        }

        // �}�E�X�̍��N���b�N�����ꂽ�ꍇ
        if (Input.GetMouseButtonDown(1))
        {
            // �N���b�N�����ʒu���擾
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // Z���͎g��Ȃ��̂ŁA0�ɌŒ�

            // �X�v���C�g��\������
            GameObject newSprite = Instantiate(SpritePrefab, clickPosition, Quaternion.identity);

            // �X�v���C�g����莞�Ԍ�ɔj������
            StartCoroutine(DestroySpriteAfterDelay(newSprite, fTileLifetime));
        }
    }


    //���ۂɕ�/���𐶐�����֐�
    void DrawTiles(Tilemap tilemap,Vector3Int start, Vector3Int end)
    {
        //�K�v�ȐF�X�Ȑ��l�����߂��
        int deltaX = Mathf.Abs(end.x - start.x);
        int deltaY = Mathf.Abs(end.y - start.y);
        int signX = start.x < end.x ? 1 : -1;
        int signY = start.y < end.y ? 1 : -1;
        int error = deltaX - deltaY;

        int x = start.x;
        int y = start.y;

        while (true)
        {
            //�J�n�ʒu�ƏI���ʒu���r�A��X�ƃ�Y�ǂ��炪�傫������r
            if (deltaX < deltaY)
            {//Y�����̂ق����傫��������A�ǂ𐶐�
                //�^�C���`��
                tilemap.SetTile(new Vector3Int(x, y, 0), wall);

                //�����ݒ�
                activeTiles[new Vector3Int(x, y, 0)] = fTileLifetime;

                //�Ō�܂ŏ�������E�o
                if (y == end.y)
                    break;

                //���ɕ`�悷��^�C���̍��W�����߂�
                int error2 = error * 2;
                if (error2 < deltaX)
                {
                    error += deltaX;
                    y += signY;
                }
            }
            else
            {//X�����̂ق����傫��������A���𐶐�
                //�^�C���`��
                tilemap.SetTile(new Vector3Int(x, y, 0), floor);

                //�����ݒ�
                activeTiles[new Vector3Int(x, y, 0)] = fTileLifetime;

                //�Ō�܂ŏ�������E�o
                if (x == end.x)
                    break;

                //���ɕ`�悷��^�C���̍��W�����߂�
                int error2 = error * 2;
                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    x += signX;
                }
            }
        }
    }

    IEnumerator DestroySpriteAfterDelay(GameObject spriteObject, float delay)
    {
        // �w�肵�����Ԃ�҂�
        yield return new WaitForSeconds(delay);

        // �X�v���C�g��j������
        Destroy(spriteObject);
    }
}


