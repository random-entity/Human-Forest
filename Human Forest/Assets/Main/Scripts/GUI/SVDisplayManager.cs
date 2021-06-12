using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManager : MonoBehaviour
{
    private HumanForest hf;

    [SerializeField] private Transform SVDisplayPrefab;
    private float SVDisplayWidth = 1f;
    private float SVDisplayIntervalX = 1.5f;

    private void Awake()
    {
        hf = HumanForest.instance;
    }

    public Dictionary<Person, SVDisplay> SVDisplayGroup_U_p; // RealOrImagePerson p => Quad for displaying U(p)
    public Transform Band_U_p_center;

    public SVDisplay SVDisplayGroup_T_C;
    public Transform Band_T_C_center;

    private void Start()
    {
        SVDisplayGroup_U_p = new Dictionary<Person, SVDisplay>();

        int index = 0;
        foreach (Person p in hf.RealAndImagesSociety)
        {
            Transform svd_U_p_Transform = Instantiate(SVDisplayPrefab);
            svd_U_p_Transform.gameObject.name = "SVDisp_U_p(" + p.gameObject.name + ")";

            float imageHolderIndex = index % hf.InitialPersonCount;
            float normPos = imageHolderIndex - 0.5f * (hf.InitialPersonCount - 1);
            svd_U_p_Transform.position = Band_U_p_center.position + Vector3.right * (SVDisplayIntervalX * normPos);
            svd_U_p_Transform.SetParent(Band_U_p_center);

            svd_U_p_Transform.gameObject.SetActive(p.IsReal);

            SVDisplayGroup_U_p.Add(p, svd_U_p_Transform.GetComponent<SVDisplay>());
            index++;
        }

        SVDisplayManagerManager.SetSVListRefToHfPM2SV(SVDisplayGroup_U_p);

        UpdateSVDisplaySize();
    }

    #region OnUpdate SVDisplay Transform 
    // SVDisplay가 갖고 있는 SVList의 Update에 의한 변화는 SVDisplay 클래스에서 처리할 것이다.
    public void UpdateSVDisplaySize()
    {
        foreach (Person p in hf.RealAndImagesSociety)
        {
            SVDisplay svd = SVDisplayGroup_U_p[p];

            svd.BorderBottomLeft.position = svd.transform.position - new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
            svd.BorderTopRight.position = svd.transform.position + new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
        }
    }
    #endregion
}