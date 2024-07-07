using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    private PhotonView parentPhotonView; // �θ� ������Ʈ�� PhotonView ������ ������ ����

    private void Awake()
    {
        // �θ� ������Ʈ���� PhotonView ������Ʈ�� ã�Ƽ� �����մϴ�.
        parentPhotonView = GetComponentInParent<PhotonView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Scene5Wall"))
        {
            photonView.RPC("RequestDestroy", RpcTarget.MasterClient, other.gameObject.GetPhotonView().ViewID);
        }
        /*if (other.gameObject.tag == ("Monster"))
        {
            photonView.RPC("RequestDestroy", RpcTarget.MasterClient, other.gameObject.GetPhotonView().ViewID);
        }*/
    }
    [PunRPC]
    void RequestDestroy(int viewID)
    {
        // viewID�� ����Ͽ� ������ ������Ʈ�� ã���ϴ�.
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            PhotonNetwork.Destroy(targetView.gameObject);
        }
    }
}
