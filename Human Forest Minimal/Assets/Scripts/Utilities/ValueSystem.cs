// using UnityEngine;

// [System.Serializable]
// public class ValueSystem
// {
//     public float WeightEmotion, WeightHealth, WeightReputation, WeightKindness;
//     public float Liberty;

//     public ValueSystem(bool randomize)
//     {
//         WeightEmotion = 0.25f;
//         WeightHealth = 0.25f;
//         WeightReputation = 0.25f;
//         WeightKindness = 0.25f;

//         Liberty = 0.5f;

//         if (randomize)
//         {
//             float a = Random.Range(-0.07f, 0.07f);
//             float b = Random.Range(-0.07f, 0.07f);
//             float c = Random.Range(-0.07f, 0.07f);
//             float d = -(a + b + c);

//             WeightEmotion += a;
//             WeightHealth += b;
//             WeightReputation += c;
//             WeightKindness += d;

//             Liberty += Random.Range(-0.25f, 0.25f);
//         }

//         // + 금지조항들?
//     }

//     private void normalize()
//     {
//         float sum = WeightEmotion + WeightHealth + WeightReputation + WeightKindness;
//         if (sum > 0)
//         {
//             WeightEmotion /= sum;
//             WeightHealth /= sum;
//             WeightReputation /= sum;
//             WeightKindness /= sum;
//         }
//         else
//         {
//             Debug.Log("Sum of values is <= 0. Resetting.");
//             WeightEmotion = 0.25f;
//             WeightHealth = 0.25f;
//             WeightReputation = 0.25f;
//             WeightKindness = 0.25f;
//         }
//     }
// }