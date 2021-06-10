using UnityEngine;

public class Person : MonoBehaviour
{
    public Vector2 Position;

    private void Awake()
    {
        Position = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)) * EnvironmentManager.instance.LandSize;
        SetTransformToPositionVector();
    }

    private void SetTransformToPositionVector()
    {
        transform.position = new Vector3(Position.x, 1, Position.y);
    }
}