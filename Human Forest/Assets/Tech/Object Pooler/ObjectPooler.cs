using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    [System.Serializable]
    public class ObjectPool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<ObjectPool> PoolList;

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    [SerializeField] private Transform initialParent;

    public override void Init()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ObjectPool pool in PoolList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, initialParent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 spawnPosition, Transform parent, Color color, float mass)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("[ObjectPooler] Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.GetComponent<SpriteRenderer>().color = color;
        objectToSpawn.GetComponent<Rigidbody2D>().mass = mass;
        objectToSpawn.transform.position = spawnPosition; // ObjectPooler에서 위치는 기본적으로 해줄게! 당연히 그래야지~
        objectToSpawn.transform.SetParent(parent);

        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void DeactivateAll(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("[ObjectPooler] Pool with tag " + tag + " doesn't exist");
        }
        else
        {
            Queue<GameObject> queue = PoolDictionary[tag];
            for (int i = 0; i < queue.Count; i++)
            {
                GameObject obj = queue.Dequeue();

                if (obj.activeInHierarchy) obj.SetActive(false);

                queue.Enqueue(obj);
            }
        }
    }
}