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
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Scene1Manager>();
            }

            return m_instance;
        }
    }
    private static Scene1Manager m_instance; 
    public GameObject PlayerPrefab;
    public bool IsGameover { get; private set; } 


 
    private void Awake()
    {
        if (instance != this)
        {
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
