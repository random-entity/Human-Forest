using System.Collections.Generic;
using UnityEngine;

public struct FluidSpawnConfig
{
    public List<(float x, float y, float w, float h)> NormXYWHList;
    public float frameWidth, frameHeight;
    public List<Color> Swatch;
}

public class SVDisplay : MonoBehaviour
{
    public List<cloat2> SVList; // (x = State, y = Value) Pair
    private int count;

    [SerializeField] private List<(float x, float y, float w, float h)> NormXYWHList; // (xy = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준(normalized).
    [SerializeField] private Transform BorderBottomLeft, BorderTopRight;
    private Vector3 BorderBottomLeftPosition, BorderWidthHeight;

    private List<Transform> RectList;
    [SerializeField] private Transform RectPrefab;
    [SerializeField] private Transform RectParent;
    private Transform RectForWeightedMean;

    public List<Color> Swatch;

    [SerializeField] private FluidSystem fluidSystem;

    private void Awake()
    {
        UpdateSVListCount();

        RectList = new List<Transform>();
        for (int i = 0; i < Const.MaxSVListCount; i++)
        {
            Transform rect_i = Instantiate(RectPrefab, RectParent);
            rect_i.gameObject.SetActive(i < count);

            Color color_i;
            if (i < Swatch.Count) color_i = Swatch[i];
            else color_i = Color.black;

            rect_i.GetComponent<SpriteRenderer>().color = color_i;
            RectList.Add(rect_i);
        }
        RectForWeightedMean = Instantiate(RectPrefab, RectParent);

        NormXYWHList = new List<(float, float, float, float)>();

        OnSVListUpdate();
    }

    private void OnSVListUpdate()
    {
        UpdateSVListCount();
        NormalizeValues();
        UpdateXYWHList();
        UpdateRectList();
        UpdateBorder();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) fluidSystem.SpawnFluidParticles(new FluidSpawnConfig
        {
            NormXYWHList = NormXYWHList,
            Swatch = Swatch,
            frameWidth = 10f,
            frameHeight = 10f
        });
        if (Input.GetKeyDown(KeyCode.Alpha3)) RectParent.gameObject.SetActive(!RectParent.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Alpha4)) RectForWeightedMean.gameObject.SetActive(!RectForWeightedMean.gameObject.activeInHierarchy);

        OnSVListUpdate();
    }

    #region Housekeeping
    private void UpdateSVListCount()
    {
        if (count == 0)
        {
            Debug.LogWarning("SVList.Count == 0");
        }

        count = SVList.Count;
    }

    private void NormalizeValues()
    {
        UpdateSVListCount();

        float sum = 0;

        foreach (cloat2 sv in SVList)
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
                SVList[i] = new cloat2(SVList[i].x, SVList[i].y / sum);
            }
        }
    }

    private void UpdateXYWHList()
    {
        NormXYWHList.Clear();

        float x = 0;

        for (int i = 0; i < count; i++)
        {
            NormXYWHList.Add((x, 0f, SVList[i].y, SVList[i].x));
            MatchTransformToXYWH(RectList[i], NormXYWHList[i]);
            x += SVList[i].y;
        }

        MatchTransformToXYWH(RectForWeightedMean, (0f, 0f, 1f, GetWeightedMeans()));
    }

    private void UpdateRectList()
    {
        for (int i = 0; i < Const.MaxSVListCount; i++)
        {
            RectList[i].gameObject.SetActive(i < count);
        }
    }

    private void UpdateBorder()
    {
        BorderBottomLeftPosition = BorderBottomLeft.position;
        BorderWidthHeight = BorderTopRight.position - BorderBottomLeft.position;
    }

    private float GetWeightedMeans()
    {
        NormalizeValues();

        float m = 0f;
        for (int i = 0; i < count; i++)
        {
            m += SVList[i].y * SVList[i].x;
        }
        return m;
    }

    #region XWH2Vector3
    private Vector3 XYWH2Position((float x, float y, float w, float h) xywh)
    {
        return BorderBottomLeftPosition + new Vector3(BorderWidthHeight.x * (xywh.x + 0.5f * xywh.w), BorderWidthHeight.y * (xywh.y + 0.5f * xywh.h), 0f);
    }

    private Vector3 XYWH2Scale((float x, float y, float w, float h) xywh)
    {
        return new Vector3(BorderWidthHeight.x * xywh.w, BorderWidthHeight.y * xywh.h, 1f); ;
    }

    private void MatchTransformToXYWH(Transform t, (float x, float y, float w, float h) xywh)
    {
        t.localScale = XYWH2Scale(xywh);
        t.position = XYWH2Position(xywh);
    }
    #endregion
    #endregion
}
