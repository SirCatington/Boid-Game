using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float attractionRadius;
    public float attractionForce;
    Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        AttractNearbyBoids();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlockAgent collider = collision.gameObject.GetComponent<FlockAgent>();
        if (collider != null)
        {
            collider.KillSelf();
        }
    }

    void AttractNearbyBoids()
    {
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c.tag == "Boid")
            {
                FlockAgent targetAgent = c.gameObject.GetComponent<FlockAgent>();
                targetAgent.AttractTo((Vector2) transform.position, attractionForce, attractionRadius);                    
            }            
        }        
    }
}
