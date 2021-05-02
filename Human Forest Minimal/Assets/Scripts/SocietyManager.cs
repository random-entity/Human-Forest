using System.Collections.Generic;
using UnityEngine;

public class SocietyManager : MonoSingleton<SocietyManager>
{
    public Dictionary<(Person, Person), float> DirectionalEmotions = new Dictionary<(Person, Person), float>();
    public Dictionary<(Person, Person), float> DirectionalExpectedEmotions = new Dictionary<(Person, Person), float>();
    public Person PersonPrefab;
    public Transform RealSocietyGO;
    public int InitialPersonCount = 16;
    public List<Person> RealSociety = new List<Person>();

    private void Awake()
    {
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab, RealSocietyGO);
            pi.tempIndex = i;
            RealSociety.Add(pi);
        }

        foreach (Person sub in RealSociety)
        {
            foreach (Person obj in RealSociety)
            {
                DirectionalEmotions[(sub, obj)] = 0.5f;
                DirectionalExpectedEmotions[(sub, obj)] = 0.5f;
            }
        }
        Debug.LogFormat("DirectionalEmotions Count: {0}\n DirectionalExpectedEmotions Cound: {1}", DirectionalEmotions.Count, DirectionalExpectedEmotions.Count);
        Debug.Log(DirectionalExpectedEmotions[(RealSociety[0], RealSociety[1])]);
    }
}