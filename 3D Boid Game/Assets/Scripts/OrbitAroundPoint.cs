using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundPoint : MonoBehaviour
{
    public bool orbit;
    public float orbitSpeed;
    public Transform orbitPoint;
    public float radius;
    public float orbitAngle;
    public bool rotate;
    public float rotateSpeed;
    public float rotateAngle;
    
    public float xMultiplier;
    public float yMultiplier;
    public float zMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponentInChildren<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (orbit)
        {
            orbitAngle += orbitSpeed * Time.deltaTime;
            Vector2 offset = new Vector2(Mathf.Sin(orbitAngle), Mathf.Cos(orbitAngle)) * radius;
            transform.position = orbitPoint.position + (Vector3)offset;
        }

        if (rotate)
        {
            transform.Rotate(rotateSpeed * xMultiplier, rotateSpeed * yMultiplier, rotateSpeed * zMultiplier);
        }
        
    }
}
