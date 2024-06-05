using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ProjecterSympathy : MonoBehaviour
{
    [Header("�L����X�s�[�h"), SerializeField]
    private float fSpreadSpeed = 1.0f;

    [Header("�ő唼�a"), SerializeField]
    private float fMaxRadius = 4.0f;

    [Header("���̑���"), SerializeField]
    private float fLineWidth = 0.1f;

    [Header("�~����̓_�̐�"), SerializeField]
    private int iSegments = 20;

    [Header("�}�e���A��"), SerializeField]
    private Material material;

    [Header("�����G�t�F�N�g"), SerializeField]
    private GameObject MeowingPrefab;

    private GameObject MeowingObj; 
    Animator animator;

    // ���a
    private float fRadius = 0.0f;

    // ������
    private bool isSympathy = false;

    private LineRenderer lineRenderer;

    private Transform trans_Player;

    private Vector3 SympathyPosition = Vector3.zero;

    // �X�^�[�g�֐����s��ɍŏ��Ɏ��s�����
    private bool isInitialized = false;

    // ���e�@�̃X�N���v�g���i�[���郊�X�g
    private List<N_ProjectHologram> list = new List<N_ProjectHologram>();

    private N_PlaySound playSound;

    private bool isPossible = true;

    public void SetIsPossible(bool _possible)
    {
        isPossible = _possible;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(material!=null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = material;
        }
        trans_Player = this.gameObject.transform;

        playSound = transform.GetChild(1).gameObject.GetComponent<N_PlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        // false�Ŏ��t���[����
        if (!isPossible)
        {
            return;
        }

        // �ŏ��������s
        if (!isInitialized)
        {
            // �X�e�[�W�ɔz�u���ꂽ���e�@�̐e�I�u�W�F�N�g�擾
            Transform ParentTrans = GameObject.Find("Projecters").gameObject.GetComponent<Transform>();

            int i = 0;
            // �q�I�u�W�F�N�g�i�e�v���W�F�N�^�[�j�̃X�N���v�g���i�[
            foreach (Transform child in ParentTrans)
            {
                // ���X�g�ɓ����
                list.Add(child.GetComponent<N_ProjectHologram>());
                i++;
            }

            isInitialized = true;
        }

        // �L�[���͂��������狤�J�n
        if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("SympathyButton"))
        {
            if(isSympathy == false && !playSound.GetIsPlaying())
            {
                isSympathy = true;
                // �\��
                if (material != null)
                {
                    lineRenderer.enabled = true;
                }
                SympathyPosition = trans_Player.position;

                // ���X�g�Ɋi�[���ꂽ�e�I�u�W�F�N�g
                foreach (N_ProjectHologram SC_Holo in list)
                {
                    // ������
                    SC_Holo.Initialize();
                }

                // se�Đ�
                playSound.PlaySound(N_PlaySound.SEName.CrowCry);
                //Debug.Log("�Đ�");
                MeowingObj= Instantiate(MeowingPrefab, SympathyPosition, Quaternion.identity);
                MeowingObj.transform.localScale = new Vector3(fMaxRadius * 2.2f, fMaxRadius * 2.2f, 1);
                animator = MeowingObj.GetComponent<Animator>();
                animator.SetFloat("Speed", fSpreadSpeed/10f);
            }
        }

        // ����
        if (isSympathy)
        {
            // ���Ԍo�߂ōL����~��`��

            // ���a���L����
            fRadius += fSpreadSpeed * Time.deltaTime;
            
            // �Ō�̃��[�v
            if(fRadius >= fMaxRadius)
            {
                fRadius = fMaxRadius;

                isSympathy = false;
            }

            // �~��`�悷�鏈��
            if (material != null)
            {
                lineRenderer.widthMultiplier = fLineWidth;
                lineRenderer.positionCount = iSegments + 1;

                float deltaTheta = (2f * Mathf.PI) / iSegments;
                float theta = 0f;

                for (int i = 0; i < iSegments + 1; i++)
                {
                    float x = fRadius * Mathf.Cos(theta);
                    float y = fRadius * Mathf.Sin(theta);
                    Vector3 pos = new Vector3(SympathyPosition.x + x, SympathyPosition.y + y, 0f);
                    lineRenderer.SetPosition(i, pos);
                    theta += deltaTheta;
                }
            }

            // ���X�g�Ɋi�[���ꂽ�I�u�W�F�N�g�𑍓�����œ����蔻��
            foreach(N_ProjectHologram SC_Holo in list)
            {
                SC_Holo.CheckAreaSympathy(SympathyPosition,fRadius);
            }

            // �Ō�̃��[�v�ɓ���
            if (!isSympathy)
            {
                fRadius = 0.0f;
                if (material != null)
                {
                    lineRenderer.enabled = false;
                }
            }
            
        }
    }
}
