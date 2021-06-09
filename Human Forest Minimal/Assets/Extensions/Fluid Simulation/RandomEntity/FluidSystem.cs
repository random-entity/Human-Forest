using System.Collections.Generic;
using UnityEngine;

public class FluidSystem : MonoBehaviour
{
    public List<CFloat2> SVList; // (x = State, y = Value) Pair

    private float count; // Runtime 동안 이 값이 바뀔 일은 없겠지.
    
    [SerializeField] private GameObject FluidParticlePrefab; // CircleCollider2D Radius 알려고.
    private float particleDiameter, particleDNormX, particleDNormY;
    [SerializeField] private Transform FluidParticlesParent;
    
    [SerializeField] private List<(float x, float y, float w, float h)> XYWHList; // (xy = 왼쪽아래꼭지점의 x좌표) 이건 [0, 1]^3 기준.
    [SerializeField] private Vector3 FrameBottomLeftPosition, FrameWidthHeight;

    private List<Transform> RectList;
    [SerializeField] private Transform RectTemplatePrefab;
    [SerializeField] private Transform RectTemplateParent;

    private List<Transform> BarrierList;
    [SerializeField] private Transform BarrierPrefab;
    [SerializeField] private Transform BarrierParent;

    [SerializeField] private Transform propellerParent;
    [SerializeField] private float propellerSpeed;

    // [SerializeField] private ScriptableObject Swatch;
    public List<Color> Swatch;

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
        BarrierList = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            Transform rect_i = Instantiate(RectTemplatePrefab, RectTemplateParent);
            rect_i.gameObject.SetActive(true);
            Color color_i = Swatch[i];
            rect_i.GetComponent<SpriteRenderer>().color = new Color(color_i.r, color_i.g, color_i.b, 0.5f);
            RectList.Add(rect_i);

            if (i == count - 1) break;

            Transform b_i = Instantiate(BarrierPrefab, BarrierParent);
            b_i.gameObject.SetActive(true);
            BarrierList.Add(b_i);
        }

        XYWHList = new List<(float, float, float, float)>();
        NormalizeValues();
        UpdateXYWHList();
        SetBarrierFromSVList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SpawnFluidParticles();
        if (Input.GetKeyDown(KeyCode.Alpha2)) ObjectPooler.instance.DeactivateAll("Fluid");
        if (Input.GetKeyDown(KeyCode.Alpha3)) RectTemplateParent.gameObject.SetActive(!RectTemplateParent.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Alpha4)) BarrierParent.gameObject.SetActive(!BarrierParent.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Alpha5)) propellerParent.gameObject.SetActive(!propellerParent.gameObject.activeInHierarchy);

        RunPropeller();
    }

    private void RunPropeller()
    {
        int i = 0;
        foreach (Transform propeller in propellerParent.transform)
        {
            i++;
            float direction = i % 2 == 0 ? -1f : 1f;
            propeller.transform.RotateAround(propeller.transform.position, Vector3.forward, direction * propellerSpeed * Time.deltaTime);
        }
    }

    private void SpawnFluidParticles()
    {
        ObjectPooler.instance.DeactivateAll("Fluid");

        for (int i = 0; i < count; i++)
        {
            var xywh_i = XYWHList[i];

            float x = xywh_i.x;
            float y = xywh_i.y;
            float w = xywh_i.w;
            float h = xywh_i.h;

            Color color = Swatch[i];

            for (float px = x; px + particleDNormX < x + w; px += particleDNormX)
            {
                for (float py = y; py + particleDNormY < 1.15f * (y + h); py += particleDNormY)
                {
                    ObjectPooler.SpawnFromPool("Fluid", XYWH2Position((px, py, particleDNormX, particleDNormY)), FluidParticlesParent, color);
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

    private void SetBarrierFromSVList()
    {
        for (int i = 0; i < count - 1; i++)
        {
            BarrierList[i].transform.position = new Vector3(FrameBottomLeftPosition.x + FrameWidthHeight.x * XYWHList[i + 1].x, 0f, 0f);
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