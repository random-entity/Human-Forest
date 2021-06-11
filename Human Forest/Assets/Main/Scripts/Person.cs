using UnityEngine;

public class Person : MonoBehaviour
{
    public Vector2 Position;
    public bool IsReal; // private set 하다가 stack overflow 떴으나 왜인지 몰라서 일단 public으로.
    public Person ImageHolder;

    private void Awake()
    {
        Position = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)) * EnvironmentManager.instance.LandSize;
        SetTransformToPositionVector();
    }

    public void SetIsRealAndImageHolder(bool isReal, Person imageHolder)
    {
        IsReal = isReal;
        ImageHolder = imageHolder;
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}