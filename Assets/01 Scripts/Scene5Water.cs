using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Scene5Water : MonoBehaviour
{
    public GameObject waterimage;

    private void Awake()
    {
        waterimage.SetActive(false);

    }
    void Start()
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
    private void OnCollisionEnter(Collision Player)
    {
        PhotonView photonview = Player.gameObject.GetComponent<PhotonView>();
        if (photonview.IsMine)
        {
            waterimage.SetActive(true);

            PlayerMovement.isPositionFixed = true;
            Scene5Manager.instance.Drowning();

            StartCoroutine(RotatePlayerOverTime(Player.gameObject, Quaternion.Euler(-86f, -127f, 0f), 3));

        }



        IEnumerator RotatePlayerOverTime(GameObject player, Quaternion targetRotation, float duration)
        {
            Quaternion startRotation = player.transform.rotation;
            float time = 0f;

            while (time < duration)
            {
                player.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            player.transform.rotation = targetRotation;
        }

    }
}


   

   
    


