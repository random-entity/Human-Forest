using UnityEngine;

public delegate void OnUpdate();
public delegate void OnPersonClick(Person clickedPerson);

public class EventManager : MonoBehaviour
{
    public static event OnUpdate OnUpdateSV;
    public static event OnPersonClick OnGUI_U_p_Click;

    public static void InvokeOnUpdateSV()
    {
        Debug.Log("EventManager.InvokeOnUpdateSV");
        OnUpdateSV?.Invoke();
    }

    public static void InvokeOnGUI_U_p_Click(Person clickedPerson)
    {
        Debug.Log("EventManager.InvokeOnGUI_U_p_Click");
        if (OnGUI_U_p_Click != null)
            OnGUI_U_p_Click(clickedPerson);
    }
}