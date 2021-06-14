using UnityEngine;

public delegate void OnUpdate();
public delegate void OnPersonClick(Person clickedPerson);

public class EventManager : MonoBehaviour
{
    public static event OnUpdate OnUpdateSV_U_p;
    public static event OnUpdate OnUpdateSV_T_C;
    public static event OnPersonClick OnGUI_U_p_Click;

    public static void InvokeOnUpdateSV_U_p()
    {
        Debug.Log("EventManager.InvokeOnUpdateSV");
        OnUpdateSV_U_p?.Invoke();
    }

    public static void InvokeOnUpdateSV_T_C()
    {
        OnUpdateSV_T_C?.Invoke();
    }

    public static void InvokeOnGUI_U_p_Click(Person clickedPerson)
    {
        Debug.Log("EventManager.InvokeOnGUI_U_p_Click");
        if (OnGUI_U_p_Click != null)
            OnGUI_U_p_Click(clickedPerson);
    }
}