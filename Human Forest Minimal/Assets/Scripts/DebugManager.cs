using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            (PPAction, Person) resultDesired = SocietyManager.instance.RealSociety[0].GetDesiredPPAction();
            Debug.LogFormat("Desired Action = {0}, Object = {1}", resultDesired.Item1.tempName, resultDesired.Item2.tempIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            (PPAction, Person) resultGood = SocietyManager.instance.RealSociety[0].GetPersonallyGoodPPAction();
            Debug.LogFormat("Personally Good Action = {0}, Object = {1}", resultGood.Item1.tempName, resultGood.Item2.tempIndex);
        }
    }
}