using UnityEngine;
using System.Collections;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public GameObject bulletPrefab;
    public int poolSize = 10;

    private GameObject[] pool;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(bulletPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].transform.rotation = rotation;
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        return null;
    }
}