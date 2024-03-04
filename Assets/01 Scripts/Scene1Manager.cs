using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class Scene1Manager : MonoBehaviourPunCallbacks
{
    PhotonView PV;



    public static Scene1Manager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<Scene1Manager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static Scene1Manager m_instance; // 싱글톤이 할당될 static 변수
    public GameObject PlayerPrefab;
    public bool IsGameover { get; private set; } // 게임 오버 상태

    // 부딪힐 때 전환할 씬의 이름

 
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }
    void Start()
    {


        PV = photonView;
        Vector3 Playerposition = new Vector3(29.74f, -10.39f, 1.87f);
        PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, Quaternion.identity);

        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("Scene1order", 0);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);



    }
    public static void Angel()
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("Scene1order", 0);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

    }
    public static void Demon()
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("Scene1order", 1);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

    }

}
