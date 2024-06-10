using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GroundCheck : MonoBehaviour
{
    [Header("地面に当たってるか"), SerializeField]
    private bool isGround = false;

    public bool GroundCheck()
    {
        return isGround;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround)
        {
            //Debug.Log("当たり" + gameObject.transform.parent.gameObject.name);
        }
        else
        {
            //Debug.Log("はずれ" + gameObject.transform.parent.gameObject.name);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }
}
