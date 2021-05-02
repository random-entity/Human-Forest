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

    public (List<Person>, (Dictionary<Person, Person>, Dictionary<Person, Person>)) CloneSociety()
    {
        List<Person> cloneSociety = new List<Person>();
        Dictionary<Person, Person> real2Clone = new Dictionary<Person, Person>();
        Dictionary<Person, Person> clone2Real = new Dictionary<Person, Person>();

        foreach (Person person in RealSociety)
        {
            Person clone = Instantiate(person);
            cloneSociety.Add(clone);
            real2Clone[person] = clone;
            clone2Real[clone] = person;
        }

        foreach (Person clone in cloneSociety)
        {
            Person original = clone2Real[clone];
            foreach (Person objClone in cloneSociety)
            {
                Person objOriginal = clone2Real[objClone];
                clone.DirectionalEmotions[objClone] = original.DirectionalEmotions[objOriginal];
                clone.DirectionalExpectedEmotions[objClone] = original.DirectionalExpectedEmotions[objOriginal];
            }
        }

        return (cloneSociety, (real2Clone, clone2Real));
    }
}