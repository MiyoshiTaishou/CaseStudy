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
        if(isOnce)
        {
            return;
        }

        if(animator.GetBool("wark"))
        {
            Debug.Log("アニメーション");
            M_GameMaster.SetGamePlay(true);
            isOnce = true;
        }
        else
        {
            M_GameMaster.SetGamePlay(false);
        }
    }

    public void SetPushBool(bool _push)
    {
        animator.SetBool("push", _push);
    }
}
