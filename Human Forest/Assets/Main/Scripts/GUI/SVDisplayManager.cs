using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManager : MonoSingleton<SVDisplayManager>
{
    private HumanForest hf;

    [SerializeField] private Transform SVDisplayPrefab;
    private float SVDisplayWidth = 1f;
    private float SVDisplayIntervalX = 1.5f;

    #region U(p)
    public Dictionary<Person, SVDisplay> SVDisplayGroup_U_p; // RealOrImagePerson p => Quad for displaying U(p)
    public Transform Band_U_p_center;
    public static EvaluationTypes.Utility UEvalType;
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

    public void OnUpdateUEvalTypeOrImageHolder()
    {
        if (ImageHolder.IsGod)
        {
            UEvalType = EvaluationTypes.Utility.Omniscient;
            SetSVDisplayGroup_U_pActiveByImageHolder(God.god);
            Debug.Log("setting UEvalType to Omniscient because imageHolder is God.");
        }
        else if (ImageHolder.IsReal)
        {
            if (UEvalType == EvaluationTypes.Utility.Omniscient)
            {
                UEvalType = EvaluationTypes.Utility.Image_OthersValuesConsiderate;
                Debug.Log("setting UEvalType to Image_OthersValuesConsiderate because imageHolder is a real Person.");
            }

            // 먼저 Omniscient인지 확인하고 맞으면 바꾸고 그 다음 진행

            if (UEvalType == EvaluationTypes.Utility.Image_OthersValuesConsiderate)
            {
                SetSVDisplayGroup_U_pRefToHfPM2SV();
                SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);
            }
            else if (UEvalType == EvaluationTypes.Utility.Image_NonOthersValuesConsiderate)
            {
                SetSVDisplayGroup_U_pRefToPM2SV(hf.QsStatePsValue[ImageHolder]);
                SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);
            }

        }

        // switch (UEvalType)
        // {
        //     case EvaluationTypes.Utility.Omniscient:
        //         SetSVDisplayGroup_U_pActiveByImageHolder(God.god);
        //         break;

        //     case EvaluationTypes.Utility.Image_OthersValuesConsiderate:
        //         SetSVDisplayGroup_U_pRefToHfPM2SV();
        //         SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);
        //         break;

        //     case EvaluationTypes.Utility.Image_NonOthersValuesConsiderate:
        //         SetSVDisplayGroup_U_pRefToPM2SV(hf.QsStatePsValue[ImageHolder]);
        //         SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolder);
        //         break;
        // }
    }

    public void UpdateUEvalType(EvaluationTypes.Utility uEvalType)
    {
        UEvalType = uEvalType;
        OnUpdateUEvalTypeOrImageHolder();
    }

    public void UpdateImageHolder(Person imageHolder)
    {
        ImageHolder = imageHolder;
        Debug.Log("imageHolder set to " + imageHolder.gameObject.name);
        OnUpdateUEvalTypeOrImageHolder();
    }
    #endregion

    #region T(c)
    public Dictionary<EvaluationTypes.TotalUtility, SVDisplay> SVDisplayGroup_T_C;
    public Transform Band_T_C_center;
    #endregion

    public override void Init()
    {
        hf = HumanForest.instance;

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

        // SVDisplayGroup_T_C 등 다른 그룹(Band)에 있는 SVDisplay들도 해줘야... 
    }
    #endregion

    private void OnEnable()
    {
        EventManager.OnGUI_U_p_Click += UpdateImageHolder;
    }
    private void OnDisable()
    {
        EventManager.OnGUI_U_p_Click -= UpdateImageHolder;
    }
}