using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using System;
using System.Reflection;
using UnityEditor;
using System.Linq;
using Cinemachine;

public class Scene4Manager : MonoBehaviourPunCallbacks
{
    public GameObject Image1;
    public GameObject Image2;
    public GameObject Image3;
    public GameObject Image4;
    public GameObject Image5;

    public GameObject CubeImage;

    private CanvasGroup canvasGroup1;
    private CanvasGroup canvasGroup2;
    private CanvasGroup canvasGroup3;
    private CanvasGroup canvasGroup4;
    private CanvasGroup canvasGroup5;
    private CanvasGroup cubeDescribe;

    public GameObject[] Mummies;
    public GameObject[] Mummies2;

    public float fadeDuration = 1f; // ���̵� ���� �ð�

    public TMP_Text DeathEnding;
    List<string> deathPlayersInfo = new List<string>();

    private bool Scene4Ending = false;

    public GameObject Notice;

    PhotonView PV;
    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;
    private static Scene4Manager m_instance; // �̱����� �Ҵ�� static ����
    public GameObject PlayerPrefab;
    public static Scene4Manager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<Scene4Manager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }

        PlayerMovement.isPositionFixed = true;

        Image1.SetActive(false);
        Image2.SetActive(false);
        Image3.SetActive(false);
        Image4.SetActive(false);
        Image5.SetActive(false);
        foreach (GameObject mummy in Mummies)
        {
            mummy.SetActive(false);

        }
        foreach (GameObject mummy2 in Mummies2)
        {
            mummy2.SetActive(false);

        }
        canvasGroup1 = Image1.GetComponent<CanvasGroup>();
        canvasGroup2 = Image2.GetComponent<CanvasGroup>();
        canvasGroup3 = Image3.GetComponent<CanvasGroup>();
        canvasGroup4 = Image4.GetComponent<CanvasGroup>();
        canvasGroup5 = Image5.GetComponent<CanvasGroup>();
        cubeDescribe = CubeImage.GetComponent<CanvasGroup>();
        CubeImage.SetActive(false);
        StartCinematic();

    }
    public void StartCinematic()
    {
        cinematicCamera.Priority = 11;
        playerCamera.Priority = 9;

        Invoke("EndCinematicAndSpawnPlayer", 10f);
    }
    void EndCinematicAndSpawnPlayer()
    {
        cinematicCamera.Priority = 9;

        playerCamera.Priority = 11;
        AudioManager.instance.PlayAudio(0);

        StartCoroutine(Waitfortime());

    }
    void Start()
    {
        PV = photonView;
        {
            PV = photonView;
            Vector3 Playerposition = new Vector3(-39.1f, -11.45f, -82.767f);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, rotation);

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
   

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }



    }

    public void CheckPlayersAndLoadScene()
    {
        int playersWithStatus = 0; // ���°� ������ �÷��̾� ��

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object status;
            if (player.CustomProperties.TryGetValue("Status", out status))
            {
                if (status is int playerStatus && (playerStatus == 1 || playerStatus == 2))
                {
                    playersWithStatus++; // ���� 1 �Ǵ� 2�� �ش��ϴ� �÷��̾� �� ����

             
                }
            }
        }

        // ��� �÷��̾ ���¸� ������ �ִٸ� ���� ������ ����
        if (playersWithStatus == PhotonNetwork.PlayerList.Length && !Scene4Ending)
        {
            Scene4Ending = true;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                object status;

                if (player.CustomProperties.TryGetValue("Status", out status))
                    if ((int)status == 2) // ���°� 2�� �÷��̾ ���� ������ �����մϴ�.
                         {
                            string personality = player.CustomProperties["Personality"] as string;
                            string nickname = player.NickName;
                            deathPlayersInfo.Add("��Ī " + personality + " " + nickname + ",\n");
                        }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                if (deathPlayersInfo.Count > 0)
                {
                    string deathPlayersStr = string.Join("\n", deathPlayersInfo);
                    PV.RPC("UpdateDeathPlayersInfo", RpcTarget.AllBuffered, deathPlayersStr);
                }

                // ��� ������ �����Ǿ��� ���� ���� �������� ����
                PV.RPC("StartEndGameSequence", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }
    [PunRPC]

    IEnumerator EndGameSequence()
    {
        Image1.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup1, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        Image2.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup2, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        Image1.SetActive(false);
        Image3.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup3, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        Image2.SetActive(false);
        Image4.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup4, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(5);
        Image3.SetActive(false);
        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;


        AssignNewMasterClient4();
        object statusObject;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Status", out statusObject))
        {
            int status = (int)statusObject;
            if (status == 1)
            {

                SceneManager.LoadScene("Scene5");
            }
            else
            {
                Image5.SetActive(true);
                Image4.SetActive(false);
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }

    }
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }
    void AssignNewMasterClient4()
    {
        if ((int)PhotonNetwork.MasterClient.CustomProperties["Status"] == 2)
        {
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Player player in players)
            {
                if ((int)player.CustomProperties["Status"] == 1)
                {
                    PhotonNetwork.SetMasterClient(player);
                    break;
                }
            }
        }
    }
    [PunRPC]
    void UpdateDeathPlayersInfo(string playersInfo)
    {
        if (string.IsNullOrEmpty(playersInfo))
        {
            DeathEnding.text = "\n\n\n������ �ƹ��� ���׾����ϴ�.";
        }
        else
        {
            DeathEnding.text = string.Join("\n", playersInfo) + "\nť�꿡�� ���� �߸��� ����߽��ϴ�.";
        }
    }

    private IEnumerator Waitfortime()
    {
        CubeImage.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup1, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(4);
        CubeImage.SetActive(false);
        PlayerMovement.isPositionFixed = false;

    }
}
