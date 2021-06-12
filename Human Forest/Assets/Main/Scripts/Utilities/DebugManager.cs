using System;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    HumanForest hf;

    private void Awake()
    {
        hf = HumanForest.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a");

            var pm2sv = hf.PM2SV;
            foreach (Person p in hf.RealSociety)
            {
                foreach (Matter m in Enum.GetValues(typeof(Matter)))
                {
                    cloat2 sv = pm2sv[p][m];
                    // Debug.Log(sv.x);
                    // Debug.Log(sv.y);

                    sv.addClamp(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                    // Debug.Log(sv.x);
                    // Debug.Log(sv.y);

                    Debug.Log("debugmanager:" + sv.x);
                }
            }

            // EventManager.OnUpdatePM2SV();
            // EventManager.InvokeOnUpdatePM2SV();
        }
    }


    // private void OnEnable()
    // {
    //     EventManager.OnUpdatePM2SV += DebugManager_OnUpdatePM2SV;
    // }
    // private void OnDisable()
    // {
    //     EventManager.OnUpdatePM2SV -= DebugManager_OnUpdatePM2SV;
    // }
    // private void DebugManager_OnUpdatePM2SV()
    // {
    //     throw new NotImplementedException();
    // }
}