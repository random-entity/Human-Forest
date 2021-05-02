using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public bool isAlive;
    public int tempIndex;

    public float Emotion, Health;
    public Dictionary<Person, float> DirectionalEmotions = new Dictionary<Person, float>();
    public Dictionary<Person, float> DirectionalExpectedEmotions = new Dictionary<Person, float>();
    public ValueSystem PersonalValues;
    public Vector2 Position;

    public float Happiness;

    private void Start()
    {
        isAlive = true;
        PersonalValues = new ValueSystem();

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
        Person desireObj = null;
        float max = Mathf.NegativeInfinity;

        foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
        {
            foreach (Person obj in SocietyManager.instance.RealSociety)
            {
                if (obj != this)
                {
                    float selfDeltaEmotion = pPAction.EstimateDeltaEmotionSub(this, obj);

                    if (max < selfDeltaEmotion)
                    {
                        max = selfDeltaEmotion;
                        desire = pPAction;
                        desireObj = obj;
                    }

                    Debug.LogFormat("Subject {0} is Estimating DesiredPPAction {1} to Object {2}\nselfDeltaEmotion = {3}", this.tempIndex, pPAction.tempName, obj.tempIndex, selfDeltaEmotion);
                }
            }
        }

        return (desire, desireObj);
    }

    public (PPAction PersonallyGoodPPAction, Person obj) GetPersonallyGoodPPAction()
    {
        PPAction good = BehaviorManager.instance.Idle;
        Person goodObj = this;
        float max = Mathf.NegativeInfinity;

        foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
        {
            foreach (Person obj in SocietyManager.instance.RealSociety)
            {
                if (obj != this)
                {
                    var cloneConfig = SocietyManager.instance.CloneSociety();

                    Person cloneThis = cloneConfig.Item2.Item1[this];
                    Person cloneObj = cloneConfig.Item2.Item1[obj];

                    pPAction.Execute(cloneThis, cloneObj);

                    float sumOfHappiness = SocietyManager.instance.GetSumOfHappiness(cloneConfig.Item1);

                    if (max < sumOfHappiness)
                    {
                        max = sumOfHappiness;
                        good = pPAction;
                        goodObj = obj;
                    }

                    GameObject.Destroy(cloneConfig.Item3);

                    Debug.LogFormat("Subject {0} is Estimating PersonallyGoodPPAction {1} to Object {2}\nsumOfHappiness = {3}", this.tempIndex, pPAction.tempName, obj.tempIndex, sumOfHappiness);
                }
            }
        }

        return (good, goodObj);
    }

    public PPAction GetEthicalPPAction()
    {
        return null;
    }


    public float GetHappiness(ValueSystem valueSystem, List<Person> society)
    {
        float happiness = 0f;

        float reputation = 0f;
        float othersEmotion = 0f;
        int aliveCount = 0;
        foreach (Person obj in society)
        {
            if (obj.isAlive)
            {
                aliveCount++;

                reputation += DirectionalExpectedEmotions[obj];

                othersEmotion += obj.Emotion * DirectionalEmotions[obj];
            }
        }
        Debug.LogFormat("aliveCount {0}", aliveCount);
        reputation /= (float)aliveCount;
        othersEmotion /= (float)aliveCount;

        happiness = valueSystem.WeightEmotion * Emotion + valueSystem.WeightHealth * Health + valueSystem.WeightReputation * reputation + valueSystem.WeightKindness * othersEmotion;

        return happiness;
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}