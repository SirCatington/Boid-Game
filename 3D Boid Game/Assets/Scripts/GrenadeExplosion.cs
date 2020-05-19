using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public float attractionRadius;
    public float attractionForce;
    public float startingStrength;

    Transform transform;

    public float decayRate;
    public float decay;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        SetFromStrength((int)startingStrength);
    }

    // Update is called once per frame
    void Update()
    {
        AttractNearbyBoids();

        if (decay < 1)
        {
            decay += decayRate * Time.deltaTime;
            attractionRadius = Mathf.Abs(startingStrength) * 3 * (1 - decay);
            attractionForce = startingStrength * (1 - decay);
            transform.localScale = Vector3.one * Mathf.Abs(startingStrength) * (1 - decay);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetFromStrength(int strength)
    {
        if (strength > 0)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(0 / 255f, 125 / 255f, 255 / 255f));  
        }
        else if (strength < 0)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(255 / 255f, 125 / 255f, 0 / 255f));
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
                targetAgent.AttractTo(transform.position, attractionForce, attractionRadius);
            }
        }
    }
}
