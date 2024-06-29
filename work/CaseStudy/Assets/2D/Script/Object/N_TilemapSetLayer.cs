using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class N_TilemapSetLayer : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;

    [Header("ÉåÉCÉÑÅ["), SerializeField]
    private int layer = -3;

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
            tilemapRenderer = GetComponent<TilemapRenderer>();

            tilemapRenderer.sortingOrder = layer;

            init = true;
        }
    }
}
