using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController‎ : MonoBehaviour
{
    public Transform spawnPoint;
    public float fireRate = 0.2f;

    private float nextFireTime;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            if (Input.GetMouseButton(0) && Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
            yield return null;
        }
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(spawnPoint.position, spawnPoint.rotation);
        if (bullet != null)
        {
            // You might want to add additional behavior or components to the bullet here.
        }
    }

    private void OnValidate()
    {
        fireRate = Mathf.Max(0.0f, fireRate);
    }
}
