using System.Collections.Generic;
using UnityEngine;

public static class Extensions
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

    public static Dictionary<T, float> SplitDictionary<T>(Dictionary<T, cloat2> original, bool xTrueYFalse)
    {
        Dictionary<T, float> split = new Dictionary<T, float>();

        foreach (T key in original.Keys)
        {
            cloat2 sv = original[key];
            if (xTrueYFalse)
            {
                split.Add(key, sv.x);
            }
            else
            {
                split.Add(key, sv.y);
            }
        }

        return split;
    }
}

public static class Const
{
    public const int MaxSVListCount = 15;
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

    public void add(float dx, float dy)
    {
        x += dx;
        y += dy;
    }

    public void addClamp(float dx, float dy)
    {
        x += dx;
        x = Mathf.Clamp(x, 0f, 1f);
        y += dy;
        y = Mathf.Clamp(y, 0f, 1f);
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