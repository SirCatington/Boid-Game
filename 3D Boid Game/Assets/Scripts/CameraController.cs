using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera camera;
    public float scale;
    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {

        camera.orthographicSize -= Input.mouseScrollDelta.y * scale;
        if (camera.orthographicSize > 25)
        {
            camera.orthographicSize = 25;
        }
        else if (camera.orthographicSize < 1)
        {
            camera.orthographicSize = 1;
        }

    }

    public void MoveCameraTo(Vector3 pos)
    {
        pos.z = 20;
        gameObject.transform.position = pos;
    }
}
