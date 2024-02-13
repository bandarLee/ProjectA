using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angelwall : MonoBehaviour
{
    public GameObject DoorAngel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Scene1Manager.Angel();

        }

    }
}
