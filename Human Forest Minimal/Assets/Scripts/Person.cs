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

    // public (PPAction DesiredPPAction, Person obj) GetDesiredPPAction()
    // {
    //     PPAction desire = BehaviorManager.instance.Idle;
    //     Person desireObj = null;
    //     float max = Mathf.NegativeInfinity;

    //     foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
    //     {
    //         foreach (Person obj in SocietyManager.instance.RealSociety)
    //         {
    //             if (obj != this)
    //             {
    //                 float selfDeltaEmotion = pPAction.EstimateDeltaEmotionSub(this, obj);

    //                 if (max < selfDeltaEmotion)
    //                 {
    //                     max = selfDeltaEmotion;
    //                     desire = pPAction;
    //                     desireObj = obj;
    //                 }

    //                 // Debug.LogFormat("Subject {0} is Estimating DesiredPPAction {1} to Object {2}\nselfDeltaEmotion = {3}", this.tempIndex, pPAction.tempName, obj.tempIndex, selfDeltaEmotion);
    //             }
    //         }
    //     }

    //     return (desire, desireObj);
    // }

    public ((PPAction DesiredPPAction, Person obj), (PPAction PersonallyGoodPPAction, Person obj), (PPAction EthicalAction, Person obj)) GetDesiredAndPersonallyGoodAndEthicalPPAction()
    {
        PPAction desire = BehaviorManager.instance.Idle;
        Person desireObj = this;
        float maxDesire = Mathf.NegativeInfinity;

        PPAction good = BehaviorManager.instance.Idle;
        Person goodObj = this;
        float maxGood = Mathf.NegativeInfinity;

        PPAction ethical = BehaviorManager.instance.Idle;
        Person ethicalObj = this;
        float maxEthical = Mathf.NegativeInfinity;

        foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
        {
            foreach (Person obj in SocietyManager.instance.RealSociety)
            {
                if (obj != this)
                {
                    float selfDeltaEmotion = pPAction.EstimateDeltaEmotionSub(this, obj);

                    if (maxDesire < selfDeltaEmotion)
                    {
                        maxDesire = selfDeltaEmotion;
                        desire = pPAction;
                        desireObj = obj;
                    }

                    var cloneConfig = SocietyManager.instance.CloneSociety();

                    Person cloneThis = cloneConfig.Item2.Item1[this];
                    Person cloneObj = cloneConfig.Item2.Item1[obj];

                    pPAction.Execute(cloneThis, cloneObj);

                    float sumOfHappinessSubjectivePersonal = SocietyManager.instance.GetSumOfHappiness(true, true, cloneThis, cloneConfig.Item1);

                    if (maxGood < sumOfHappinessSubjectivePersonal)
                    {
                        maxGood = sumOfHappinessSubjectivePersonal;
                        good = pPAction;
                        goodObj = obj;
                    }

                    float sumOfHappinessObjectiveEthical = SocietyManager.instance.GetSumOfHappiness(false, false, cloneThis, cloneConfig.Item1);

                    if (maxEthical < sumOfHappinessObjectiveEthical)
                    {
                        maxEthical = sumOfHappinessObjectiveEthical;
                        ethical = pPAction;
                        ethicalObj = obj;
                    }

                    GameObject.Destroy(cloneConfig.Item3);

                    // Debug.LogFormat("Subject {0} is Estimating PersonallyGoodPPAction {1} to Object {2}\nsumOfHappiness = {3}", this.tempIndex, pPAction.tempName, obj.tempIndex, sumOfHappiness);
                }
            }
        }

        return ((desire, desireObj), (good, goodObj), (ethical, ethicalObj));
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}