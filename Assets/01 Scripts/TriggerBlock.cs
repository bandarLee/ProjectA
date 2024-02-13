using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlock : MonoBehaviour
{
    public GameObject Stage;

    void Start()
    {
        Stage.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            Stage.SetActive(true);

        }

    }
    void Update()
    {
        
    }
}
