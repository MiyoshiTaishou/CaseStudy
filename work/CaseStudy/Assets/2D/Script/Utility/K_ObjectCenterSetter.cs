using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_ObjectCenterSetter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.gameObject.transform.position.z);
    }
}
