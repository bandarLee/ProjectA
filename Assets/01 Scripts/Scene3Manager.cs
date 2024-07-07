using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Scene3Manager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    private static Scene3Manager m_instance; // ΩÃ±€≈Ê¿Ã «“¥Áµ… static ∫Øºˆ
    public GameObject PlayerPrefab;
    public static Scene3Manager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Scene3Manager>();
            }

            return m_instance;
        }
    }
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        InitializePlayerHealth();
        PV = photonView;
        Vector3 Playerposition = new Vector3(-18.19f, -15f, -11.7f);
        PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, Quaternion.identity);

        Hashtable playerProperties = new Hashtable();


        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
    void Start()
    {
   
    }

    void Update()
    {
        
    }
    void InitializePlayerHealth()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable initialProps = new Hashtable { { "Health", 3 } };
            player.SetCustomProperties(initialProps);
        }
    }
}
