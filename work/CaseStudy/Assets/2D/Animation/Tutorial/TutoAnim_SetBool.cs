using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoAnim_SetBool : MonoBehaviour
{
    private Animator animator;

    private bool init = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            animator = GetComponent<Animator>();

            init = true;
        }
    }

    private void SetLeftStick()
    {
        animator.SetBool("LeftStick", true);
    }
}
