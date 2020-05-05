using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void MoveCameraTo(Vector3 pos)
    {
        pos.z = 20;
        gameObject.transform.position = pos;
    }
}
