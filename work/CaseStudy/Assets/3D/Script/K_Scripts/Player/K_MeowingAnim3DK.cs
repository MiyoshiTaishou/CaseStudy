using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_MeowingAnim3DK : MonoBehaviour
{
    [Header("è¡ñ≈éûä‘(s)"), SerializeField]
    private float deleteTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
