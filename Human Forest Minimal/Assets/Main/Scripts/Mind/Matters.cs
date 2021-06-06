using System;
using System.Collections.Generic;

public enum Matter
{
    Test1, Test2,

    // 정서
    EmotionValence,
    // EmotionArousal,
    // Mood,

    // // 사회적 관계
    // SocialAffinity,
    // Reputation,

    // // 신체
    // Health,

    // // 부
    // Wealth,

    // // 자유
    // SenseOfLiberty,

    // // 미적감상
    // AestheticAppreciation,
} // Personal Matter도 어차피 누구한테서 영향 받았나를 기록해둘거라면 interpersonal 이랑 다를 바가 없지 않나. <= 근데 영향을 주는 게 개인이 아니라 (내가 제삼자인 경우에) 문장이기 때문에 그냥 따로 SV에 저장해두는 걸로 합시다. 

public enum RelationalMatter // 상상은 여기에 들어가지 않는다. 상상은 각 Person 속의 ImageSociety 속 ClonePerson들의 MSV 값으로.
{
    EmotionValence,
    // EmotionArousal,
    // // => 요게 PersonalMatter.SocialAffinity로.

    // DesiredRequitalEmotionValence,
    // DesiredRequitalEmotionArousal,

    // // ExpectedEmotionValence, // 이걸 상상이지만 이 Matter enum에 넣는 이유: 행복 계산에 중요할 것 같아서.
    // // ExpectedEmotionArousal,
    // // => Desired와 Expected의 곱이 PersonalMatter.SocialAffinity로.

    // // ... 가치가 뭐뭐 있을까
}

public enum EvaluativeMatter // State는 따로 없고 Value인 것들
{
    // Evaluative
    ExistentialValue, // 패배감, 자괴감, 자신감, 내가 가치 있는 사람인가. // Fulfillment와 합체. // Existence, // 저 사람의 존재 가치.
    // AestheticValue, // 저 사람의 미적 가치. 
    // => 요게 PersonalMatter.Reputation으로. 
}

public class M2S // Matter => State
{
    public Dictionary<Matter, float> Map;

    public M2S()
    {
        Map = new Dictionary<Matter, float>();

        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            Map.Add(m, 0.5f);
        }
    }
}

public class M2V // Matter => Value
{
    public Dictionary<Matter, float> Map;

    public M2V()
    {
        Map = new Dictionary<Matter, float>();

        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            Map.Add(m, 1f);
        }
    }
}

// public abstract class T2Float<T>
// {
//     public Dictionary<T, float> Map;

//     public T2Float()
//     {
//         Map = new Dictionary<T, float>();

//         foreach (T m in Enum.GetValues(typeof(T)))
//         {
//             Map.Add(m, 1f);
//         }
//     }
// }

public class R2P2S // RelationamMatter => ObjectPerson => State
{
    public Dictionary<RelationalMatter, Dictionary<Person, float>> Map;

    public R2P2S()
    {
        Map = new Dictionary<RelationalMatter, Dictionary<Person, float>>();

        foreach (RelationalMatter r in Enum.GetValues(typeof(RelationalMatter)))
        {
            Dictionary<Person, float> d = new Dictionary<Person, float>();
            Map.Add(r, d);

            foreach (Person q in SocietyManager.instance.RealSociety)
            {
                d.Add(q, 0.5f);
            }
        }
    }
}

public class R2P2V // RelationalMatter => ObjectPerson => Value
{
    public Dictionary<RelationalMatter, Dictionary<Person, float>> Map;

    public R2P2V()
    {
        Map = new Dictionary<RelationalMatter, Dictionary<Person, float>>();

        foreach (RelationalMatter r in Enum.GetValues(typeof(RelationalMatter)))
        {
            Dictionary<Person, float> d = new Dictionary<Person, float>();
            Map.Add(r, d);

            foreach (Person q in SocietyManager.instance.RealSociety)
            {
                d.Add(q, 1f);
            }
        }
    }
}

public class E2P2V // EvaluativeMatter => Person => Value
{
    public Dictionary<EvaluativeMatter, Dictionary<Person, float>> Map;

    public E2P2V()
    {
        Map = new Dictionary<EvaluativeMatter, Dictionary<Person, float>>();

        foreach (EvaluativeMatter e in Enum.GetValues(typeof(EvaluativeMatter)))
        {
            Dictionary<Person, float> d = new Dictionary<Person, float>();
            Map.Add(e, d);

            foreach (Person q in SocietyManager.instance.RealSociety)
            {
                d.Add(q, 1f);
            }
        }
    }
}

public class MSV
{
    public M2S State;
    public M2V Value;
    public R2P2S RState;
    public R2P2V RValue;
    public E2P2V EValue;

    public MSV()
    {
        State = new M2S();
        Value = new M2V();
        RState = new R2P2S();
        RValue = new R2P2V();
        EValue = new E2P2V();
    }
}

// public class StateValuePair // State & Value
// {
//     public float State;
//     public float Value;

//     // public List<BehaviorInformation> ... // 이 SV의 주인 Matter에 영향을 끼친 에피소드들이 뭐 있었는지 저장.

//     public void Add(StateValuePair deltaSV)
//     {
//         State += deltaSV.State;
//         Value += deltaSV.Value;
//     }

//     public StateValuePair() // default initialization
//     {
//         State = 0.5f;
//         Value = 1f; // 일단 Matter 개수를 모르니 1로...
//     }

//     public StateValuePair(float state, float value)
//     {
//         State = state;
//         Value = value;
//     }
// }

// public class PersonalMatterMatrix
// {
//     public Dictionary<Matter, StateValuePair> Matrix;

//     public PersonalMatterMatrix()
//     {
//         Matrix = new Dictionary<Matter, StateValuePair>();

//         foreach (Matter m in Enum.GetValues(typeof(Matter)))
//         {
//             Matrix.Add(m, new StateValuePair());
//         }
//     }
// }

// public class RelationalMatterMatrix
// {
//     public Dictionary<RelationalMatter, Dictionary<Person, StateValuePair>> Matrix; // (interpersonal matter) => ((object Person) => (state, value))

//     public RelationalMatterMatrix()
//     {
//         Matrix = new Dictionary<RelationalMatter, Dictionary<Person, StateValuePair>>();

//         foreach (RelationalMatter r in Enum.GetValues(typeof(RelationalMatter)))
//         {
//             Dictionary<Person, StateValuePair> d = new Dictionary<Person, StateValuePair>();
//             Matrix.Add(r, d);
//             foreach (Person q in SocietyManager.instance.RealSociety)
//             {
//                 d.Add(q, new StateValuePair());
//             }
//         }
//     }
// }