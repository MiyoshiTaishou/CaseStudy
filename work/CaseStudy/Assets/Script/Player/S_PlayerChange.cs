using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerChange : MonoBehaviour
{
    /// <summary>
    ///Bボタンが押されたかどうか
    /// </summary>
    private
    bool IsBDown = false;

    [Header("変身前の見た目"),SerializeField]
    Sprite defaultsprite;

    [Header("変身後の見た目"),SerializeField]
    Sprite changedsprite;

    private
    SpriteRenderer spriteRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(!spriteRenderer)
        {
            Debug.LogError("SpriteRendererがありません");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //変身ボタンが押されたかどうか判定
        IsBDown = Input.GetKeyDown(KeyCode.C);//ここでキー指定してね
        if (IsBDown) 
        {
            //現在の状況に応じてタグと見た目を変更
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
