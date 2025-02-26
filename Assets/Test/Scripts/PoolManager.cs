using UnityEngine;
using System.Collections.Generic;

public class PoolManager<T> where T : MonoBehaviour
{
    private Queue<T> poolQueue = new Queue<T>();
    private T prefab;
    private Transform parentContainer;

    public PoolManager(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentContainer = parent;

        ExpandPool(initialSize); // Ensure pool is populated at the start
    }

    public T GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (poolQueue.Count == 0)
        {
            ExpandPool(1); // Expand when needed
        }

        T obj = poolQueue.Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T obj = GameObject.Instantiate(prefab, parentContainer);
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public bool Contains(T obj)
    {
        return poolQueue.Contains(obj);
    }
}
