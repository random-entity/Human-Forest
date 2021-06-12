// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public delegate void OnUpdate();

// public class EventManager : MonoBehaviour
// {
//     public static event OnUpdate OnUpdatePM2SV;

//     public static void InvokeOnUpdatePM2SV()
//     {
//         Debug.Log("invoking");
//         OnUpdatePM2SV?.Invoke();
//     }
// }