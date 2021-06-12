using System;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    HumanForest hf;
    public SVDisplayManager sVDisplayManager;

    private void Awake()
    {
        hf = HumanForest.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var pm2sv = hf.PM2SV;
            foreach (Person p in hf.RealSociety)
            {
                foreach (Matter m in Enum.GetValues(typeof(Matter)))
                {
                    pm2sv[p][m].addClamp(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                }
            }

            EventManager.InvokeOnUpdatePM2SV();
        }
    }
}