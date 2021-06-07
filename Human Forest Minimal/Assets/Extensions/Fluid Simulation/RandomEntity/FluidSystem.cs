using System.Collections.Generic;
using UnityEngine;

public class FluidSystem : MonoBehaviour
{
    public List<CFloat2> SVList; // (x = State, y = Value) Pair
    private float count; // Runtime 동안 이 값이 바뀔 일은 없겠지.
    [SerializeField] private List<(float, float, float)> XWHList; // (x = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준.
    [SerializeField] private Vector3 FrameBottomLeftPosition, FrameWidthHeight;
    private List<Transform> RectList;
    [SerializeField] private Transform RectPrefab;
    [SerializeField] private GameObject FluidParticlePrefab; // CircleCollider2D Radius 알려고.
    [SerializeField] private float particleDiameter, particleDiameterNormalizedX, particleDiameterNormalizedY;
    ObjectPooler ObjectPooler;

    private void Awake()
    {
        particleDiameter = FluidParticlePrefab.GetComponent<CircleCollider2D>().radius * 2f;
        particleDiameterNormalizedX = particleDiameter / FrameWidthHeight.x;
        particleDiameterNormalizedY = particleDiameter / FrameWidthHeight.y;

        ObjectPooler = ObjectPooler.instance;

        count = SVList.Count;
        CheckSVListEmpty();

        RectList = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            Transform rect_i = Instantiate(RectPrefab);
            rect_i.gameObject.SetActive(true);
            RectList.Add(rect_i);
        }

        XWHList = new List<(float, float, float)>();

        NormalizeValues();
        UpdateXWHList();

        RectPrefab.gameObject.SetActive(false);

        SpawnFluidParticles();
    }

    private void SpawnFluidParticles()
    {
        for (int i = 0; i < count; i++)
        {
            var xwh_i = XWHList[i];
            float x = xwh_i.Item1;
            float w = xwh_i.Item2;
            float h = xwh_i.Item3;

            for (float px = x; px < x + w; px += particleDiameterNormalizedX)
            {
                for (float py = 0f; py < h; py += particleDiameterNormalizedY)
                {
                    ObjectPooler.SpawnFromPool("Fluid", XWH2Position((px, py, 0)));
                }
            }
        }
    }

    private void UpdateXWHList()
    {
        XWHList.Clear();

        float x = 0;

        for (int i = 0; i < count; i++)
        {
            XWHList.Add((x, SVList[i].y, SVList[i].x));
            MatchTransformToXWH(RectList[i], XWHList[i]);
            x += SVList[i].y;
        }
    }

    #region Housekeeping
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

        foreach (CFloat2 sv in SVList)
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
                SVList[i] = new CFloat2(SVList[i].x, SVList[i].y / sum);
            }
        }
    }
    #endregion

    #region XWH2Vector3
    private Vector3 XWH2Position((float x, float w, float h) xwh)
    {
        return FrameBottomLeftPosition + new Vector3(FrameWidthHeight.x * (xwh.x + 0.5f * xwh.w), FrameWidthHeight.y * (0.5f * xwh.h), 0f);
    }

    private Vector3 XWH2Scale((float x, float w, float h) xwh)
    {
        return new Vector3(FrameWidthHeight.x * xwh.w, FrameWidthHeight.y * xwh.h, 1f); ;
    }

    private void MatchTransformToXWH(Transform t, (float x, float w, float h) xwh)
    {
        t.localScale = XWH2Scale(xwh);
        t.position = XWH2Position(xwh);
    }
    #endregion
}