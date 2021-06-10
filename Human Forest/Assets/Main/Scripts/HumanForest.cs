using System;
using System.Collections.Generic;
using UnityEngine;

public class HumanForest : MonoSingleton<HumanForest>
{
    public Person PersonPrefab;
    public int InitialPersonCount = 12;
    public List<Person> RealSociety;

    Dictionary<Person, Dictionary<Person, Person>> PsImageOfQs; // ImageMatrix[p][q] = RealPerson p => RealPerson q => ImagePerson p.Image(q).
    public List<Person> RealAndImagesSociety; // 초기화 때를 위해 편의상 전체에 대한 레퍼런스 남겨놓으려고.
    [SerializeField] private Material imagePersonMaterial;

    /*
    ** RelationalMatter에 대한 state와 value는 ImagePerson끼리만 가질 수 있다. (물론 p.Image(p)는 자기 자신, ImagePerson이지만 RealPerson.)
    ** RealPerson p가 RealPerson q에게 갖는 RelationalMatter 그런 건 허용하지 않는다는 이야기.
    ** 그런 것을 포함한 모든 것은 RealPerson p의 ImageSociety 속에서만 일어나게끔 하는 것이 덜 복잡하다.
    */
    Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>> PQRrM2State; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm의 state.
    Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>> PQRrM2Value; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm의 value.

    Dictionary<Person, Dictionary<Matter, float>> PM2State; // RealOrImagePerson p => M2Float (Matter m => float state). p의 s 함수.
    Dictionary<Person, Dictionary<Matter, float>> PM2Value; // RealOrImagePerson p => M2Float (Matter m => float state). p의 sigma 함수.

    Dictionary<Person, Dictionary<Person, float>> PQ2C; // RealPerson p => (RealPerson q => float consideration). p의 c 함수.

    #region Relation-Dependent Matters' State 계산
    

    #endregion

    #region Utility 계산
    public float Utility(Dictionary<Matter, float> s, Dictionary<Matter, float> sigma) // SSigma2Float U = (M2Float s) => (M2Float sigma) => (float u).
    {
        float u = 0f;
        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            float state = s[m];
            float value = sigma[m];

            u += state * value;
        }
        return u;
    }

    public float Utility(Person evaluator, Person target) // RealPerson evaluator의 이미지 속 ImagePerson image[target]의 Utility
    {
        Person image = PsImageOfQs[evaluator][target];
        return Utility(PM2State[image], PM2Value[image]);
    }

    public float Utility(Person selfEvaluator) // p가 스스로의 Utility를 계산할 때
    {
        return Utility(selfEvaluator, selfEvaluator); // p의 이미지 속 image[p] = (RealPerson) p니까.
    }
    #endregion

    #region Total Utility 계산
    public float TotalUtility(Dictionary<Person, float> c) // 실제 인간들이 갖고 있는 정확한 값을 가지고 주어진 c 함수로 계산할 때.
    {
        float t = 0f;
        foreach (Person p in RealSociety)
        {
            float u = Utility(PM2State[p], PM2Value[p]);
            t += c[p] * u;
        }
        return t;
    }

    public float TotalUtility(Person evaluator, Dictionary<Person, float> c) // evaluator가 자신의 이미지 인간들을 주어진 c 함수로 계산할 때.
    {
        float t = 0f;
        foreach (Person target in RealSociety)
        {
            Person image = PsImageOfQs[evaluator][target]; // evaluator의 이미지 속 target 
            float u = Utility(evaluator, image);

            t += c[target] * u;
        }
        return t;
    }

    public float TotalUtility(Person evaluator) // evaluator가 자신의 이미지 인간들을 자기가 신념으로서 가지고 있는 c 함수로 계산할 때.
    {
        return TotalUtility(evaluator, PQ2C[evaluator]);
    }
    #endregion

    #region initialization
    public override void Init()
    {
        RealSociety = new List<Person>();

        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab);
            RealSociety.Add(pi);
        }

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
                    PsImage.Add(q, psImageOfQ);
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

        RealAndImagesSociety = new List<Person>();

        foreach (Person p in RealSociety)
        {
            foreach (Person q in RealSociety)
            {
                RealAndImagesSociety.Add(PsImageOfQs[p][q]);
            }
        }

        PM2State = new Dictionary<Person, Dictionary<Matter, float>>();
        PM2Value = new Dictionary<Person, Dictionary<Matter, float>>();
        foreach (Person p in RealAndImagesSociety)
        {
            Dictionary<Matter, float> m2s = new Dictionary<Matter, float>();
            Dictionary<Matter, float> m2v = new Dictionary<Matter, float>();

            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                m2s.Add(m, 0.5f);
                m2v.Add(m, 1f);
            }

            PM2State.Add(p, m2s);
            PM2Value.Add(p, m2v);
        }

        PQRrM2State = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>>();
        PQRrM2Value = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>>();

        foreach (Person p in RealSociety)
        {
            Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>> qRrM2State = new Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>();
            Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>> qRrM2Value = new Dictionary<Person, Dictionary<Person, Dictionary<Relation, float>>>();

            foreach (Person q in RealSociety)
            {
                Person psImageOfQ = PsImageOfQs[p][q];
                Dictionary<Person, Dictionary<Relation, float>> rrM2State = new Dictionary<Person, Dictionary<Relation, float>>();
                Dictionary<Person, Dictionary<Relation, float>> rrM2Value = new Dictionary<Person, Dictionary<Relation, float>>();

                foreach (Person r in RealSociety)
                {
                    Person psImageOfR = PsImageOfQs[p][r];
                    Dictionary<Relation, float> rM2State = new Dictionary<Relation, float>();
                    Dictionary<Relation, float> rM2Value = new Dictionary<Relation, float>();

                    foreach (Relation rm in Enum.GetValues(typeof(Relation)))
                    {
                        rM2State.Add(rm, 0.5f);
                        rM2Value.Add(rm, 1f);
                    }

                    rrM2State.Add(r, rM2State);
                    rrM2Value.Add(r, rM2State);
                }

                qRrM2State.Add(psImageOfQ, rrM2State);
                qRrM2Value.Add(psImageOfQ, rrM2State);
            }

            PQRrM2State.Add(p, qRrM2State);
            PQRrM2Value.Add(p, qRrM2State);
        }
    }
    #endregion
}