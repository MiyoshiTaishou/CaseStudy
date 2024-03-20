using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    /// <summary>
    ///B�{�^���������ꂽ���ǂ���
    /// </summary>
    private
    bool IsBDown = false;

    [Header("�ϐg�O�̌�����"),SerializeField]
    Sprite defaultsprite;

    [Header("�ϐg��̌�����"),SerializeField]
    Sprite changedsprite;

    private
    SpriteRenderer spriteRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(!spriteRenderer)
        {
            Debug.LogError("SpriteRenderer������܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�ϐg�{�^���������ꂽ���ǂ�������
        IsBDown = Input.GetKeyDown(KeyCode.C);//�����ŃL�[�w�肵�Ă�
        if (IsBDown) 
        {
            //���݂̏󋵂ɉ����ă^�O�ƌ����ڂ�ύX
            if (this.tag == "Player")
            {
                this.tag = "Enemy";
                spriteRenderer.sprite = changedsprite;
            }
            else if(this.tag=="Enemy")
            {
                this.tag = "Player";
                spriteRenderer.sprite = defaultsprite;
            }
        }
    }
}
