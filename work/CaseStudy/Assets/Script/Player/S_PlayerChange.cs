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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //変身ボタンが押されたかどうか判定
        IsBDown = Input.GetKeyDown(KeyCode.C);
        if (IsBDown) 
        {
            if (this.tag == "Player")
            {
                this.tag = "Enemy";
            }
            else if(this.tag=="Enemy")
            {
                this.tag = "Player";
            }
        }
    }
}
