using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessLigh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Light>().color = new Color(-1f, -1f, -1f);
    }

    
}
