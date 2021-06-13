using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HumanForest : MonoSingleton<HumanForest>
{
    public Person PersonPrefab;
    [SerializeField] private Transform PersonsParent;
    public int InitialPersonCount = 12;
    public List<Person> RealSociety;
    public List<string> PersonNames;

    public Dictionary<Person, Dictionary<Person, Person>> PsImageOfQs; // ImageMatrix[p][q] = RealPerson p => RealPerson q => ImagePerson p.Image(q).
    public List<Person> RealAndImagesSociety; // 초기화 때를 위해 편의상 전체에 대한 레퍼런스 남겨놓으려고.
    [SerializeField] private Material imagePersonMaterial;

    #region Utilities
    // 히힝
    private float randFloat()
    {
        return UnityEngine.Random.Range(0.1f, 0.9f);
    }

    private int randInt()
    {
        return UnityEngine.Random.Range(0, InitialPersonCount);
    }

    private List<string> getRandomNames()
    {
        string[] names = PersonalInformations.Names;

        List<string> chosenNames = new List<string>();

        while (chosenNames.Count < InitialPersonCount)
        {
            int choice = randInt();
            string name = names[choice];

            if (!chosenNames.Contains(name)) chosenNames.Add(name);
        }

        return chosenNames;
    }
    #endregion

    #region 관계망 data structuring
    /*
    ** RelationalMatter에 대한 state와 value는 ImagePerson끼리만 가질 수 있다. (물론 p.Image(p)는 자기 자신, ImagePerson이지만 RealPerson.)
    ** RealPerson p가 RealPerson q에게 갖는 RelationalMatter 그런 건 허용하지 않는다는 이야기.
    ** 그런 것을 포함한 모든 것은 RealPerson p의 ImageSociety 속에서만 일어나게끔 하는 것이 덜 복잡하다.
    */
    public Dictionary<Person, Dictionary<Matter, cloat2>> PM2SV; // RealOrImagePerson p => M2Cloat2 sv. p의 ssigma 함수.
    public Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat2>>>> PQRrM2SV; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm => sv.
    public Dictionary<Person, Dictionary<Person, cloat>> PQ2C; // RealPerson Evaluator p => (ImagePerson p.Image(q) => float consideration). p의 c 함수. // 나중에 U와 연계해서 계산하게 편하게 Q는 P의 image로 한정시키자.

    #region PsImageOfQs에 Dependent
    public Dictionary<Person, List<Person>> P2PsImageSoc;
    #endregion

    #region cloat Dictionaries Dependent on cloat2 Dictionaries for 편의
    // PM2SV의 split version
    public Dictionary<Person, Dictionary<Matter, cloat>> PM2State;
    public Dictionary<Person, Dictionary<Matter, cloat>> PM2Value;

    // p의 s와 q의 sigma를 믹스매치
    public Dictionary<Person, Dictionary<Person, Dictionary<Matter, cloat2>>> PsStateQsValue;
    // p의 sigma와 q의 s를 믹스매치 (물론 cloat2 는 x = q.state, y = p.value 순)
    public Dictionary<Person, Dictionary<Person, Dictionary<Matter, cloat2>>> QsStatePsValue;

    //// PQRrM2SV의 split version
    // public Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat>>>> PQRrM2State;
    // public Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat>>>> PQRrM2Value;
    #endregion
    #endregion

    #region Relation-Dependent Matters' State 계산

    #endregion

    #region Utility 계산
    // SSigma2Float U = (M2Float s) => (M2Float sigma) => (float u).

    public float Utility(EvaluationTypes.Utility evalType, Person evaluator, Person target)
    {
        switch (evalType)
        {
            case EvaluationTypes.Utility.Omniscient: // 이 경우 evaluator는 의미 없다.
                return Utility(target);

            case EvaluationTypes.Utility.Image_OthersValuesConsiderate:
                return Utility(evaluator, target, true);

            case EvaluationTypes.Utility.Image_NonOthersValuesConsiderate:
                return Utility(evaluator, target, false);

            default:
                Debug.LogWarning("[Utility] unknown Evaluation Type");
                return float.NaN;
        }
    }

    // 정의 대로 계산: s와 sigma가 분리되어 있을 때를 포함한 가장 일반적인 식.
    public float Utility(Dictionary<Matter, cloat> s, Dictionary<Matter, cloat> sigma)
    {
        float u = 0f;
        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            float state = s[m].f;
            float value = sigma[m].f;

            u += state * value;
        }
        return u;
    }

    // s와 sigma가 한 Dictionary의 cloat2로 묶여 있을 때.
    public float Utility(Dictionary<Matter, cloat2> m2sv)
    {
        float u = 0f;
        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            float state = m2sv[m].x.f;
            float value = m2sv[m].y.f;

            u += state * value;
        }
        return u;
    }

    public float Utility(Person evaluator, Person target, bool isConsiderateOfTargetsValues) // RealPerson evaluator의 이미지 속 ImagePerson image[target]의 Utility // p가 q의 Utility를 Image(q)의 가치관에 따라 계산 : true
    // p가 q의 Utility를 p 자신의 가치관에 따라 계산 : false
    {
        Person image_target = PsImageOfQs[evaluator][target];

        if (isConsiderateOfTargetsValues)
        {
            return Utility(PM2SV[image_target]);
        }
        else
        {
            return Utility(PsStateQsValue[image_target][evaluator]);
        }
    }

    public float Utility(Person selfEvaluator) // p가 스스로의 Utility를 계산할 때
    {
        return Utility(selfEvaluator, selfEvaluator, true); // p의 이미지 속 image[p] = (RealPerson) p니까.
    }
    #endregion

    #region Total Utility 계산
    public float TotalUtility(Dictionary<Person, cloat> c) // 실제 인간들이 갖고 있는 정확한 값을 가지고 주어진 c 함수로 계산할 때.
    {
        float t = 0f;
        foreach (Person p in RealSociety)
        {
            float u = Utility(PM2SV[p]);
            t += c[p].f * u;
        }
        return t;
    }

    public float TotalUtility(Person evaluator, Dictionary<Person, cloat> c, bool isConsiderateOfTargetsValues) // evaluator가 자신의 이미지 인간들을 윤리에서 주어진 c 함수로 계산할 때.
    {
        float t = 0f;
        foreach (Person target in RealSociety)
        {
            Person image = PsImageOfQs[evaluator][target]; // evaluator의 이미지 속 target 
            float u = Utility(evaluator, image, isConsiderateOfTargetsValues); // 

            t += c[target].f * u;
        }
        return t;
    }

    public float TotalUtility(Person evaluator, bool isConsiderateOfTargetsValues) // evaluator가 자신의 이미지 인간들을 자기가 신념으로서 가지고 있는 c 함수로 계산할 때.
    {
        return TotalUtility(evaluator, PQ2C[evaluator], isConsiderateOfTargetsValues);
    }
    #endregion

    #region Initializations
    public override void Init()
    {
        #region 이름 정하기 게임
        PersonNames = getRandomNames();
        #endregion

        #region RealSociety
        RealSociety = new List<Person>();
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab);
            RealSociety.Add(pi);

            pi.transform.SetParent(PersonsParent);
            pi.gameObject.name = "[P" + i + "]" + PersonNames[i];

            pi.SetIsRealAndImageHolder(true, pi);
        }
        #endregion

        #region PsImageOfQs, P2PsImageSoc
        PsImageOfQs = new Dictionary<Person, Dictionary<Person, Person>>();
        foreach (Person p in RealSociety)
        {
            Dictionary<Person, Person> PsImage = new Dictionary<Person, Person>();

            foreach (Person q in RealSociety)
            {
                if (q == p)
                {
                    PsImage.Add(p, p); // 혼란을 줄이기 위해 p's image of p는 p와 reference부터 똑같게 하자.
                }
                else
                {
                    Person psImageOfQ = Instantiate(q);
                    psImageOfQ.gameObject.GetComponent<MeshRenderer>().material = imagePersonMaterial;
                    psImageOfQ.gameObject.name = p.gameObject.name + "'s Image of " + q.gameObject.name;
                    PsImage.Add(q, psImageOfQ);

                    psImageOfQ.SetIsRealAndImageHolder(false, p);
                }
            }

            PsImageOfQs.Add(p, PsImage);
        }

        foreach (Person p in RealSociety)
        {
            float angle = 0f;

            foreach (Person q in RealSociety)
            {
                angle += Mathf.PI * 2f / 12f;

                if (q != p)
                {
                    Transform imageTransform = PsImageOfQs[p][q].transform;

                    imageTransform.position = p.transform.position + new Vector3(Mathf.Cos(angle), 2f, Mathf.Sin(angle));
                    imageTransform.localScale = p.transform.localScale * 0.25f;
                    imageTransform.SetParent(p.transform);
                }
            }
        }

        P2PsImageSoc = new Dictionary<Person, List<Person>>(); // Dependent on psImageOfQ
        foreach (Person p in RealSociety)
        {
            P2PsImageSoc.Add(p, PsImageOfQs[p].Values.ToList<Person>());
        }
        #endregion

        #region RealAndImagesSociety
        RealAndImagesSociety = new List<Person>();

        foreach (Person p in RealSociety)
        {
            foreach (Person q in RealSociety)
            {
                RealAndImagesSociety.Add(PsImageOfQs[p][q]);
            }
        }
        #endregion

        #region T2SV
        PM2SV = new Dictionary<Person, Dictionary<Matter, cloat2>>();
        foreach (Person p in RealAndImagesSociety)
        {
            Dictionary<Matter, cloat2> m2sv = new Dictionary<Matter, cloat2>();

            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                m2sv.Add(m, new cloat2(randFloat(), randFloat()));
            }
            PM2SV.Add(p, m2sv);
        }

        PQRrM2SV = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat2>>>>();
        foreach (Person p in RealSociety)
        {
            Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat2>>> qRrM2SV = new Dictionary<Person, Dictionary<Person, Dictionary<Relation, cloat2>>>();

            foreach (Person q in RealSociety)
            {
                Person psImageOfQ = PsImageOfQs[p][q];

                Dictionary<Person, Dictionary<Relation, cloat2>> rrM2SV = new Dictionary<Person, Dictionary<Relation, cloat2>>();

                foreach (Person r in RealSociety)
                {
                    Person psImageOfR = PsImageOfQs[p][r];

                    Dictionary<Relation, cloat2> rM2SV = new Dictionary<Relation, cloat2>();

                    foreach (Relation rm in Enum.GetValues(typeof(Relation)))
                    {
                        rM2SV.Add(rm, new cloat2(randFloat(), randFloat()));
                    }
                    rrM2SV.Add(r, rM2SV);
                }
                qRrM2SV.Add(psImageOfQ, rrM2SV);
            }
            PQRrM2SV.Add(p, qRrM2SV);
        }

        PQ2C = new Dictionary<Person, Dictionary<Person, cloat>>();
        foreach (Person p in RealSociety)
        {
            Dictionary<Person, cloat> q2c = new Dictionary<Person, cloat>();
            foreach (Person q in RealSociety)
            {
                var image_q = PsImageOfQs[p][q];

                q2c.Add(image_q, new cloat(1f));
            }
            PQ2C.Add(p, q2c);
        }
        #endregion

        #region Dependent fields : SV split, mixmatch 등
        PM2State = new Dictionary<Person, Dictionary<Matter, cloat>>();
        PM2Value = new Dictionary<Person, Dictionary<Matter, cloat>>();
        PsStateQsValue = new Dictionary<Person, Dictionary<Person, Dictionary<Matter, cloat2>>>();
        QsStatePsValue = new Dictionary<Person, Dictionary<Person, Dictionary<Matter, cloat2>>>();
        foreach (Person p in RealAndImagesSociety)
        {
            var M2State_p = DictExt.SplitDictionary<Matter>(PM2SV[p], true);
            PM2State.Add(p, M2State_p);

            var M2Value_p = DictExt.SplitDictionary<Matter>(PM2SV[p], false);
            PM2Value.Add(p, M2Value_p);

            Dictionary<Person, Dictionary<Matter, cloat2>> fixedPsStateQsValue = new Dictionary<Person, Dictionary<Matter, cloat2>>();

            Dictionary<Person, Dictionary<Matter, cloat2>> QsStatefixedPsValue = new Dictionary<Person, Dictionary<Matter, cloat2>>();

            foreach (Person q in RealAndImagesSociety)
            {
                var M2Value_q = DictExt.SplitDictionary<Matter>(PM2SV[q], false);
                fixedPsStateQsValue.Add(q, DictExt.MergeDictionary<Matter>(M2State_p, M2Value_q));

                var M2State_q = DictExt.SplitDictionary<Matter>(PM2SV[q], true);
                QsStatefixedPsValue.Add(q, DictExt.MergeDictionary<Matter>(M2State_q, M2Value_p));
            }

            PsStateQsValue.Add(p, fixedPsStateQsValue);
            QsStatePsValue.Add(p, QsStatefixedPsValue);
        }
        #endregion
    }
    #endregion
}