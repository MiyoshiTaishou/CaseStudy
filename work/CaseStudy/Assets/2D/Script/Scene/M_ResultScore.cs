using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ResultScore : MonoBehaviour
{
    [Header("•\Ž¦‚µ‚½‚¢‰æ‘œ"), SerializeField]
    private GameObject[] image = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in image)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(M_GameMaster.GetDethCount() == 0)
        {
            image[0].SetActive(true);
        }
    }
}
