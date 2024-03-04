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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<Scene1Manager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    private static Scene1Manager m_instance; // �̱����� �Ҵ�� static ����
    public GameObject PlayerPrefab;
    public bool IsGameover { get; private set; } // ���� ���� ����

    // �ε��� �� ��ȯ�� ���� �̸�

 
    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
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
