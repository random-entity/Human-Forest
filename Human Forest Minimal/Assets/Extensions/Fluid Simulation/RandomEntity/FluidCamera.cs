using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCamera : MonoBehaviour
{
    public RenderTexture output;
    [SerializeField] private GameObject quad;

    private void Awake()
    {
        output = new RenderTexture(256, 256, 8, RenderTextureFormat.ARGB32);
        output.Create();

        quad.GetComponent<MeshRenderer>().material.mainTexture = output;
    }
}
