using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocietyManager : MonoSingleton<SocietyManager>
{
    public ValueSystem Ethics;

    public Person PersonPrefab;
    public int InitialPersonCount = 16;
    public List<Person> RealSociety = new List<Person>();

    private void Awake()
    {
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab);
            pi.Index = i;
            RealSociety.Add(pi);
            pi.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(true);
        }
    }

    public float GetHappiness(Person p, ValueSystem valueSystem, List<Person> society)
    {
        float happiness = 0f;

        float reputation = 0f;
        float othersEmotion = 0f;
        
        int aliveCount = 0;

        foreach (Person obj in society)
        {
            if (obj.IsAlive)
            {
                aliveCount++;

                reputation += p.DirectionalExpectedEmotions[obj];

                othersEmotion += obj.Emotion * p.DirectionalEmotions[obj];
            }
        }
        reputation /= (float)aliveCount;
        othersEmotion /= (float)aliveCount;

        happiness = valueSystem.WeightEmotion * p.Emotion + valueSystem.WeightHealth * p.Health + valueSystem.WeightReputation * reputation + valueSystem.WeightKindness * othersEmotion;

        return happiness;
    }

    public float GetSumOfHappiness(bool subjectiveTrueObjectiveFalse, bool personalTrueSocialFalse, Person whoIsComputingIfSubjective, List<Person> society)
    {
        float sum = 0f;

        if (subjectiveTrueObjectiveFalse)
        {
            if (personalTrueSocialFalse)
            {
                foreach (Person p in society)
                {
                    sum += GetHappiness(p, whoIsComputingIfSubjective.PersonalValues, society);
                }
            }
            else
            {
                // whoIsComputingIfSubjective의 심상 속 p에게 SocietyManager.instance.Ethics 적용한 거
            }
        }
        else
        {
            if (personalTrueSocialFalse)
            {
                foreach (Person p in society)
                {
                    sum += GetHappiness(p, p.PersonalValues, society);
                }
            }
            else
            {
                foreach (Person p in society)
                {
                    sum += GetHappiness(p, SocietyManager.instance.Ethics, society);
                }
            }
        }

        return sum;
    }

    public (List<Person> CloneSociety, (Dictionary<Person, Person> Real2CloneDict, Dictionary<Person, Person> Clone2RealDict), GameObject CloneSocietyParentGO) CloneSociety()
    {
        List<Person> cloneSociety = new List<Person>();
        Dictionary<Person, Person> real2Clone = new Dictionary<Person, Person>();
        Dictionary<Person, Person> clone2Real = new Dictionary<Person, Person>();

        var cloneSocietyParentGO = new GameObject("CloneSociety");

        foreach (Person person in RealSociety)
        {
            Person clone = Instantiate(person, cloneSocietyParentGO.transform);
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

        return (cloneSociety, (real2Clone, clone2Real), cloneSocietyParentGO);
    }
}