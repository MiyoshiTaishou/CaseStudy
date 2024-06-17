using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_HoloPlayerDestroy : MonoBehaviour
{
    [Header("è¡Ç¶ÇÈë¨ìx"), SerializeField]
    private float DisappearTime = 0.5f;

    private SpriteRenderer spriteRenderer;

    private N_ProjectHologram projectHologram;

    private bool AlphaDown = false;

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
            spriteRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();
            projectHologram = gameObject.transform.parent.GetComponent<N_ProjectHologram>();

            init = true;
        }

        if (AlphaDown)
        {
            Color color = spriteRenderer.color;


            if (color.a > 0.0f)
            {
                color.a -= Time.deltaTime * 1.0f / DisappearTime;

                if(color.a < 0.0f)
                {
                    color.a = 0.0f;

                    AlphaDown = false;
                    projectHologram.SetOnOff(false);
                }

                spriteRenderer.color = color;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AlphaDown = true;
        }
    }

    public void OnAlpha()
    {
        Color color = spriteRenderer.color;

        color = new Color(color.a, color.g, color.b, 1.0f);

        spriteRenderer.color = color;
    }
}
