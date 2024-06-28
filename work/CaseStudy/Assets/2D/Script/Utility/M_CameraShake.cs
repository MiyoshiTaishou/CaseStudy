using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �J������h�炷����
/// </summary>
public class M_CameraShake : MonoBehaviour
{
    [Header("�h�炷����"), SerializeField]
    private float m_Power = 1.0f;

    [Header("�h�炷����"), SerializeField]
    private float m_TimeLimit = 1.0f;

   
    private Tilemap tilemap; // �^�C���}�b�v

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        tilemap = GetComponent<N_TrackingPlayer>().GetTilemap();
        if(tilemap)
        {
            CalculateBounds();
        }
    }

    /// <summary>
    /// ���̃X�N���v�g�ŌĂԂƗh���
    /// </summary>
    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    /// <summary>
    /// �h��鏈��
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShake()
    {
        //�h���O�̃J�����̍��W
        Vector3 initPos = transform.position;

        //�o�ߎ��Ԍv��
        float countTime = 0.0f;

        //�h��鎞�Ԃ̊ԏ�������
        while (countTime < m_TimeLimit)
        {
            //�J�����̈ʒu�������_���Ō��߂�
            float camX = initPos.x + Random.Range(-m_Power, m_Power);
            float camY = initPos.y + Random.Range(-m_Power, m_Power);

            if (tilemap)
            {
                // �J�����̔����̍����ƕ����v�Z
                camHalfHeight = Camera.main.orthographicSize;
                camHalfWidth = camHalfHeight * Camera.main.aspect;
                if (camY <= minBounds.y + camHalfHeight)
                {
                    camY = minBounds.y + camHalfHeight;
                }
                Vector3 newPosition = transform.position;
                camX = Mathf.Clamp(newPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            }


            transform.position = new Vector3(camX, camY, initPos.z);
            countTime += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;

    }

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

        // �I�t�Z�b�g��ǉ����ăJ�������X�e�[�W�O���f���Ȃ��悤�ɂ���
        minBounds = new Vector2(minWorld.x, minWorld.y);
        maxBounds = new Vector2(maxWorld.x, maxWorld.y);
    }
}
