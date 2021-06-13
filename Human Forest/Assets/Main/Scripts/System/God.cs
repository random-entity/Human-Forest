using System.Collections.Generic;
using UnityEngine;

public class God : MonoSingleton<God>
{
    private HumanForest hf;
    public static Person god;

    public Dictionary<Person, float> ConsiderationEthic_PureUtilitarian;

    public override void Init()
    {
        hf = HumanForest.instance;
        god = GetComponent<Person>();

        god.gameObject.name = "God";
        god.IsGod = true;

        god.gameObject.SetActive(false);

        ConsiderationEthic_PureUtilitarian = new Dictionary<Person, float>();
        foreach (Person p in hf.RealAndImagesSociety)
        {
            ConsiderationEthic_PureUtilitarian.Add(p, 1f / hf.InitialPersonCount);
        }
    }
}