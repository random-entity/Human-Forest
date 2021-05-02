using System.Collections.Generic;
using UnityEngine;

public class SocietyManager : MonoSingleton<SocietyManager>
{
    public Person PersonPrefab;
    public int InitialPersonCount = 16;
    public List<Person> RealSociety = new List<Person>();

    private void Awake()
    {
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab);
            pi.tempIndex = i;
            RealSociety.Add(pi);
        }
    }
}