using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoSingleton<BehaviorManager>
{
    [HideInInspector] public List<PPAction> PPActionList;
    public PPAction Kill, Violence, Idle, Befriend, Love;

    private void Awake()
    {
        Kill = new PPAction(new f_0_1_inf(1f, -1f, -1.1f), new f_0_1_inf(-1f, -0.99f, 1f));
        Violence = new PPAction(new f_0_1_inf(0.5f, -1f, -4f), new f_0_1_inf(-0.75f, -0.5f, -2f));
        Idle = new PPAction(new f_0_1_inf(-0.1f, 0.1f, -4f), new f_0_1_inf(-0.1f, 0.1f, -4f));
        Befriend = new PPAction(new f_0_1_inf(-0.4f, 0.2f, 0.6f), new f_0_1_inf(-0.4f, 0.2f, 0.6f));
        Love = new PPAction(new f_0_1_inf(-1f, 1f, -2f), new f_0_1_inf(-1f, 1f, -1.3f));

        PPActionList.Add(Kill);
        PPActionList.Add(Violence);
        PPActionList.Add(Idle);
        PPActionList.Add(Befriend);
        PPActionList.Add(Love);
    }
}