using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_DuctAnimReset : MonoBehaviour
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

            init = false;
        }
    }

    private void ResetAnim()
    {
        animator.SetBool("duct", false);
    }
}
