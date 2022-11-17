using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public List<Boid> boids = new List<Boid>();
    static BoidManager instance;

    [SerializeField] public float radius = 3;
    [SerializeField] public float steerSpeed = 0.5f;
    [SerializeField] public float speed = 0.2f;
    [SerializeField] GameObject boidPrefab;
    [SerializeField] float spawnRadius = 20;
    [SerializeField] int spawnAmount = 10;

    void Awake(){
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static BoidManager GetBoidManager()
    {
        return instance;
    }


    public void RegisterBoid(Boid boid)
    {
        boids.Add(boid);
    }

    [ContextMenu("spawn boids")] public void SpawnBoids()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            GameObject spawnedBoid = Instantiate(boidPrefab, position, Quaternion.identity);
        }
        
    }

}
