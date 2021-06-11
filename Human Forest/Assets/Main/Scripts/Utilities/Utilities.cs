using UnityEngine;

public static class Utilities
{
    public static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}

public struct PersonPair
{
    Person Subject, Object;

    public PersonPair(Person sub, Person obj)
    {
        this.Subject = sub;
        this.Object = obj;
    }
}

[System.Serializable]
public struct sloat2
{
    public float x, y;

    public sloat2(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

[System.Serializable]
public class cloat2
{
    public float x, y;

    public cloat2(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

[System.Serializable]
public class f_0_1_inf
{
    public float f_0, f_1, f_inf;

    public f_0_1_inf(float f_0, float f_1, float f_inf)
    {
        this.f_0 = f_0;
        this.f_1 = f_1;
        this.f_inf = f_inf;
    }
}