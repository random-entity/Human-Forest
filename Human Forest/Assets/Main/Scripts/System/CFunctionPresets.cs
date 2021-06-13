using System.Collections.Generic;

public class CFunctionPresets : MonoSingleton<CFunctionPresets>
{
    private HumanForest hf;

    public static Dictionary<Person, cloat> Equalitarian;
    public static Dictionary<Person, cloat> Affective;
    public static Dictionary<Person, cloat> Selfish;

    public static Dictionary<EvaluationTypes.TotalUtility, Dictionary<Person, cloat>> GetCPreset;

    public override void Init()
    {
        hf = HumanForest.instance;

        Equalitarian = new Dictionary<Person, cloat>();
        Affective = new Dictionary<Person, cloat>();
        Selfish = new Dictionary<Person, cloat>();

        foreach (Person p in hf.RealSociety)
        {
            foreach (Person q in hf.RealSociety)
            {
                var psImageOfQ = hf.PsImageOfQs[p][q];

                Equalitarian.Add(psImageOfQ, new cloat(1f / hf.InitialPersonCount));
                Affective.Add(psImageOfQ, hf.PQRrM2SV[p][p][q][Relation.EmotionValence].x);
                Selfish.Add(psImageOfQ, new cloat(psImageOfQ == p ? 1f : 0f));
            }
        }

        GetCPreset = new Dictionary<EvaluationTypes.TotalUtility, Dictionary<Person, cloat>>();
        GetCPreset.Add(EvaluationTypes.TotalUtility.Equalitarian, Equalitarian);
        GetCPreset.Add(EvaluationTypes.TotalUtility.Affective, Affective);
        GetCPreset.Add(EvaluationTypes.TotalUtility.Selfish, Selfish);
    }
}