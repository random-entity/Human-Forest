using System;
using UnityEngine;

[System.Serializable]
public class PPAction
{
    public f_0_1_inf sub_f_0_1_inf, obj_f_0_1_inf;
    Func<float, float> deltaEmotionSub, deltaEmotionObj;

    public PPAction(f_0_1_inf _sub_f_0_1_inf, f_0_1_inf _obj_f_0_1_inf)
    {
        this.sub_f_0_1_inf = _sub_f_0_1_inf;
        this.obj_f_0_1_inf = _obj_f_0_1_inf;
        this.deltaEmotionSub = GetDeltaEmotionFunc(this.sub_f_0_1_inf);
        this.deltaEmotionObj = GetDeltaEmotionFunc(this.obj_f_0_1_inf);
    }

    private Func<float, float> GetDeltaEmotionFunc(f_0_1_inf f_0_1_inf)
    {
        float u = f_0_1_inf.f_0;
        float v = f_0_1_inf.f_1;
        float w = f_0_1_inf.f_inf;

        float a = (v - w) / (u - v);
        float b = (u - w) * a;

        return (x) => (w + b / (x + a));
    }

    public float EstimateDeltaEmotionSub(Person sub, Person obj)
    {
        return deltaEmotionSub(SocietyManager.instance.DirectionalExpectedEmotions[(sub, obj)]);
    }

    public float EstimateDeltaEmotionObj(Person sub, Person obj)
    {
        return deltaEmotionObj(SocietyManager.instance.DirectionalExpectedEmotions[(sub, obj)]);
    }

    public void Execute(Person sub, Person obj)
    {
        var subToObj = (sub, obj);
        var objToSub = (obj, sub);

        float deltaEmotionSub = EstimateDeltaEmotionSub(sub, obj);

        sub.Emotion = Mathf.Clamp01(sub.Emotion + deltaEmotionSub);

        SocietyManager.instance.DirectionalEmotions[subToObj] = Mathf.Clamp01(SocietyManager.instance.DirectionalEmotions[subToObj] + deltaEmotionSub * 0.5f);


        float deltaEmotionObj = EstimateDeltaEmotionSub(obj, sub);

        obj.Emotion = Mathf.Clamp01(obj.Emotion + deltaEmotionObj);

        SocietyManager.instance.DirectionalEmotions[objToSub] = Mathf.Clamp01(SocietyManager.instance.DirectionalEmotions[objToSub] + deltaEmotionObj * 0.5f);

        SocietyManager.instance.DirectionalExpectedEmotions[subToObj] = Mathf.Clamp01((SocietyManager.instance.DirectionalExpectedEmotions[subToObj] + SocietyManager.instance.DirectionalEmotions[objToSub]) * 0.5f);

        SocietyManager.instance.DirectionalExpectedEmotions[objToSub] = Mathf.Clamp01((SocietyManager.instance.DirectionalExpectedEmotions[objToSub] + SocietyManager.instance.DirectionalEmotions[subToObj]) * 0.5f);
    }
}