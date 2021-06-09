using System;
using System.Collections.Generic;

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

    Dictionary<Person, Dictionary<Person, Person>> ImageMatrix; // Image[p][q] = p's image of q.
    public List<Person> RealAndImagesSociety;

    Dictionary<Person, Dictionary<Matter, float>> PM2S;
    Dictionary<Person, Dictionary<Matter, float>> PM2V;
    Dictionary<Person, float> P2C;

    public float Utility(Person p)
    {
        float u = 0f;
        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            float s = PM2S[p][m];
            float v = PM2V[p][m];

            u += s * v;
        }
        return u;
    }

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

        ImageMatrix = new Dictionary<Person, Dictionary<Person, Person>>();

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
                    Person PsImageOfQ = Instantiate(q);
                    PsImage.Add(q, PsImageOfQ);
                }
            }

            ImageMatrix.Add(p, PsImage);
        }

        RealAndImagesSociety = new List<Person>();

        foreach (Person p in RealSociety)
        {
            foreach (Person q in RealSociety)
            {
                RealAndImagesSociety.Add(ImageMatrix[p][q]);
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
    }
}