using System;

public class PPAction
{
    float3 sub_f_0_1_inf, obj_f_0_1_inf;

    public float2 EstimateDeltaEmotion(Person sub, Person obj, float3 f_0_1_inf)
    {
        float u = f_0_1_inf.x;
        float v = f_0_1_inf.y;
        float w = f_0_1_inf.z;

        float a = (v - w) / (u - v);
        float b = (u - w) * a;

        Func<float, float> deltaEmotion = (x) => (w + b / (x + a));

        float directedEmotionSubToObj = SocietyManager.instance.DirectionalEmotions[new PersonPair(sub, obj)];
        float deltaEmotionSub = deltaEmotion(directedEmotionSubToObj);

        float directedExpectedEmotionSubToObj = SocietyManager.instance.DirectionalExpectedEmotions[new PersonPair(sub, obj)];
        float deltaEmotionEstimatedObj = deltaEmotion(directedExpectedEmotionSubToObj);

        return new float2(deltaEmotionSub, deltaEmotionEstimatedObj);
    }
}