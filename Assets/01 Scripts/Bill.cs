using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    public Camera c;
    void Start()
    {

    }

    void Update()
    {
        transform.forward = c.transform.forward;
    }
}
