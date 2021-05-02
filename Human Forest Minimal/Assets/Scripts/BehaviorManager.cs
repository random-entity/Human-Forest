using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoSingleton<BehaviorManager>
{
    [HideInInspector] public List<PPAction> PPActionList;
    public PPAction Kill, Violence, Idle, Befriend, Love;

    private void Awake()
    {
        PPActionList.Add(Kill);
        PPActionList.Add(Violence);
        PPActionList.Add(Idle);
        PPActionList.Add(Befriend);
        PPActionList.Add(Love);
    }
}