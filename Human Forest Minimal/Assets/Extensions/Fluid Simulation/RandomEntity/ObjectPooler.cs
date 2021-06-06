using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    public override void Init()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 spawnPosition)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPosition; // ObjectPooler에서 위치는 기본적으로 해줄게! 당연히 그래야지~

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}