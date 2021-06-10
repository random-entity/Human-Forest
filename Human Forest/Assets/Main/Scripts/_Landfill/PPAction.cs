// using System;

// [System.Serializable]
// public class PPAction
// {
//     public f_0_1_inf sub_f_0_1_inf, obj_f_0_1_inf;
//     Func<float, float> deltaEmotionSub, deltaEmotionObj;
//     public string tempName;

//     public PPAction(f_0_1_inf _sub_f_0_1_inf, f_0_1_inf _obj_f_0_1_inf, string tempName)
//     {
//         this.sub_f_0_1_inf = _sub_f_0_1_inf;
//         this.obj_f_0_1_inf = _obj_f_0_1_inf;
//         UpdateFunctions();

//         this.tempName = tempName;
//     }

//     public void UpdateFunctions()
//     {
//         this.deltaEmotionSub = GetDeltaEmotionFunc(this.sub_f_0_1_inf);
//         this.deltaEmotionObj = GetDeltaEmotionFunc(this.obj_f_0_1_inf);
//     }

//     private Func<float, float> GetDeltaEmotionFunc(f_0_1_inf f_0_1_inf)
//     {
//         float u = f_0_1_inf.f_0;
//         float v = f_0_1_inf.f_1;
//         float w = f_0_1_inf.f_inf;

//         float a = (v - w) / (u - v);
//         float b = (u - w) * a;

//         return (x) => (w + b / (x + a));
//     }
// }