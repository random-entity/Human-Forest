using System.Collections.Generic;
using UnityEngine;

public class FluidSystem : MonoBehaviour
{
    public List<Float2> SVList; // (x = State, y = Value) Pair
    private float count; // Runtime 동안 이 값이 바뀔 일은 없겠지.
    [SerializeField] private List<(float, float, float)> XWHList; // (x = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준.
    private List<Transform> RectList;
    [SerializeField] private Transform RectPrefab;
    // public List<Color> Colors;

    ObjectPooler ObjectPooler;

    private void Awake()
    {
        ObjectPooler = ObjectPooler.instance;

        count = SVList.Count;
        CheckSVListEmpty();

        NormalizeValues();

        RectList = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            RectList.Add(Instantiate(RectPrefab));
        }

        XWHList = new List<(float, float, float)>();

        SetXWHList();
        RectPrefab.gameObject.SetActive(false);
    }

    private void CheckSVListEmpty()
    {
        if (count == 0)
        {
            Debug.LogWarning("SVList.Count == 0");
            return;
        }
    }

    private void NormalizeValues()
    {
        CheckSVListEmpty();

        float sum = 0;

        foreach (Float2 sv in SVList)
        {
            sum += sv.y;
        }

        if (sum <= 0)
        {
            Debug.LogWarning("sum of values <= 0");
            return;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                SVList[i] = new Float2(SVList[i].x, SVList[i].y / sum);
            }
        }
    }

    private Vector3 XWH2Position((float x, float w, float h) xwh)
    {
        return new Vector3(-5f, -5f, 0) + 10f * new Vector3(xwh.x + 0.5f * xwh.w, 0.5f * xwh.h, 0f);
    }

    private Vector3 XWH2Scale((float x, float w, float h) xwh)
    {
        return 10f * new Vector3(xwh.w, xwh.h, 1f); ;
    }

    private void SetXWHList()
    {
        XWHList.Clear();

        float x = 0;

        for (int i = 0; i < count; i++)
        {
            XWHList.Add((x, SVList[i].y, SVList[i].x));
            MatchRectToFloat3(RectList[i], XWHList[i]);
            x += SVList[i].y;
        }
    }

    private void MatchRectToFloat3(Transform rect, (float x, float w, float h) xwh)
    {
        rect.localScale = XWH2Scale(xwh);
        rect.position = XWH2Position(xwh);
    }
}