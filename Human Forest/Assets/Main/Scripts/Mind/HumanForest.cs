using System;
using System.Collections.Generic;

public class History : MonoSingleton<History>
{
    public static List<Episode> history;
}

public class Episode
{

}

public interface Agent
{

}

public class God : Agent
{

}

public class HumanForest : MonoSingleton<HumanForest>
{
    public Person PersonPrefab;
    public int InitialPersonCount = 12;
    public List<Person> RealSociety;

    Dictionary<Person, Dictionary<Person, Person>> PsImageOfQs; // ImageMatrix[p][q] = RealPerson p => RealPerson q => ImagePerson p.Im(q).
    public List<Person> RealAndImagesSociety; // 초기화 때를 위해 편의상 전체에 대한 레퍼런스 남겨놓으려고.

    // RelationalMatter에 대한 state와 value는 ImagePerson끼리만 가질 수 있다. (물론 p.Image(p)는 자기 자신, ImagePerson이지만 RealPerson.)
    Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>> PQRrM2State; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm의 state.
    Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>> PQRrM2Value; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm의 value.

    Dictionary<Person, Dictionary<Matter, float>> PM2State; // RealOrImagePerson p => M2Float (Matter m => float state). p의 s 함수.
    Dictionary<Person, Dictionary<Matter, float>> PM2Value; // RealOrImagePerson p => M2Float (Matter m => float state). p의 sigma 함수.

    Dictionary<Person, Dictionary<Person, float>> PQ2C; // RealPerson p => (RealPerson q => float consideration). p의 c 함수.

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

    public float Utility(Person evaluator, Person target) // RealPerson evaluator의 ImagePerson image[target]의 Utility
    {
        Person image = PsImageOfQs[evaluator][target];
        return Utility(PM2State[image], PM2Value[image]);
    }

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
                    PsImage.Add(q, psImageOfQ);
                }
            }

            PsImageOfQs.Add(p, PsImage);
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

        PQRrM2State = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>>();
        PQRrM2Value = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>>();

        foreach (Person p in RealSociety)
        {
            Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>> qRrM2State = new Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>();
            Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>> qRrM2Value = new Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>();

            foreach (Person q in RealSociety)
            {
                Person psImageOfQ = PsImageOfQs[p][q];
                Dictionary<Person, Dictionary<RelationalMatter, float>> rrM2State = new Dictionary<Person, Dictionary<RelationalMatter, float>>();
                Dictionary<Person, Dictionary<RelationalMatter, float>> rrM2Value = new Dictionary<Person, Dictionary<RelationalMatter, float>>();

                foreach (Person r in RealSociety)
                {
                    Person psImageOfR = PsImageOfQs[p][r];
                    Dictionary<RelationalMatter, float> rM2State = new Dictionary<RelationalMatter, float>();
                    Dictionary<RelationalMatter, float> rM2Value = new Dictionary<RelationalMatter, float>();

                    foreach (RelationalMatter rm in Enum.GetValues(typeof(RelationalMatter)))
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
}