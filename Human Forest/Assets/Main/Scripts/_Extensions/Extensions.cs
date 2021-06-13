using System;
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
}

public static class DictExt
{
    public static Dictionary<T, cloat> SplitDictionary<T>(Dictionary<T, cloat2> original, bool xTrueYFalse) // T는 Enum이어야 합니다.
    {
        Dictionary<T, cloat> split = new Dictionary<T, cloat>();

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

    public static Dictionary<T, cloat2> MergeDictionary<T>(Dictionary<T, cloat> s, Dictionary<T, cloat> sigma) // s와 sigma는 같은 Keys를 갖고 있어야 합니다.
    {
        Dictionary<T, cloat2> merged = new Dictionary<T, cloat2>();

        foreach (T key in s.Keys)
        {
            cloat2 sv = new cloat2(s[key], sigma[key]);

            merged.Add(key, sv);
        }

        return merged;
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
public class cloat2
{
    public cloat x, y;

    public cloat2(float _x, float _y)
    {
        x = new cloat(_x);
        y = new cloat(_y);
    }

    public cloat2(cloat _x, cloat _y)
    {
        x = _x;
        y = _y;
    }

    public void add(float dx, float dy)
    {
        x.add(dx);
        y.add(dy);
    }

    public void addClamp(float dx, float dy)
    {
        add(dx, dy);
        x.addClamp(0f);
        y.addClamp(0f);
    }
}

public class cloat
{
    public float f;

    public cloat(float _f)
    {
        f = _f;
    }

    public void add(float df)
    {
        f += df;
    }

    public void addClamp(float df)
    {
        add(df);
        f = Mathf.Clamp(f, 0.001f, 1f);
    }
}