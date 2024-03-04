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
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<MainManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static MainManager m_instance; // 싱글톤이 할당될 static 변수
    public GameObject PlayerPrefab;
    private string m_Name;
    public bool IsGameover { get; private set; } // 게임 오버 상태

    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;

    [PunRPC]
    public void UpdateNickname(string nickname)
    {
        // 모든 플레이어가 호출하여 닉네임 업데이트
        UIManager.instance.UpdateMemberList(nickname);
    }


    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
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
