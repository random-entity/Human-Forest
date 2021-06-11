using UnityEngine;

public class FluidCamera : MonoBehaviour
{
    public RenderTexture output;
    [SerializeField] private GameObject displayQuad;

    private void Awake()
    {
        output = new RenderTexture(256, 256, 8, RenderTextureFormat.ARGB32);
        output.Create();
        GetComponent<Camera>().targetTexture = output;

        displayQuad.GetComponent<MeshRenderer>().material.mainTexture = output;
    }
}