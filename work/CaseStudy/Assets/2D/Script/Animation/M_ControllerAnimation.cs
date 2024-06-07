using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ControllerAnimation : MonoBehaviour
{
    /// <summary>
    /// コントローラーのアニメーション
    /// </summary>
    Animator animator;

    private bool isOnce = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {      
        if(animator.GetBool("endWork") && !isOnce)
        {
            Debug.Log("アニメーション");
            M_GameMaster.SetGamePlay(true);
            isOnce = true;
        }
        else if(!isOnce)
        {
            M_GameMaster.SetGamePlay(false);
        }

        if(isOnce)
        {
            float hor = Input.GetAxis("Horizontal");

            if(hor >= 0.3f)
            {
                animator.SetBool("wark", true);
            }
        }        
    }

    public void SetPushBool(bool _push)
    {
        animator.SetBool("push", _push);
    }
}
