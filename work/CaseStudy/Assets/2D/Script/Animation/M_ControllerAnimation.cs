using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ControllerAnimation : MonoBehaviour
{
    /// <summary>
    /// コントローラーのアニメーション
    /// </summary>
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetBool("wark"))
        {
            M_GameMaster.SetGamePlay(true);
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
