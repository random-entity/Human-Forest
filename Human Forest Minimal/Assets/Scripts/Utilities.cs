public struct PersonPair
{
    Person Subject, Object;

    public PersonPair(Person sub, Person obj)
    {
        this.Subject = sub;
        this.Object = obj;
    }
}

public struct float2
{
    public float x, y;

    public float2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct float3
{
    public float x, y, z;

    public float3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}