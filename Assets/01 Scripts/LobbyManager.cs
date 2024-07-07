using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public TMP_InputField NicknameInput;
    public TMP_InputField PersonalityInput;

    public Text connectionInfoText;
    public Button joinButton;



    private void Start()
    {

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속중...";
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        Hashtable playerProperties = new Hashtable();
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            string personalityValue = PersonalityInput.text;

            playerProperties.Add("Personality", personalityValue);

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }

        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "룸에 접속...";
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 6 }; 
            PhotonNetwork.JoinOrCreateRoom("MyUniqueRoom", roomOptions, TypedLobby.Default);
        
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }



    public override void OnJoinedRoom()
    {
        connectionInfoText.text = " 파티에 참가합니다. ";
        PhotonNetwork.LoadLevel("Main");
    }

}
