using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    List<Boid> LocalBoidList = new List<Boid>(); 
    Vector3 direction;
    float radius;
    float steerSpeed;
    float speed;


    void Start()
    {
        BoidManager.GetBoidManager().RegisterBoid(this);
        radius = BoidManager.GetBoidManager().radius;
        steerSpeed = BoidManager.GetBoidManager().steerSpeed;
        speed = BoidManager.GetBoidManager().speed;
    }

    void Update()
    {
        UpdateLocalList();
        
        DoAlignment();
        DoCohesion();
        DoSeperation();
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }


    void UpdateLocalList()
    {
        LocalBoidList.Clear();
        List<Boid> boids = BoidManager.GetBoidManager().boids;
        for (int i = 0; i < boids.Count; i++)
        {
            if (boids[i] == this){
                continue;
            }
            if (Vector3.Distance(transform.position, boids[i].transform.position) < BoidManager.GetBoidManager().radius)
            {
                LocalBoidList.Add(boids[i]);
            }
        }
    }

    void DoCohesion()
    {
        Vector3 avg = Vector3.zero;
        if (LocalBoidList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < LocalBoidList.Count; i++)
        {
            avg += LocalBoidList[i].transform.position;
        }
        avg = avg / LocalBoidList.Count;
        Vector3 dir = avg - transform.position;
        transform.forward += dir * steerSpeed;
    } 

    void DoAlignment()
    {
        Vector3 avg = Vector3.zero;
        if (LocalBoidList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < LocalBoidList.Count; i++)
        {
            avg += LocalBoidList[i].transform.forward;
        }
        avg = avg / LocalBoidList.Count;
        transform.forward += avg * steerSpeed;
    }

    void DoSeperation()
    {
        float sepRad = radius * 0.8f;
        List<Boid> boids = new List<Boid>();
        boids.Clear();
        for (int i = 0; i < LocalBoidList.Count; i++)
        {
            if (LocalBoidList[i] == this){
                continue;
            }
            if (Vector3.Distance(transform.position, LocalBoidList[i].transform.position) < sepRad)
            {
                boids.Add(LocalBoidList[i]);
            }
        }
        Vector3 avg = Vector3.zero;
        if (boids.Count == 0)
        {
            return;
        }
        for (int i = 0; i < boids.Count; i++)
        {
            avg += boids[i].transform.position;
        }
        avg = avg / boids.Count;
        Vector3 dir = avg - transform.position;
        transform.forward -= dir * steerSpeed;
    }
}
