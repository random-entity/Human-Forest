using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private Camera[] cameras;

    public override void Init()
    {
        if (cameras.Length > 0)
        {

        }
        else
        {
            Debug.Log("No cameras registered to CameraManager");
        }
    }

    private void IO(int camIndex)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == camIndex) cameras[i].gameObject.SetActive(true);
            else cameras[i].gameObject.SetActive(false);
        }
    }
}