using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoSingleton<History>
{
    public static List<Episode> history;
}

public class Episode // = Behavior Information
{

}

public interface Agent
{

}

// public class God : Agent
// {

// }

public enum Species
{
    Human,
    Cat,
}

/*

초기화만 잘 하면 할 수 있는 것: 편견 있는 상태로 시작할 것인가, 편견 없는 모든 값 중립디폴트인 상태로 시작할 것인가.

enum Species 넣는다면: 종에 따른 차별을 하는 사람들인가 아닌가.

금지조항들

*/


// public bool IsAlive;

// public int Index;

// public MSV Mind;

// public float Emotion;
// public float Health;

// public Dictionary<Person, float> DirectionalEmotions = new Dictionary<Person, float>();
// public Dictionary<Person, float> DirectionalExpectedEmotions = new Dictionary<Person, float>();

// public ValueSystem PersonalValues;

// private void Start()
// {

//     Emotion = 0.5f + UnityEngine.Random.Range(-0.25f, 0.25f);
//     Health = 0.5f + UnityEngine.Random.Range(-0.25f, 0.25f);

//     foreach (Person obj in SocietyManager.instance.RealSociety)
//     {
//         DirectionalEmotions[obj] = UnityEngine.Random.Range(0.4f, 0.6f);
//         DirectionalExpectedEmotions[obj] = UnityEngine.Random.Range(0.4f, 0.6f);
//     }

//     PersonalValues = new ValueSystem(true);
// }

// public ((PPAction DesiredPPAction, Person obj), (PPAction PersonallyGoodPPAction, Person obj), (PPAction EthicalAction, Person obj)) GetDesiredAndPersonallyGoodAndEthicalPPAction()
// {
//     PPAction desire = BehaviorManager.instance.Idle;
//     Person desireObj = this;
//     float maxDesire = Mathf.NegativeInfinity;

//     PPAction good = BehaviorManager.instance.Idle;
//     Person goodObj = this;
//     float maxGood = Mathf.NegativeInfinity;

//     PPAction ethical = BehaviorManager.instance.Idle;
//     Person ethicalObj = this;
//     float maxEthical = Mathf.NegativeInfinity;

//     foreach (PPAction pPAction in BehaviorManager.instance.PPActionList)
//     {
//         foreach (Person obj in SocietyManager.instance.RealSociety)
//         {
//             if (obj != this)
//             {
//                 float selfDeltaEmotion = pPAction.EstimateDeltaEmotionSub(this, obj);

//                 if (maxDesire < selfDeltaEmotion)
//                 {
//                     maxDesire = selfDeltaEmotion;
//                     desire = pPAction;
//                     desireObj = obj;
//                 }

//                 var cloneSocietyConfig = SocietyManager.instance.CloneSociety();

//                 Person cloneThis = cloneSocietyConfig.Item2.Real2CloneDict[this];
//                 Person cloneObj = cloneSocietyConfig.Item2.Real2CloneDict[obj];

//                 pPAction.Execute(cloneThis, cloneObj);

//                 float sumOfHappinessSubjectivePersonal = SocietyManager.instance.GetSumOfHappiness(true, true, cloneThis, cloneSocietyConfig.CloneSociety);

//                 if (maxGood < sumOfHappinessSubjectivePersonal)
//                 {
//                     maxGood = sumOfHappinessSubjectivePersonal;
//                     good = pPAction;
//                     goodObj = obj;
//                 }

//                 float sumOfHappinessObjectiveEthical = SocietyManager.instance.GetSumOfHappiness(false, false, cloneThis, cloneSocietyConfig.CloneSociety);

//                 if (maxEthical < sumOfHappinessObjectiveEthical)
//                 {
//                     maxEthical = sumOfHappinessObjectiveEthical;
//                     ethical = pPAction;
//                     ethicalObj = obj;
//                 }

//                 GameObject.Destroy(cloneSocietyConfig.CloneSocietyParentGO);

//                 Debug.LogFormat(
//                     "Subject {0} is Estimating PPAction {1} to Object {2}\nselfDeltaEmotion = {3}\nsumOfHappinessSubjectivePersonal = {4}\nsumOfHappinessObjectiveEthical = {5}",
//                     this.Index, pPAction.tempName, obj.Index, selfDeltaEmotion, sumOfHappinessSubjectivePersonal, sumOfHappinessObjectiveEthical
//                 );
//             }
//         }
//     }

//     return ((desire, desireObj), (good, goodObj), (ethical, ethicalObj));
// }