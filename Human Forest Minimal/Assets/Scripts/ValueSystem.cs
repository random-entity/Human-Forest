using UnityEngine;

[System.Serializable]
public class ValueSystem
{
    public float WeightEmotion, WeightHealth, WeightReputation, WeightKindness;
    public float Liberty;

    public ValueSystem()
    {
        WeightEmotion = 0.25f;
        WeightHealth = 0.25f;
        WeightReputation = 0.25f;
        WeightKindness = 0.25f;
        Liberty = 0.5f;

        // + 금지조항들?
    }

    private void normalize()
    {
        float sum = WeightEmotion + WeightHealth + WeightReputation + WeightKindness;
        if (sum > 0)
        {
            WeightEmotion /= sum;
            WeightHealth /= sum;
            WeightReputation /= sum;
            WeightKindness /= sum;
        }
        else
        {
            Debug.Log("Sum of values is <= 0. Resetting.");
            WeightEmotion = 0.25f;
            WeightHealth = 0.25f;
            WeightReputation = 0.25f;
            WeightKindness = 0.25f;
        }
    }
}