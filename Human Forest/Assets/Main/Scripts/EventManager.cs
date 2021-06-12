using UnityEngine;

public delegate void OnUpdate();

public class EventManager : MonoBehaviour
{
    public static event OnUpdate OnUpdatePM2SV;

    public static void InvokeOnUpdatePM2SV() // 이 함수를 불러주신다면, OnUpdatePM2SV를 listen하고 있는 subscribers님들이 subscribe할 당시에 [공지 받았을 때 부르기로 한 메소드들]을 전부 불러드리겠습니다.
    {
        OnUpdatePM2SV?.Invoke();
    }
}