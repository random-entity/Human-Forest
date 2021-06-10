using System.Collections.Generic;
using System.Linq;

public enum Matter
{
    EmotionValence,
    Reputation,
    #region landfill
    //    Test1, Test2,
    // 정서
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
    // Personal Matter도 어차피 누구한테서 영향 받았나를 기록해둘거라면 interpersonal 이랑 다를 바가 없지 않나. <= 근데 영향을 주는 게 개인이 아니라 (내가 제삼자인 경우에) 문장이기 때문에 그냥 따로 SV에 저장해두는 걸로 합시다. 
    #endregion
}

public enum Relation
{
    EmotionValence,
    #region landfill
    // EmotionArousal,
    // // => 요게 PersonalMatter.SocialAffinity로.

    // DesiredRequitalEmotionValence,
    // DesiredRequitalEmotionArousal,

    // // ExpectedEmotionValence, // 이걸 상상이지만 이 Matter enum에 넣는 이유: 행복 계산에 중요할 것 같아서.
    // // ExpectedEmotionArousal,
    // // => Desired와 Expected의 곱이 PersonalMatter.SocialAffinity로.

    // // ... 가치가 뭐뭐 있을까

    // Evaluative
    // ExistentialValue, // 패배감, 자괴감, 자신감, 내가 가치 있는 사람인가. // Fulfillment와 합체. // Existence, // 저 사람의 존재 가치.
    // AestheticValue, // 저 사람의 미적 가치. 
    // => 요게 PersonalMatter.Reputation으로. 
    #endregion
}

public class MatterManager : MonoSingleton<MatterManager>
{
    public Dictionary<Matter, (Relation, bool outboundTrue_inboundFalse)> InfluenceMap;
    public Matter[] RelationDependentMatters;

    public override void Init()
    {
        // Matter들과 Relation들 사이의 이분할일방향그래프 그리기.
        InfluenceMap = new Dictionary<Matter, (Relation, bool outboundTrue_inboundFalse)>();
        InfluenceMap.Add(Matter.EmotionValence, (Relation.EmotionValence, true));
        InfluenceMap.Add(Matter.Reputation, (Relation.EmotionValence, false));

        RelationDependentMatters = InfluenceMap.Keys.ToArray();
    }
}