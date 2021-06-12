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
    // 여기에서 OnClick 이벤트 발생시키자.
    // 테두리도 만들자.

    #region field declarations
    public List<cloat2> SVList; // (x = State, y = Value) Pair
    private int count;

    [SerializeField] private List<(float x, float y, float w, float h)> NormXYWHList; // (xy = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준(normalized).

    public Transform BorderBottomLeft, BorderTopRight;
    private Vector3 BorderBottomLeftPosition, BorderWidthHeight;

    private List<Transform> RectList;
    [SerializeField] private Transform RectPrefab;
    [SerializeField] private Transform RectParent;
    private Transform RectForWeightedMean;

    public List<Color> Swatch;

    private FluidSystem fluidSystem;
    #endregion

    private void Awake()
    {
        fluidSystem = FluidSystem.instance;

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
    }

    private void Start()
    {
        OnUpdateSVList();
    }

    private void Update()
    {
        #region Debug
        // if (Input.GetKeyDown(KeyCode.Alpha1)) fluidSystem.SpawnFluidParticles(new FluidSpawnConfig
        // {
        //     NormXYWHList = NormXYWHList,
        //     Swatch = Swatch,
        //     frameWidth = 10f,
        //     frameHeight = 10f
        // });
        if (Input.GetKeyDown(KeyCode.Alpha3)) OnUpdateSVList();
        if (Input.GetKeyDown(KeyCode.Alpha4)) RectForWeightedMean.gameObject.SetActive(!RectForWeightedMean.gameObject.activeInHierarchy);
        #endregion
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     OnUpdateSVList();
        // }
        //
    }

    #region Event Subscription
    // private void OnEnable()
    // {
    //     EventManager.OnUpdatePM2SV += OnUpdateSVList;
    // }
    // private void OnDisable()
    // {
    //     EventManager.OnUpdatePM2SV -= OnUpdateSVList;
    // }
    #endregion

    #region OnUpdateSVList
    private void OnUpdateSVList()
    {
        UpdateSVListCount();
        NormalizeValues();
        UpdateXYWHList();
        UpdateRectList();
        UpdateBorder();
        UpdateWeightedMeans();
        MatchRectListTransformToXYWH();
    }

    private void UpdateSVListCount()
    {
        count = SVList.Count;

        if (count == 0)
        {
            Debug.LogWarning("SVList.Count == 0");
        }
    }

    private void NormalizeValues() // UpdateSVListCount를 먼저 하세요
    {
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

    private void UpdateXYWHList() // NormalizeValues를 먼저 하세요. 
    {
        NormXYWHList.Clear();

        float x = 0;

        for (int i = 0; i < count; i++)
        {
            NormXYWHList.Add((x, 0f, SVList[i].y, SVList[i].x));
            x += SVList[i].y;
        }
    }

    private void MatchRectListTransformToXYWH() // UpdateRectList와 UpdateBorder를 한 후에 하세요.
    {
        for (int i = 0; i < count; i++)
        {
            MatchTransformToXYWH(RectList[i], NormXYWHList[i]);
        }

        MatchTransformToXYWH(RectForWeightedMean, (0f, 0f, 1f, UpdateWeightedMeans()));
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

    private float UpdateWeightedMeans() //NormalizeValues를 먼저 하세요.
    {
        float m = 0f;
        for (int i = 0; i < count; i++)
        {
            m += SVList[i].y * SVList[i].x;
        }
        return m;
    }
    #endregion

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
}
