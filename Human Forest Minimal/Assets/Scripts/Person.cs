using UnityEngine;

public class Person : MonoBehaviour
{
    public StatusSet Stats;
    public Vector2 Position;

    public float Happiness;

    private void Awake()
    {
        Position = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)) * GameManager.instance.LandSize;

    }

    private void Update()
    {
        SetTransformToPositionVector();
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}

public class StatusSet
{
    public Status Health;
    public Status Emotion;
}

public class Status
{
    public float State;
    public float ValueWeight;
}
