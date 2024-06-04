using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerTrail : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g"),SerializeField]
    private GameObject player;

    [Header("�c���I�u�W�F�N�g"), SerializeField]
    private GameObject trailPrefab;

    [Header("�c���̊Ԋu"), SerializeField]
    private int trailInterval = 3;

    [Header("�c���̒���"), SerializeField]
    private int trailLength = 5; // �c���̒���

    [Header("�c���̃t�F�[�h�A�E�g����"), SerializeField]
    private float fadeTime = 0.5f; // �c���̃t�F�[�h�A�E�g����

    [Header("�c���̕\���ʒu"),SerializeField]
    private Vector3 trailOffset = Vector3.zero;

    private int intervalCounter;
    private GameObject[] trails;

    void Start()
    {
        trails = new GameObject[trailLength];
        intervalCounter = trailInterval;
    }

    void Update()
    {
        if(!M_GameMaster.GetGamePlay())
        {
            return;
        }

        // �v���C���[���_�b�V�����Ă��邩�ǂ������m�F
        if (player.GetComponent<M_PlayerMove>().GetIsDash())
        {
            intervalCounter--;
            // �c���̊Ԋu���ƂɎc���𐶐�
            if (intervalCounter <= 0)
            {
                intervalCounter = trailInterval;
                GenerateTrail();
            }
        }
    }

    void GenerateTrail()
    {
        // �v���C���[�̌��݂̉�]���擾
        Quaternion playerRotation = player.transform.rotation;

        // �V�����c���𐶐�
        GameObject newTrail = Instantiate(trailPrefab, player.transform.position + trailOffset, playerRotation);
        // �c�������X�g�ɒǉ�
        for (int i = 0; i < trails.Length - 1; i++)
        {
            trails[i] = trails[i + 1];
        }
        trails[trails.Length - 1] = newTrail;
        // �c���̃t�F�[�h�A�E�g���J�n
        StartCoroutine(FadeOutTrail(newTrail));
    }

    IEnumerator FadeOutTrail(GameObject trail)
    {
        SpriteRenderer trailRenderer = trail.GetComponent<SpriteRenderer>();
        Color color = trailRenderer.color;
        float startTime = Time.time;
        while (Time.time < startTime + fadeTime)
        {
            float t = (Time.time - startTime) / fadeTime;
            color.a = Mathf.Lerp(1f, 0f, t);
            trailRenderer.color = color;
            yield return null;
        }
        Destroy(trail);
    }
}
