public struct PersonPair
{
    Person Subject, Object;

    public PersonPair(Person sub, Person obj)
    {
        this.Subject = sub;
        this.Object = obj;
    }
}

[System.Serializable]
public struct float2
{
    public float x, y;

    public float2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]
public class f_0_1_inf
{
    public float f_0, f_1, f_inf;

    public f_0_1_inf(float f_0, float f_1, float f_inf)
    {
        this.f_0 = f_0;
        this.f_1 = f_1;
        this.f_inf = f_inf;
    }
}