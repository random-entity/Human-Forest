using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    [SerializeField] private List<PPAction> PPActionList;
    public PPAction kill, violence, idle, befriend, love;

    private void Awake()
    {
        PPActionList.Add(kill);
        PPActionList.Add(violence);
        PPActionList.Add(idle);
        PPActionList.Add(befriend);
        PPActionList.Add(love);
    }
}
