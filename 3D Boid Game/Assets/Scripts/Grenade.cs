using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Transform transform;
    public GrenadeExplosion explosionPrefab;
    public float strength;

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (transform.position.z < 0)
        {
            GrenadeExplosion explosion = Instantiate(explosionPrefab, transform.position + Vector3.forward * 2, Quaternion.identity);
            explosion.startingStrength = strength;
            Destroy(gameObject);
        }
    }
    //public static Vector3 Parabola(Vector3 start, Vector3 end, Vector2 direction, float height, float t)
    //{
    //    //start = (0, 0, 0) end = (-5, -5, 0)

    //    // Vertex of function is (h, k)
    //    float h = ((start.x + end.x) / 2); // -2.5
    //    float k = Mathf.Max(start.z, end.z) + height; // 5
    //    float a = -k / Mathf.Pow(start.x - h, 2); // -0.8
    //    float z = a * Mathf.Pow(t - h, 2) + k;                 //a(x-h)^2 + k = z  -0.8(2.5)^2 + 5

    //    return new Vector3(direction.x * t, direction.y * t, -z);
    //}

    //My Attempt At parabola
    //float distance = Vector2.Distance(target, startingPos);
    //float gradient = (target.y - startingPos.y) / (target.x - startingPos.x);
    //distance = Mathf.Pow(distance, -1f);
    //        distanceTravelled += speed;
    //        float x = distanceTravelled;
    //float y = gradient * distanceTravelled;
    //        if (startingPos.x == 0)
    //        {
    //            coefficient = (height / (Mathf.Abs(target.x)));
    //        }
    //        else if (target.x == 0)
    //        {
    //            coefficient = (height / (Mathf.Abs(startingPos.x)));
    //        }
    //        else
    //        {
    //            coefficient = (height / (Mathf.Abs(startingPos.x* target.x)));
    //        }
    //        float z = coefficient * (distanceTravelled - startingPos.x) * (distanceTravelled - target.x);
}
