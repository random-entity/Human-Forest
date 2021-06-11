using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalGUICamera : MonoBehaviour
{
    private Vector3 touchStart;
    private Camera gUICamera;
    [SerializeField] private float zoomMin = 1f;
    [SerializeField] private float zoomMax = 8f;
    [SerializeField] private float zoomSensitivity = 2f;

    private void Start()
    {
        gUICamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = gUICamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - gUICamera.ScreenToWorldPoint(Input.mousePosition);
            gUICamera.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity);
    }

    private void Zoom(float increment)
    {
        gUICamera.orthographicSize = Mathf.Clamp(gUICamera.orthographicSize - increment, zoomMin, zoomMax);
    }
}
