using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//梯子の処理
public class LadderController : MonoBehaviour
{
    [Header("登る速度"), SerializeField]
    private float fClimeSpeed = 3.0f;

    private bool isClimbing = false;

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            Rigidbody2D rb = _collision.GetComponent<Rigidbody2D>();
            
            float fInputY = Input.GetAxis("Vertical");

            if(fInputY != 0.0f)
            {
                _collision.transform.position = new Vector3(transform.position.x, _collision.transform.position.y, _collision.transform.position.z);
            }
          
            Debug.Log("登ってる");                               
            rb.gravityScale = 0; // 重力を無効にする
            rb.velocity = new Vector2(rb.velocity.x, fInputY * fClimeSpeed);                       
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            Rigidbody2D rb = _collision.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f; // 重力を有効にする
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
}
