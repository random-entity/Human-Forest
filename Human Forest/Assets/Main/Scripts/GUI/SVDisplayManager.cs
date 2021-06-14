using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SVDisplayManager : MonoSingleton<SVDisplayManager>
{
    private static HumanForest hf;

    [SerializeField] private Transform SVDisplayPrefab;
    private float SVDisplayWidth = 1f;
    public float SVDisplayIntervalX = 1.5f;


    #region U(p)

    public static EvaluationTypes.Utility UEvalTypeCurrent;
    public static EvaluationTypes.Utility UEvalTypeWhenImageHolderRealPerson;
    public static EvaluationTypes.Utility UEvalTypeWhenImageHolderGod; // 변하지 않을 것이다.
    public static Person ImageHolderCurrent;
    public static Person ImageHolderWhenGod; // 변하지 않을 것이다.
    public static Person ImageHolderWhenRealPerson;

    public Dictionary<Person, SVDisplay> SVDisplayGroup_U_p; // RealOrImagePerson p => Quad for displaying U(p)
    public Transform Band_U_p_center;

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
            svd_U_p_Transform.localPosition += new Vector3(0f, 0f, -0.5f);

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

    public void UpdateUEvalTypeOrImageHolder()
    {
        Debug.Log("sVDisplayManager.UpdateUEvalTypeOrImageHolder");

        switch (UEvalTypeCurrent)
        {
            case EvaluationTypes.Utility.Omniscient:
                SetSVDisplayGroup_U_pRefToHfPM2SV();
                break;

            case EvaluationTypes.Utility.Image_OthersValuesConsiderate:
                SetSVDisplayGroup_U_pRefToHfPM2SV();
                break;

            case EvaluationTypes.Utility.Image_NonOthersValuesConsiderate:
                SetSVDisplayGroup_U_pRefToPM2SV(hf.QsStatePsValue[ImageHolderCurrent]);
                break;

            default:
                break;
        }

        SetSVDisplayGroup_U_pActiveByImageHolder(ImageHolderCurrent);

        EventManager.InvokeOnUpdateSV(); // SVList들에게 무슨 변화가 생기면 꼭 불러줍시다.
    }

    // ImageHolder = ~~~ 나 UEvalTypeCurrent = ~~~ 같이 직접 assign하지 말고 Update뭐시기 메소드를 이용하세요.

    public void UpdateUEvalType(EvaluationTypes.Utility newUEvalType)
    {
        if (newUEvalType == EvaluationTypes.Utility.Omniscient)
        {
            ImageHolderCurrent = ImageHolderWhenGod;
            UEvalTypeCurrent = newUEvalType;
        }
        else
        {
            UEvalTypeWhenImageHolderRealPerson = newUEvalType;
            UEvalTypeCurrent = UEvalTypeWhenImageHolderRealPerson;

            if (ImageHolderCurrent == God.god)
            {
                ImageHolderCurrent = ImageHolderWhenRealPerson;
            }
        }

        UpdateUEvalTypeOrImageHolder();
    }

    public void UpdateImageHolder(Person newImageHolder)
    {
        Debug.Log("sVDisplayManager.UpdateImageHolder");
        if (newImageHolder.IsGod)
        {
            ImageHolderCurrent = ImageHolderWhenGod;
            UEvalTypeCurrent = UEvalTypeWhenImageHolderGod;
        }
        else
        {
            ImageHolderWhenRealPerson = newImageHolder;
            ImageHolderCurrent = ImageHolderWhenRealPerson;
            UEvalTypeCurrent = UEvalTypeWhenImageHolderRealPerson;
        }

        UpdateUEvalTypeOrImageHolder();
    }
    #endregion

    #region T(c)
    public Dictionary<EvaluationTypes.TotalUtility, SVDisplay> SVDisplayGroup_T_C;
    public Transform Band_T_C_center;
    public Person T_Evaluator;

    private void InitializeSVDisplayGroup_T_C()
    {
        SVDisplayGroup_T_C = new Dictionary<EvaluationTypes.TotalUtility, SVDisplay>();

        foreach (EvaluationTypes.TotalUtility c in Enum.GetValues(typeof(EvaluationTypes.TotalUtility)))
        {
            Transform svd_T_C_Transform = Instantiate(SVDisplayPrefab);
            svd_T_C_Transform.gameObject.name = "SVDisp_T_c(" + c.ToString() + ")";

            float normPos = (int)c - 0.5f * (Enum.GetNames(typeof(EvaluationTypes.TotalUtility)).Length - 1);
            svd_T_C_Transform.position = Band_T_C_center.position + Vector3.right * (SVDisplayIntervalX * normPos);
            svd_T_C_Transform.SetParent(Band_T_C_center);
            svd_T_C_Transform.localPosition += new Vector3(0f, 0f, -0.5f);

            SVDisplayGroup_T_C.Add(c, svd_T_C_Transform.GetComponent<SVDisplay>());
        }

        UpdateSVDisplayGroup_T_C();
    }

    public void UpdateSVDisplayGroup_T_C()
    {
        Debug.Log("sVDisplayManager.UpdateSVDisplayGroup_T_C");
        foreach (EvaluationTypes.TotalUtility c in Enum.GetValues(typeof(EvaluationTypes.TotalUtility)))
        {
            var svd = SVDisplayGroup_T_C[c];

            List<cloat2> p2uc = new List<cloat2>();

            if (ImageHolderCurrent.IsReal)
            {
                foreach (Person q in hf.RealSociety)
                {
                    var CFunction = CFunctionPresets.GetCPreset[c];

                    float U_imageQ = hf.Utility(UEvalTypeCurrent, ImageHolderCurrent, q); // hf.Utility 함수 안에서 q를 p's image of q로 만들어서 계산한다. argument로 넣는 것은 RealPerson.

                    p2uc.Add(new cloat2(U_imageQ, CFunction[hf.PsImageOfQs[ImageHolderCurrent][q]].f));
                }
            }
            else
            {
                foreach (Person p in hf.RealSociety)
                {
                    p2uc.Add(new cloat2(hf.Utility(p), CFunctionPresets.GetCPreset[EvaluationTypes.TotalUtility.Equalitarian][p].f));
                }
            }

            svd.SVList = p2uc;
        }
    }

    #endregion

    public override void Init()
    {
        hf = HumanForest.instance;

        ImageHolderWhenRealPerson = hf.RealSociety[0];
        ImageHolderWhenGod = God.god; // 변하지 않을 것이다.
        ImageHolderCurrent = God.god;

        UEvalTypeWhenImageHolderRealPerson = EvaluationTypes.Utility.Image_OthersValuesConsiderate;
        UEvalTypeWhenImageHolderGod = EvaluationTypes.Utility.Omniscient; // 변하지 않을 것이다.
        UEvalTypeCurrent = EvaluationTypes.Utility.Omniscient;

        InitializeSVDisplayGroup_U_p();

        InitializeSVDisplayGroup_T_C();

        UpdateAllSVDisplayBorders();
    }

    #region OnUpdate SVDisplay Border/Size/Transform 
    // SVDisplay가 갖고 있는 SVList의 Update에 의한 변화는 SVDisplay 클래스에서 처리할 것이다.
    public void UpdateAllSVDisplayBorders()
    {
        foreach (SVDisplay svd in SVDisplayGroup_U_p.Values)
        {
            UpdateSVDisplayBorders(svd);
        }
        foreach (SVDisplay svd in SVDisplayGroup_T_C.Values)
        {
            UpdateSVDisplayBorders(svd);
        }

        // SVDisplayGroup_T_C 등 다른 그룹(Band)에 있는 SVDisplay들도 해줘야... 
    }

    public void UpdateSVDisplayBorders(SVDisplay svd)
    {
        svd.BorderBottomLeft.position = svd.transform.position - new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
        svd.BorderTopRight.position = svd.transform.position + new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
    }
    #endregion

    private void OnEnable()
    {
        EventManager.OnUpdateSV += UpdateSVDisplayGroup_T_C;
        EventManager.OnGUI_U_p_Click += UpdateImageHolder;

    }
    private void OnDisable()
    {
        EventManager.OnUpdateSV -= UpdateSVDisplayGroup_T_C;
        EventManager.OnGUI_U_p_Click -= UpdateImageHolder;
    }



    [SerializeField] Text Text_ImageHolder;
    [SerializeField] Text Text_UEvalType;

    private int index = 0;
    private void Update()
    {
        Text_ImageHolder.text = "ImageHolder = " + ImageHolderCurrent.gameObject.name;
        Text_UEvalType.text = "UEvalTypeCurrent = " + UEvalTypeCurrent.ToString();

        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdateImageHolder(God.god);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            UpdateImageHolder(hf.RealSociety[0]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            index++;

            UpdateUEvalType(UEvalTypeCurrent = (EvaluationTypes.Utility)(index % Enum.GetNames(typeof(EvaluationTypes.Utility)).Length));
        }
    }
}