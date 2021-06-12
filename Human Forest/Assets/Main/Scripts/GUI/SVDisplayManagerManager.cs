using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManagerManager : MonoBehaviour
{
    private static HumanForest hf;

    private void Awake()
    {
        hf = HumanForest.instance;
    }

    public static void SetSVListRefToHfPM2SV(Dictionary<Person, SVDisplay> svdGroup)
    {
        foreach (Person p in hf.RealAndImagesSociety)
        {
            SVDisplay svd = svdGroup[p];

            List<cloat2> sv = new List<cloat2>();
            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                sv.Add(hf.PM2SV[p][m]);
            }
            svd.SVList = sv;
        }
    }

    public static void SetActiveByImageHolder(Person imageHolder, Dictionary<Person, SVDisplay> svdGroup)
    {
        if (imageHolder.IsGod)
        {
            foreach (Person p in hf.RealAndImagesSociety)
            {
                svdGroup[p].gameObject.SetActive(p.IsReal);
            }
        }
        else
        {
            foreach (Person p in hf.RealAndImagesSociety)
            {
                svdGroup[p].gameObject.SetActive(p.ImageHolder == imageHolder);
            }
        }
    }

    // int i = 0;
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.G))
    //     {
    //         SetActiveByImageHolder(hf.RealSociety[i % 12], gameObject.GetComponent<SVDisplayManager>().SVDisplayGroup_U_p);
    //         i++;
    //     }
    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         SetActiveByImageHolder(God.god, gameObject.GetComponent<SVDisplayManager>().SVDisplayGroup_U_p);
    //     }
    // }
}