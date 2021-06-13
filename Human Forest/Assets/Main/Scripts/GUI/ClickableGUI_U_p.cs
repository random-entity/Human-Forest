using UnityEngine;

public class ClickableGUI_U_p : MonoBehaviour, IClickableGUI
{
    public Person TargetImageHolder;

    public ClickableGUI_U_p(Person p)
    {
        if (p.IsReal || p.IsGod)
        {
            TargetImageHolder = p;
        }
        else
        {
            TargetImageHolder = p.ImageHolder;
            Debug.LogWarning("ImageHolder is not real and is not God. Setting imageHolder = p.ImageHolder.");
        }
    }

    public void OnClicked()
    {
        if (TargetImageHolder != null)
        {
            EventManager.InvokeOnGUI_U_p_Click(TargetImageHolder);
        }
        else
        {
            Debug.Log("[ClickableGUI_U_p] this button's imageHolder == null");
        }
    }
}