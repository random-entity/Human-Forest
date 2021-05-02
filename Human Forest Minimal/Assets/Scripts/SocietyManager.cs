using System.Collections.Generic;

public class SocietyManager : MonoSingleton<SocietyManager>
{
    public Dictionary<PersonPair, float> DirectionalEmotions;
    public Dictionary<PersonPair, float> DirectionalExpectedEmotions;
    public Person PersonPrefab;
    public int InitialPersonCount = 16;

    private void Awake()
    {
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Instantiate(PersonPrefab);
        }
    }
}