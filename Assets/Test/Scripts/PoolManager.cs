using UnityEngine;
using System.Collections.Generic;

public class PoolManager<T> where T : Object
{
    private Queue<T> poolQueue = new Queue<T>();
    private List<T> activeObjects = new List<T>();
    private T prefab;
    private Transform parentContainer;

    public PoolManager(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentContainer = parent;
        ExpandPool(initialSize);
    }

    public T GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (poolQueue.Count == 0) ExpandPool(1);

        T obj = poolQueue.Dequeue();

        if (obj is GameObject gameObject)
        {
            gameObject.transform.SetPositionAndRotation(position, rotation);
            gameObject.SetActive(true);
            activeObjects.Add(obj);
        }

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        if (obj is GameObject gameObject)
        {
            gameObject.SetActive(false);
            activeObjects.Remove(obj);
            poolQueue.Enqueue(obj);
        }
    }

    public bool Contains(T obj)
    {
        return poolQueue.Contains(obj);
    }

    public List<GameObject> GetActiveObjects()
    {
        return activeObjects.ConvertAll(obj => obj as GameObject);
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T obj = Object.Instantiate(prefab, parentContainer);
            poolQueue.Enqueue(obj);
        }
    }
}
