using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviourPunCallbacks
{
    
    public GameObject itemtriggertext;
    private void Awake()
    {
        itemtriggertext.SetActive(false);

    }
    private void OnTriggerStay(Collider other)
    {
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

                if (playerMovement.maceweapon.activeSelf && playerMovement.attackKey)
                {
                    itemtriggertext.SetActive(false);
                }
                else
                {
                    itemtriggertext.SetActive(true);

                    if (Input.GetKey(KeyCode.E))
                    {
                        playerMovement.ChangeWeaponState(true);


                        itemtriggertext.SetActive(false);

                        photonView.RPC("RequestDestroy", RpcTarget.MasterClient);
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PhotonView pv = other.gameObject.GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                itemtriggertext.SetActive(false);
            }
        }
    }
    [PunRPC]
    public void RequestDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
