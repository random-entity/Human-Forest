using UnityEngine;

public class EnvironmentManager : MonoSingleton<EnvironmentManager>
{
    public GameObject Land;
    public float LandSize;

    public override void Init()
    {
        // Set land size.
        Land.transform.localScale = new Vector3(LandSize * 0.1f, 1f, LandSize * 0.1f);
    }
}