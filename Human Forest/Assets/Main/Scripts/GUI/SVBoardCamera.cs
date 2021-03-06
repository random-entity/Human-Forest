using UnityEngine;

public class SVBoardCamera : MonoBehaviour
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
            var ray = gUICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform.gameObject.GetComponent<IClickableGUI>();
                if (selection != null)
                {
                    selection.OnClicked();
                }
            }

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