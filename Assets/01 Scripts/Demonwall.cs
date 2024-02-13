using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonwall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Scene1Manager.Demon();

        }

    }
}
