using UnityEngine;

public class God : MonoSingleton<God>
{
    public static Person god;

    public override void Init()
    {
        god = GetComponent<Person>();
        if (god == null) Debug.LogWarning("[God] god == null");

        god.gameObject.name = "God";
        god.IsGod = true;

        god.gameObject.SetActive(false);
    }
}