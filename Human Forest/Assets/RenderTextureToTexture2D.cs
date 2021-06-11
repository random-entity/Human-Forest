using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureToTexture2D : MonoBehaviour
{
    [SerializeField] private RenderTexture FluidRenderTexture;
    [SerializeField] private Rect rect;
    [SerializeField] private Vector2 pivot;

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(Utilities.toTexture2D(FluidRenderTexture), rect, pivot);
    }
}
