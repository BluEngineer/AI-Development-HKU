using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public float rotationSpeed;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnUpdate()
    {
        transform.position += velocity.normalized * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), rotationSpeed * Time.deltaTime);
    }
}
