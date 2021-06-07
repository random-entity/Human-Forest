using System.Collections.Generic;
using UnityEngine;

public class FluidSystem : MonoBehaviour
{
    public List<CFloat2> SVList; // (x = State, y = Value) Pair
    private float count; // Runtime 동안 이 값이 바뀔 일은 없겠지.
    [SerializeField] private List<(float, float, float, float)> XYWHList; // (xy = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준.
    [SerializeField] private Vector3 FrameBottomLeftPosition, FrameWidthHeight;
    private List<Transform> RectList;
    [SerializeField] private Transform RectPrefab;
    [SerializeField] private GameObject FluidParticlePrefab; // CircleCollider2D Radius 알려고.
    [SerializeField] private float particleDiameter, particleDNormX, particleDNormY;
    ObjectPooler ObjectPooler;

    private void Awake()
    {
        particleDiameter = FluidParticlePrefab.transform.localScale.x * FluidParticlePrefab.GetComponent<CircleCollider2D>().radius * 2f;
        particleDNormX = particleDiameter / FrameWidthHeight.x;
        particleDNormY = particleDiameter / FrameWidthHeight.y;

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

        XYWHList = new List<(float, float, float, float)>();

        NormalizeValues();
        UpdateXYWHList();

        RectPrefab.gameObject.SetActive(false);

        SpawnFluidParticles();
    }

    private void SpawnFluidParticles()
    {
        for (int i = 0; i < count; i++)
        {
            var xywh_i = XYWHList[i];

            float x = xywh_i.Item1;
            float y = xywh_i.Item2;
            float w = xywh_i.Item3;
            float h = xywh_i.Item4;

            for (float px = x; px + particleDNormX < x + w; px += particleDNormX)
            {
                for (float py = y; py + particleDNormY < y + h; py += particleDNormY)
                {
                    ObjectPooler.SpawnFromPool("Fluid", XYWH2Position((px, py, particleDNormX, particleDNormY)));
                }
            }
        }
    }

    private void UpdateXYWHList()
    {
        XYWHList.Clear();

        float x = 0;

        for (int i = 0; i < count; i++)
        {
            XYWHList.Add((x, 0f, SVList[i].y, SVList[i].x));
            MatchTransformToXYWH(RectList[i], XYWHList[i]);
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
    private Vector3 XYWH2Position((float x, float y, float w, float h) xywh)
    {
        return FrameBottomLeftPosition + new Vector3(FrameWidthHeight.x * (xywh.x + 0.5f * xywh.w), FrameWidthHeight.y * (xywh.y + 0.5f * xywh.h), 0f);
    }

    private Vector3 XYWH2Scale((float x, float y, float w, float h) xywh)
    {
        return new Vector3(FrameWidthHeight.x * xywh.w, FrameWidthHeight.y * xywh.h, 1f); ;
    }

    private void MatchTransformToXYWH(Transform t, (float x, float y, float w, float h) xywh)
    {
        t.localScale = XYWH2Scale(xywh);
        t.position = XYWH2Position(xywh);
    }
    #endregion
}