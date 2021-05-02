using System.Collections.Generic;

public class SocietyManager : MonoSingleton<SocietyManager>
{
    public Dictionary<PersonPair, float> DirectionalEmotions;
    public Dictionary<PersonPair, float> DirectionalExpectedEmotions;
    public Person PersonPrefab;
    public int InitialPersonCount = 16;
    public List<Person> RealSociety = new List<Person>();

    private void Awake()
    {
        for (int i = 0; i < InitialPersonCount; i++)
        {
            Person pi = Instantiate(PersonPrefab);
            RealSociety.Add(pi);
        }
    }
}