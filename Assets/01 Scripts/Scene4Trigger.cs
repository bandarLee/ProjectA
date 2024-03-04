using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Trigger : MonoBehaviour
{
    public enum Trap
    {
        Trap,
        Portal
    }
    public Trap trap;

    public GameObject[] Traps;
    public GameObject WaitImage;
    private void Awake()
    {
        WaitImage.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (trap == Trap.Trap)
            {
                StartCoroutine(waitfortraptime(2, other.gameObject));
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach (GameObject monster in monsters)
                {
                    PhotonView pv = monster.GetComponent<PhotonView>();
                    if (pv != null)
                    {
                        pv.RPC("ChangeStateToSearch", RpcTarget.All);
                    }
                }
            }
            if (trap == Trap.Portal)
            {
                StartCoroutine(portal(0.3f, other.gameObject));
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach (GameObject monster in monsters)
                {
                    PhotonView pv = monster.GetComponent<PhotonView>();
                    if (pv != null)
                    {
                        pv.RPC("ChangeStateToSearch", RpcTarget.All);
                    }
                }
            }
        }



    }

    private IEnumerator waitfortraptime(float time, GameObject playerHit)
    {

        yield return new WaitForSeconds(time);
        foreach (var t in Traps)
        {
            t.SetActive(true);
            UpdatePlayerStatus(playerHit, 2); // 플레이어 상태를 "맞았음"으로 업데이트
            PhotonView photonview = playerHit.GetComponent<PhotonView>();

            if (photonview.IsMine)
            {
                PlayerMovement.isPositionFixed = true;
                WaitImage.SetActive(true);

                StartCoroutine(RotatePlayerOverTime(playerHit.gameObject, Quaternion.Euler(-86f, -127f, 0f), 3));

            }
            yield return new WaitForSeconds(time);
            t.SetActive(false);

        }
        Scene4Manager.instance.CheckPlayersAndLoadScene();


    }
    private IEnumerator portal(float time, GameObject playerHit)
    {

        yield return new WaitForSeconds(time);
   
            UpdatePlayerStatus(playerHit, 1); // 플레이어 상태를 "맞았음"으로 업데이트
            PhotonView photonview = playerHit.GetComponent<PhotonView>();

            if (photonview.IsMine)
            {
                PlayerMovement.isPositionFixed = true;
                WaitImage.SetActive(true);

            }
        yield return new WaitForSeconds(time);

        playerHit.transform.position = new Vector3(212.9f, 24.11f,-68.2f);
        playerHit.transform.rotation = Quaternion.Euler(new Vector3(0, 100, 0));

        Scene4Manager.instance.CheckPlayersAndLoadScene();


    }

    void UpdatePlayerStatus(GameObject player, int status)
    {
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "Status", status }
        };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
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
