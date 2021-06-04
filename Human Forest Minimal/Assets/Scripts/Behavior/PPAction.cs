using System;
using UnityEngine;

[System.Serializable]
public class PPAction
{
    public f_0_1_inf sub_f_0_1_inf, obj_f_0_1_inf;
    Func<float, float> deltaEmotionSub, deltaEmotionObj;
    public string tempName;

    public PPAction(f_0_1_inf _sub_f_0_1_inf, f_0_1_inf _obj_f_0_1_inf, string tempName)
    {
        this.sub_f_0_1_inf = _sub_f_0_1_inf;
        this.obj_f_0_1_inf = _obj_f_0_1_inf;
        UpdateFunctions();

        this.tempName = tempName;
    }

    public void UpdateFunctions()
    {
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
        return deltaEmotionSub(sub.DirectionalEmotions[obj]);
    }

    public float EstimateDeltaEmotionObj(Person sub, Person obj)
    {
        return deltaEmotionObj(sub.DirectionalExpectedEmotions[obj]);
    }

    public void Execute(Person sub, Person obj)
    {
        float deltaEmotionSub = EstimateDeltaEmotionSub(sub, obj);

        sub.Emotion = Mathf.Clamp01(sub.Emotion + deltaEmotionSub);

        sub.DirectionalEmotions[obj] =
        Mathf.Clamp01(
           sub.DirectionalEmotions[obj] + deltaEmotionSub * 0.5f
        );


        float deltaEmotionObj = EstimateDeltaEmotionSub(obj, sub);

        obj.Emotion = Mathf.Clamp01(obj.Emotion + deltaEmotionObj);

        obj.DirectionalEmotions[sub] =
        Mathf.Clamp01(
            obj.DirectionalEmotions[sub] + deltaEmotionObj * 0.5f
        );


        sub.DirectionalExpectedEmotions[obj] =
        Mathf.Clamp01(
            (sub.DirectionalExpectedEmotions[obj] + obj.DirectionalEmotions[sub]) * 0.5f
        );

        obj.DirectionalExpectedEmotions[sub] =
        Mathf.Clamp01(
            (obj.DirectionalExpectedEmotions[sub] + sub.DirectionalEmotions[obj]) * 0.5f
        );
    }
}