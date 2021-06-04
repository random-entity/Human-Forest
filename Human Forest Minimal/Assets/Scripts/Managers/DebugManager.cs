using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var result = SocietyManager.instance.RealSociety[0].GetDesiredAndPersonallyGoodAndEthicalPPAction();

            Debug.LogFormat("(1) Desired Action = {0}, Object = {1}\n(2) PersonallyGood Action = {2}, Object = {3}\n(3) Ethical Action = {4}, Object = {5}",
            result.Item1.Item1.tempName, result.Item1.Item2.Index,
            result.Item2.Item1.tempName, result.Item2.Item2.Index,
            result.Item3.Item1.tempName, result.Item3.Item2.Index);
        }
    }
}