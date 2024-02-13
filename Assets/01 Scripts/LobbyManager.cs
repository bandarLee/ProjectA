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
    //��Ʈ��ũ ���� ǥ�� �ؽ�Ʈ
    public Button joinButton;
    //�� ���� ��ư



    private void Start()
    {

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "������ ������ ������...";
    }

    public override void OnConnectedToMaster()
    {
        // �� ���� ��ư�� Ȱ��ȭ
        joinButton.interactable = true;
        // ���� ���� ǥ��
        connectionInfoText.text = "�¶��� : ������ ������ �����";
    }

    // ������ ���� ���� ���н� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư�� ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� ���� ǥ��
        connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
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

        // �ߺ� ���� �õ��� ���� ����, ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ������ ������ �������̶��
        if (PhotonNetwork.IsConnected)
        {
            // �� ���� ����
            connectionInfoText.text = "�뿡 ����...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // ������ ������ �������� �ƴ϶��, ������ ������ ���� �õ�
            connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";
            // ������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ���� ǥ��
        connectionInfoText.text = "����� ��Ƽ���Դϴ�. ���ſ� ���ѿ��� �׿� ������ å����";
        // �ִ� �ο� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ���� ���� ǥ��
        connectionInfoText.text = " ��Ƽ�� �����մϴ�. ";
        // ��� �� �����ڵ��� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Main");
    }

}
