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

        Emotion = 0.5f + Random.Range(-0.25f, 0.25f);
        Health = 0.5f + Random.Range(-0.25f, 0.25f);
        foreach (Person obj in SocietyManager.instance.RealSociety)
        {
            DirectionalEmotions[obj] = Random.Range(0.4f, 0.6f);
            DirectionalExpectedEmotions[obj] = Random.Range(0.4f, 0.6f);
        }

        PersonalValues = new ValueSystem(true);

        Position = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)) * GameManager.instance.LandSize;
        SetTransformToPositionVector();
    }

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

                    var cloneSocietyConfig = SocietyManager.instance.CloneSociety();

                    Person cloneThis = cloneSocietyConfig.Item2.Real2CloneDict[this];
                    Person cloneObj = cloneSocietyConfig.Item2.Real2CloneDict[obj];

                    pPAction.Execute(cloneThis, cloneObj);

                    float sumOfHappinessSubjectivePersonal = SocietyManager.instance.GetSumOfHappiness(true, true, cloneThis, cloneSocietyConfig.CloneSociety);

                    if (maxGood < sumOfHappinessSubjectivePersonal)
                    {
                        maxGood = sumOfHappinessSubjectivePersonal;
                        good = pPAction;
                        goodObj = obj;
                    }

                    float sumOfHappinessObjectiveEthical = SocietyManager.instance.GetSumOfHappiness(false, false, cloneThis, cloneSocietyConfig.CloneSociety);

                    if (maxEthical < sumOfHappinessObjectiveEthical)
                    {
                        maxEthical = sumOfHappinessObjectiveEthical;
                        ethical = pPAction;
                        ethicalObj = obj;
                    }

                    GameObject.Destroy(cloneSocietyConfig.CloneSocietyParentGO);

                    Debug.LogFormat(
                        "Subject {0} is Estimating PPAction {1} to Object {2}\nselfDeltaEmotion = {3}\nsumOfHappinessSubjectivePersonal = {4}\nsumOfHappinessObjectiveEthical = {5}",
                        this.tempIndex, pPAction.tempName, obj.tempIndex, selfDeltaEmotion, sumOfHappinessSubjectivePersonal, sumOfHappinessObjectiveEthical
                    );
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