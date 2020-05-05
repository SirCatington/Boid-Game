using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float height;
    public float width;

    private void Start()
    {
        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(width, height);
    }
}
