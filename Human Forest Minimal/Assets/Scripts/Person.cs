using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public bool isAlive;
    public int tempIndex;

    public float Emotion, Health;
    public Dictionary<Person, float> DirectionalEmotions = new Dictionary<Person, float>();
    public Dictionary<Person, float> DirectionalExpectedEmotions = new Dictionary<Person, float>();
    public ValueSystem Values;
    public Vector2 Position;

    public float Happiness;

    private void Start()
    {
        Position = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)) * GameManager.instance.LandSize;
        foreach (Person obj in SocietyManager.instance.RealSociety)
        {
            DirectionalEmotions[obj] = 0.5f;
            DirectionalExpectedEmotions[obj] = 0.5f;
        }
    }

    private void Update()
    {
        SetTransformToPositionVector();
    }

    public (PPAction DesiredPPAction, Person obj) GetDesiredPPAction()
    {
        PPAction desire = BehaviorManager.instance.Idle;
        Person desireObject = null;
        float max = Mathf.NegativeInfinity;

        foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
        {
            foreach (Person obj in SocietyManager.instance.RealSociety)
            {
                float selfDeltaEmotion = pPAction.EstimateDeltaEmotionSub(this, obj);

                Debug.LogFormat("Checking PPAction {0}, Object {1}, selfDeltaEmotion {2}", pPAction.ToString(), obj.tempIndex, selfDeltaEmotion);

                if (max < selfDeltaEmotion)
                {
                    max = selfDeltaEmotion;
                    desire = pPAction;
                    desireObject = obj;
                }
            }
        }

        return (desire, desireObject);
    }

    public PPAction GetEthicalPPAction()
    {
        return null;
    }

    public float GetHappiness()
    {
        float happiness = 0f;

        float reputation = 0f;
        float othersEmotion = 0f;
        int aliveCount = 0;
        foreach (Person obj in SocietyManager.instance.RealSociety)
        {
            if (obj.isAlive)
            {
                aliveCount++;

                reputation += DirectionalExpectedEmotions[obj];

                othersEmotion += obj.Emotion * DirectionalEmotions[obj];
            }
        }
        reputation /= (float)aliveCount;
        othersEmotion /= (float)aliveCount;

        happiness = Values.WeightEmotion * Emotion + Values.WeightHealth * Health + Values.WeightReputation * reputation + Values.WeightKindness * othersEmotion;

        return happiness;
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}