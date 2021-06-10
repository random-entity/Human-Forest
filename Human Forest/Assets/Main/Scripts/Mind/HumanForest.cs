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

public class RealPerson : Person
{

}

public class ImagePerson : Person
{

}

public class HumanForest : MonoSingleton<HumanForest>
{
    public Person PersonPrefab;
    public int InitialPersonCount = 12;
    public List<Person> RealSociety;

    Dictionary<Person, Dictionary<Person, Person>> PsImageOfQs; // ImageMatrix[p][q] = (real person) p's image of (real person) q.
    public List<Person> RealAndImagesSociety; // 전체에 대한 레퍼런스 초기화 때 편하게 편의 상 남겨놓으려고.

    Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>> PQRrM2S; // PQRrM2S[p][q][r][rm] = p.Image(q)가 p.Image(r)에게 갖는 rm의 state.
    Dictionary<Person, Dictionary<Matter, float>> PM2S; // p => (m => float), p는 real 혹은 image Person. p의 s 함수.
    Dictionary<Person, Dictionary<Matter, float>> PM2V; // p => (m => float), p는 real 혹은 image Person. p의 sigma 함수.
    // Dictionary<Person, float> P2C;

    public float Utility(Dictionary<Matter, float> s, Dictionary<Matter, float> sigma)
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

    public float Utility(Person )

    public float TotalUtility(List<Person> society, Dictionary<Person, float> c)
    {
        float t = 0f;
        foreach (Person p in society)
        {
            float u = Utility(p);
            t += c[p] * u;
        }
        return t;
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

        PM2S = new Dictionary<Person, Dictionary<Matter, float>>();
        PM2V = new Dictionary<Person, Dictionary<Matter, float>>();
        foreach (Person p in RealAndImagesSociety)
        {
            Dictionary<Matter, float> m2s = new Dictionary<Matter, float>();
            Dictionary<Matter, float> m2v = new Dictionary<Matter, float>();

            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                m2s.Add(m, 0.5f);
                m2v.Add(m, 1f);
            }

            PM2S.Add(p, m2s);
            PM2V.Add(p, m2v);
        }

        PQRrM2S = new Dictionary<Person, Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>>();
        foreach (Person p in RealSociety)
        {
            Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>> qRrM2S = new Dictionary<Person, Dictionary<Person, Dictionary<RelationalMatter, float>>>();

            foreach (Person q in RealSociety)
            {
                Person psImageOfQ = PsImageOfQs[p][q];


                foreach (Person r in RealSociety)
                {

                }
            }
        }
    }
}