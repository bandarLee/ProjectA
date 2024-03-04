using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using System;
using Cinemachine;

public class MainManager : MonoBehaviourPunCallbacks{
    PhotonView PV;
    public Text[] NicknameTexts;
    public bool IsGameEnd = false;
    public static MainManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<MainManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static MainManager m_instance; // �̱����� �Ҵ�� static ����
    public GameObject PlayerPrefab;
    private string m_Name;
    public bool IsGameover { get; private set; } // ���� ���� ����

    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;

    [PunRPC]
    public void UpdateNickname(string nickname)
    {
        // ��� �÷��̾ ȣ���Ͽ� �г��� ������Ʈ
        UIManager.instance.UpdateMemberList(nickname);
    }


    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        StartCinematic();

    }
    void Start()
    {
        PV = photonView;
        Vector3 Playerposition = new Vector3(-49.94627f, -0.165f, 9.093761f);
        PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, Quaternion.identity);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                NicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;
            }
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public void StartCinematic()
    {
        cinematicCamera.Priority = 11;
        playerCamera.Priority = 9;

        Invoke("EndCinematicAndSpawnPlayer", 16.5f); 
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            NicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;

        }
    }
    void EndCinematicAndSpawnPlayer()
    {
        cinematicCamera.Priority = 9;
        playerCamera.Priority = 11;



    }
    void Update()
    {
            
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        NicknameTexts[PhotonNetwork.PlayerList.Length].text = null;

        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            NicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;

        }



    }

}
