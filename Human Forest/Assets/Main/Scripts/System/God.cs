using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    private HumanForest hf;
    public static Person god;

    public Dictionary<Person, float> ConsiderationEthic_PureUtilitarian;

    [SerializeField] private Person personPrefab;
    private void Awake()
    {
        hf = HumanForest.instance;
        
        god = Instantiate(personPrefab);
        god.gameObject.name = "God";
        god.gameObject.SetActive(false);
        god.IsGod = true;

        ConsiderationEthic_PureUtilitarian = new Dictionary<Person, float>();
        foreach (Person p in hf.RealAndImagesSociety)
        {
            ConsiderationEthic_PureUtilitarian.Add(p, 1f / hf.InitialPersonCount);
        }
    }
}