using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_SpriteSetLayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("ÉåÉCÉÑÅ["), SerializeField]
    private int layer = 0;

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
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = layer;

            init = true;
        }
    }
}
