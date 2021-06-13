using UnityEngine;

public delegate void OnUpdate();
public delegate void OnPersonClick(Person clickedPerson);

public class EventManager : MonoBehaviour
{
    public static event OnUpdate OnUpdatePM2SV;
    public static event OnUpdate OnUpdateSVListRef;
    public static event OnPersonClick OnGUI_U_p_Click;

    public static void InvokeOnUpdatePM2SV()
    {
        OnUpdatePM2SV();
    }

    public static void InvokeOnUpdateSVListRef()
    {
        OnUpdateSVListRef?.Invoke();
    }

    public static void InvokeOnGUI_U_p_Click(Person clickedPerson)
    {
        if (OnGUI_U_p_Click != null)
            OnGUI_U_p_Click(clickedPerson);
    }
}