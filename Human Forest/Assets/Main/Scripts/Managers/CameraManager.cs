using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private Camera[] baseCameras;

    public override void Init()
    {
        if (baseCameras.Length > 0)
        {

        }
        else
        {
            Debug.LogWarning("[CameraManager] No cameras registered to CameraManager");
        }
    }

    private void IO(int camIndex)
    {
        for (int i = 0; i < baseCameras.Length; i++)
        {
            if (i == camIndex) baseCameras[i].gameObject.SetActive(true);
            else baseCameras[i].gameObject.SetActive(false);
        }
    }
}