using System.Collections.Generic;

namespace EvaluationTypes
{
    public enum Utility
    {
        Omniscient,
        Image_OthersValuesConsiderate,
        Image_NonOthersValuesConsiderate,
        // EmotionalImpulse,
    }

    public enum TotalUtility
    {
        Equalitarian,
        Affective,
        Selfish,
    }
}

public class CFunctionPresets : MonoSingleton<CFunctionPresets>
{
    private HumanForest hf;

    public static Dictionary<Person, cloat> Equalitarian;
    public static Dictionary<Person, cloat> Affective;
    public static Dictionary<Person, cloat> Selfish;

    public override void Init()
    {
        Equalitarian = new Dictionary<Person, cloat>();
        Affective = new Dictionary<Person, cloat>();
        Selfish = new Dictionary<Person, cloat>();

        foreach (Person p in hf.RealSociety)
        {
            foreach (Person psImageOfQ in hf.P2PsImageSoc[p])
            {
                Equalitarian.Add(psImageOfQ, new cloat(1f / hf.InitialPersonCount));
                Affective.Add(psImageOfQ, hf.PQRrM2SV[p][p][psImageOfQ][Relation.EmotionValence].x);
                Selfish.Add(psImageOfQ, new cloat(psImageOfQ == p ? 1f : 0f));
            }
        }
    }
}