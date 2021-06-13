using System.Collections.Generic;
using System.Linq;

public class MatterGraph : MonoSingleton<MatterGraph>
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