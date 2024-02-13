using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class Scene3Manager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    private static Scene3Manager m_instance; // 싱글톤이 할당될 static 변수
    public GameObject PlayerPrefab;
    public static Scene3Manager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<Scene3Manager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
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
        Vector3 Playerposition = new Vector3(-27.58f, -15.89f, 8.58f);
        PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, Quaternion.identity);

        Hashtable playerProperties = new Hashtable();


        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    void Update()
    {
        
    }
}
