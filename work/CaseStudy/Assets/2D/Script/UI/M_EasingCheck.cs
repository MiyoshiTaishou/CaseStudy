using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_EasingCheck : MonoBehaviour
{
    [Header("イージング関数があるオブジェクト"), SerializeField]
    private GameObject EasingObject;

    private bool isOnce = false;
    
    // Update is called once per frame
    void Update()
    {
        if(!isOnce && !EasingObject.GetComponent<M_ImageEasing>().GetIsEasing())
        {
            isOnce = true;
            GetComponent<M_ObjectEasing>().EasingOnOff();
        }
    }
}
