using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks
{
    private PhotonView parentPhotonView; // 부모 오브젝트의 PhotonView 참조를 저장할 변수

    private void Awake()
    {
        // 부모 오브젝트에서 PhotonView 컴포넌트를 찾아서 저장합니다.
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
        // viewID를 사용하여 삭제할 오브젝트를 찾습니다.
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            PhotonNetwork.Destroy(targetView.gameObject);
        }
    }
}
