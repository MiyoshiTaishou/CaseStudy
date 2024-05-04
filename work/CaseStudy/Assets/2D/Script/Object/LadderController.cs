using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��q�̏���
public class LadderController : MonoBehaviour
{
    [Header("�o�鑬�x"), SerializeField]
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
          
            Debug.Log("�o���Ă�");                               
            rb.gravityScale = 0; // �d�͂𖳌��ɂ���
            rb.velocity = new Vector2(rb.velocity.x, fInputY * fClimeSpeed);                       
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            Rigidbody2D rb = _collision.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f; // �d�͂�L���ɂ���
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
}
