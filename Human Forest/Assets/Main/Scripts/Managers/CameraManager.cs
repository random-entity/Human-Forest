using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam1, cam2, cam3;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) cam1.gameObject.SetActive(!cam1.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.W)) cam2.gameObject.SetActive(!cam2.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.E)) cam3.gameObject.SetActive(!cam3.gameObject.activeInHierarchy);
    }
}
