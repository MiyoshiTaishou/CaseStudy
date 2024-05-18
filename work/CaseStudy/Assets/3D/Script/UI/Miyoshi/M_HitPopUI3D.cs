using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HitPopUI3D : MonoBehaviour
{
    [Header("�\���������I�u�W�F�N�g������"),SerializeField]
    private GameObject hitPopUI;

    private void Start()
    {
        hitPopUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");

        if (other.CompareTag("Player"))
        {
            Debug.Log("HITPlayer");
            hitPopUI.GetComponent<M_ObjectEasing>().SetReverse(false);
            hitPopUI.SetActive(true);
            hitPopUI.GetComponent<M_ObjectEasing>().EasingOnOff();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hitPopUI.GetComponent<M_ObjectEasing>().SetReverse(true);
            hitPopUI.GetComponent<M_ObjectEasing>().EasingOnOff();
        }
    }  
}