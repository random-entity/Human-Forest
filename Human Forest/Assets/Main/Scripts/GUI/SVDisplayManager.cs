using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManager : MonoBehaviour
{
    private HumanForest hf;
    private void Awake()
    {
        hf = HumanForest.instance;
    }

    [SerializeField] private Transform SVDisplayPrefab;
    private float SVDisplayWidth = 1f;
    private float SVDisplayIntervalX = 1.5f;

    public Dictionary<Person, SVDisplay> SVDisplayGroup_U_p; // RealOrImagePerson p => Quad for displaying U(p)
    public Transform Band_U_p_center;
    public static EvaluationTypes.Utility uEvalType;
    public static Person ImageHolder;

    private void InitializeSVDisplayGroup_U_p()
    {
        SVDisplayGroup_U_p = new Dictionary<Person, SVDisplay>();

        int index = 0;
        foreach (Person p in hf.RealAndImagesSociety)
        {
            Transform svd_U_p_Transform = Instantiate(SVDisplayPrefab);
            svd_U_p_Transform.gameObject.name = "SVDisp_U_p(" + p.gameObject.name + ")";

            float imageHolderIndex = index % hf.InitialPersonCount;
            float normPos = imageHolderIndex - 0.5f * (hf.InitialPersonCount - 1);
            svd_U_p_Transform.position = Band_U_p_center.position + Vector3.right * (SVDisplayIntervalX * normPos);
            svd_U_p_Transform.SetParent(Band_U_p_center);

            svd_U_p_Transform.gameObject.SetActive(p.IsReal);

            SVDisplayGroup_U_p.Add(p, svd_U_p_Transform.GetComponent<SVDisplay>());
            index++;
        }

        SetSVDisplayGroup_U_pRefToHfPM2SV();
    }

    public void SetSVDisplayGroup_U_pRefToPM2SV(Dictionary<Person, Dictionary<Matter, cloat2>> p2sv)
    {
        foreach (Person p in hf.RealAndImagesSociety)
        {
            SVDisplay svd = SVDisplayGroup_U_p[p];

            List<cloat2> sv = new List<cloat2>();
            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                sv.Add(p2sv[p][m]);
            }
            svd.SVList = sv;
        }
    }

    public void SetSVDisplayGroup_U_pRefToHfPM2SV()
    {
        SetSVDisplayGroup_U_pRefToPM2SV(hf.PM2SV);
    }

    public void SetSVDisplayGroup_U_pActiveByImageHolder(Person imageHolder)
    {
        if (imageHolder.IsGod)
        {
            foreach (Person p in hf.RealAndImagesSociety)
            {
                SVDisplayGroup_U_p[p].gameObject.SetActive(p.IsReal);
            }
        }
        else
        {
            foreach (Person p in hf.RealAndImagesSociety)
            {
                SVDisplayGroup_U_p[p].gameObject.SetActive(p.ImageHolder == imageHolder);
            }
        }
    }

    private void OnUpdateUEvalType(EvaluationTypes.Utility uType)
    {
        switch (uType)
        {
            case EvaluationTypes.Utility.Omniscient:
                SetSVDisplayGroup_U_pActiveByImageHolder(God.god);
                break;
            case EvaluationTypes.Utility.Image_OthersValuesConsiderate:
                SetSVDisplayGroup_U_pRefToHfPM2SV();
                SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);

                break;
            case EvaluationTypes.Utility.Image_NonOthersValuesConsiderate:
                SetSVDisplayGroup_U_pRefToPM2SV(hf.QsStatePsValue[ImageHolder]);
                SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);
                break;
        }
    }

    public Dictionary<EvaluationTypes.TotalUtility, SVDisplay> SVDisplayGroup_T_C;
    public Transform Band_T_C_center;

    private void Start()
    {
        if (ImageHolder == null) ImageHolder = hf.RealSociety[0];

        InitializeSVDisplayGroup_U_p();

        UpdateSVDisplaySize();
    }

    #region OnUpdate SVDisplay Transform 
    // SVDisplay가 갖고 있는 SVList의 Update에 의한 변화는 SVDisplay 클래스에서 처리할 것이다.
    public void UpdateSVDisplaySize()
    {
        foreach (SVDisplay svd in SVDisplayGroup_U_p.Values)
        {
            svd.BorderBottomLeft.position = svd.transform.position - new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
            svd.BorderTopRight.position = svd.transform.position + new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
        }

        // 다른 그룹(Band)에 있는 SVDisplay들도 해줘야...
    }
    #endregion

    int i = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int im = i % Enum.GetNames(typeof(EvaluationTypes.Utility)).Length;
            Debug.Log((EvaluationTypes.Utility)im);
            OnUpdateUEvalType((EvaluationTypes.Utility)im);
            EventManager.InvokeOnUpdateSVListRef();
            i++;
        }
    }
}