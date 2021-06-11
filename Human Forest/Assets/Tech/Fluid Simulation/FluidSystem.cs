using System.Collections.Generic;
using UnityEngine;

public class FluidSystem : MonoSingleton<FluidSystem>
{
    // Particle에 대한 것들.
    [SerializeField] private Transform FluidParticlesParent;
    [SerializeField] private GameObject FluidParticlePrefab; // CircleCollider2D Radius 알려고.
    private float particleDiameter;
    private const float halfSqrt3 = 0.86602540378f;

    // Fluid Frame에 대한 것들.
    [SerializeField] private Transform FluidSystemOrigin; // 기준점 알려고.
    private Vector3 FrameBottomLeftPosition, FrameWidthHeight;
    [SerializeField] private Transform fluidFrameParent, fluidFramePartPrefab;
    private Dictionary<int, Transform> fluidFrameParts;

    // Fluid Barrier들.
    private List<Transform> BarrierList;
    [SerializeField] private Transform BarrierPrefab;
    [SerializeField] private Transform BarrierParent;

    // 프로펠러.
    [SerializeField] private Transform propellerParent;
    [SerializeField] private float propellerSpeed;

    // 가장 중요한 ObjectPooler instance caching.
    ObjectPooler objectPooler;

    public override void Init()
    {
        objectPooler = ObjectPooler.instance;

        particleDiameter = FluidParticlePrefab.transform.localScale.x * FluidParticlePrefab.GetComponent<CircleCollider2D>().radius * 2f;

        // 처음에만 10 x 10 틀로 하드코딩해. 나중엔 config에서 받아. 변수 갖고 있을 필요 없잖아.
        FrameBottomLeftPosition = FluidSystemOrigin.position - new Vector3(10f, 10f, 0f);
        FrameWidthHeight = new Vector3(10f, 10f, 0f);

        fluidFrameParts = new Dictionary<int, Transform>();
        for (int i = 0; i < 4; i++)
        {
            Transform part = Instantiate(fluidFramePartPrefab, fluidFrameParent);
            fluidFrameParts.Add(i, part); // 0 = left, 1 = right, 2 = bottom, 3 = top
            part.gameObject.SetActive(true);
        }
        BuildFluidFrame(10f, 10f);

        BarrierList = new List<Transform>();
        for (int i = 0; i < Const.MaxSVListCount; i++)
        {
            Transform b_i = Instantiate(BarrierPrefab, BarrierParent);
            b_i.gameObject.SetActive(true);
            BarrierList.Add(b_i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) objectPooler.DeactivateAll("Fluid");
        if (Input.GetKeyDown(KeyCode.Alpha5)) BarrierParent.gameObject.SetActive(!BarrierParent.gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Alpha6)) propellerParent.gameObject.SetActive(!propellerParent.gameObject.activeInHierarchy);

        RunPropeller();
    }

    public void SpawnFluidParticles(FluidSpawnConfig config)
    {
        OnUpdateConfig(config);

        objectPooler.DeactivateAll("Fluid");

        float particleDNormX = particleDiameter / config.frameWidth;
        float particleDNormY = particleDiameter / config.frameHeight;

        float rnx = particleDNormX * 0.5f; // radius, normalized, x방향
        float rny = particleDNormY * 0.5f; // radius, normalized, y방향

        for (int i = 0; i < config.NormXYWHList.Count; i++)
        {
            var xywh_i = config.NormXYWHList[i];

            float x = xywh_i.x;
            float y = xywh_i.y;
            float w = xywh_i.w;
            float h = xywh_i.h;

            Color color;
            if (i < config.Swatch.Count) color = config.Swatch[i];
            else color = Color.black;

            bool even = true;
            for (float py = y; py < (y + h); py += halfSqrt3 * particleDNormY)
            {
                for (float px = x + (even ? 0f : particleDNormX); px + particleDNormX < x + w; px += particleDNormX)
                {
                    objectPooler.SpawnFromPool("Fluid", XYWH2Position((px, py, particleDNormX, particleDNormY)), FluidParticlesParent, color, 1f);
                }

                even = !even;
            }
        }
    }

    #region config 받으면 업데이트해야 하는 것들.
    // 통합
    private void OnUpdateConfig(FluidSpawnConfig config)
    {
        UpdateFrameSize(config);
        BuildFluidFrame(config.frameWidth, config.frameHeight);
        UpdateBarriers(config);
    }

    private void UpdateBarriers(FluidSpawnConfig config)
    {
        var XYWHList = config.NormXYWHList;

        for (int i = 0; i < Const.MaxSVListCount; i++)
        {
            var b_i = BarrierList[i];

            if (i < config.NormXYWHList.Count - 1)
            {
                b_i.gameObject.SetActive(true);
                MatchTransformToXYWH(b_i, (XYWHList[i + 1].x, XYWHList[i + 1].y, 0.01f, 1f));
            }
            else
            {
                b_i.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateFrameSize(FluidSpawnConfig config)
    {
        FrameBottomLeftPosition = FluidSystemOrigin.position - new Vector3(config.frameWidth * 0.5f, config.frameHeight * 0.5f, 0f);
        FrameWidthHeight = new Vector3(config.frameWidth, config.frameHeight, 0f);
    }

    private void BuildFluidFrame(float fluidFrameWidth, float fluidFrameHeight)
    {
        var left = fluidFrameParts[0];
        left.localPosition = new Vector3(-fluidFrameWidth * 0.5f - 1, 0f, 0f);
        left.localScale = new Vector3(2f, fluidFrameHeight + 4f, 1f);

        var right = fluidFrameParts[1];
        right.localPosition = new Vector3(fluidFrameWidth * 0.5f + 1, 0f, 0f);
        right.localScale = new Vector3(2f, fluidFrameHeight + 4f, 1f);

        var bottom = fluidFrameParts[2];
        bottom.localPosition = new Vector3(0f, -fluidFrameHeight * 0.5f - 1, 0f);
        bottom.localScale = new Vector3(fluidFrameWidth + 4f, 2f, 1f);

        var top = fluidFrameParts[3];
        top.localPosition = new Vector3(0f, fluidFrameHeight * 0.5f + 1, 0f);
        top.localScale = new Vector3(fluidFrameWidth + 4f, 2f, 1f);
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
    #endregion
}