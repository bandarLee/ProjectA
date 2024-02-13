using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class MainPortal : MonoBehaviourPunCallbacks
{
    public string sceneToLoad;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&& PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneToLoad);
        }
    }
}