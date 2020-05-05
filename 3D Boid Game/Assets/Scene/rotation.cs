using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        for (int angle = 0; angle < 360;)
        {
            Vector3 rotation = transform.up;
            rotation = Quaternion.Euler(0, 0, angle) * rotation;
            Debug.Log(rotation);
            Debug.DrawRay(transform.position, rotation * 10, Color.red, 100f);
            angle += 20;
        }
        
    }

    
}
