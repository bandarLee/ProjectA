using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlock : MonoBehaviourPunCallbacks
{
    public GameObject Stage;
    public GameObject Cave;

    public AudioSource audioSource; // ����� �ҽ� ������Ʈ ����
    private void Awake()
    {
        Stage.SetActive(false);

    }
 
    private void OnCollisionEnter(Collision collision)
    {
        PhotonView pv = collision.gameObject.GetComponent<PhotonView>();

        if (collision.gameObject.CompareTag("Player"))
        {
            if (pv.IsMine)
            {
                Cave.SetActive(false);
                Stage.SetActive(true); 
                audioSource.Play();
            }
        }

    }
    void Update()
    {
        
    }
}
