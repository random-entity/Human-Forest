using System.Collections.Generic;
using UnityEngine;

public class ClickableGUIManager : MonoSingleton<ClickableGUIManager>
{
    private HumanForest hf;
    private SVDisplayManager svdMan;
    [SerializeField] private ClickableGUI_U_p clickableGUI_U_P_Prefab;
    public Dictionary<Person, ClickableGUI_U_p> U_p_SetImageHolderButtons;

    private void Start()
    {
        hf = HumanForest.instance;
        svdMan = SVDisplayManager.instance;

        U_p_SetImageHolderButtons = new Dictionary<Person, ClickableGUI_U_p>();
        foreach (Person p in hf.RealSociety)
        {
            var button_p = Instantiate(clickableGUI_U_P_Prefab);
            U_p_SetImageHolderButtons.Add(p, button_p);

            button_p.TargetImageHolder = p;

            Transform svdTransform = svdMan.SVDisplayGroup_U_p[p].gameObject.transform;
            button_p.gameObject.transform.position = svdTransform.position + new Vector3(0f, -2f, 0f);
        }
    }
}