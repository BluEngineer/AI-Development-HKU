using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public int boidCount;
    
    [Range(0,300)]
    public int seperation;
    [Range(0, 200)]
    public float matchVelocity;
    [Range(0, 200)]
    public float Step;
    public float boundaryStrength;
    public Vector3 startingPosition;
    public GameObject boidPrefab;
    public List<Boid> boids;

    [Header("Box Size")]
    public Vector2 xSize = new Vector2 (0,10);
    public Vector2 ySize = new Vector2(0, 10);
    public Vector2 zSize = new Vector2(0, 10);

    // Start is called before the first frame update
    void Start()
    {      
        InitializeBoids();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBoids();
    }

    private Vector3 Rule1(Boid bj)
    {
    Vector3 centerOfMass = Vector3.zero;

        foreach (Boid b in boids)
        {
            if (b != bj) centerOfMass += b.transform.position;
        }

        centerOfMass = centerOfMass / (boidCount - 1);
        return (centerOfMass - bj.transform.position) / Step;
    }

    private Vector3 Rule2(Boid bj)
    {
    Vector3 c = Vector3.zero;

        foreach (Boid b in boids)
        {
            if (b != bj)
            {
                if ((b.transform.position - bj.transform.position).magnitude < seperation)
                {
                    c = c - (b.transform.position - bj.transform.position);
                }
            }
        }
        return c;
    }

    private Vector3 Rule3(Boid bj)
    {
        Vector3 perceivedVelocity = Vector3.zero;

        foreach (Boid b in boids)
        {
            if (b != bj)
            {
                perceivedVelocity += b.velocity;
            }
        }

        perceivedVelocity = perceivedVelocity / (boidCount - 1);
        return (perceivedVelocity - bj.velocity) / matchVelocity;
    }

    private Vector3 BoundPosition(Boid b)
    {
        //int xMin = 0, xMax = 0, yMin = 0, yMax = 0, zMin = 0, zMax = 0;
        Vector3 v = Vector3.zero;

        if (b.transform.position.x < xSize.x)
        {
            v.x = boundaryStrength;
        } else if (b.transform.position.x > xSize.y)
        {
            v.x = -boundaryStrength;
        }

        if (b.transform.position.y < ySize.x)
        {
            v.y = boundaryStrength;
        }
        else if (b.transform.position.y > ySize.y)
        {
            v.y = -boundaryStrength;
        }

        if (b.transform.position.z < zSize.x)
        {
            v.z = boundaryStrength;
        }
        else if (b.transform.position.z > zSize.y)
        {
            v.z = -boundaryStrength;
        }


        return v;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 tempOffset = new Vector3(xSize.y / 2, ySize.y / 2, zSize.y / 2);
        Gizmos.DrawWireCube(transform.position + tempOffset, new Vector3(xSize.y, ySize.y, zSize.y));
    }

    private void MoveBoids()
    {
        Vector3 v1, v2, v3, v4 = Vector3.zero;

        foreach (Boid b in boids)
        {
            //Debug.Log(b.name);
            v1 = Rule1(b);
            v2 = Rule2(b);
            v3 = Rule3(b);
            v4 = BoundPosition(b);
            b.velocity += (v1 + v2 + v3 + v4);
            b.OnUpdate();
        }
    }

    private void InitializeBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            GameObject newBoid = GameObject.Instantiate(boidPrefab, new Vector3 (Random.Range(-7,7), Random.Range(-7, 7), Random.Range(-7, 7)), Quaternion.identity);
            boids.Add(newBoid.GetComponent<Boid>());

        }
        //spawn boids off-screen
    }
}
