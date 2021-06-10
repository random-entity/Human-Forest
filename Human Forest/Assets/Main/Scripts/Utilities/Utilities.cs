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
public struct sloat2
{
    public float x, y;

    public sloat2(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

[System.Serializable]
public class cloat2
{
    public float x, y;

    public cloat2(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
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