using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using TMPro;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using UnityEditor;
using System.Linq;
using Cinemachine;
public class Scene5Manager : MonoBehaviourPunCallbacks
{

    public GameObject Image1;
    public GameObject Image2;
    public GameObject Image3;
    public GameObject Image4;
    public GameObject Image5;

    public int time = 0;

    public TMP_Text timerText;
    public TMP_Text distanceText;

    private CanvasGroup canvasGroup1;
    private CanvasGroup canvasGroup2;
    private CanvasGroup canvasGroup3;
    private CanvasGroup canvasGroup4;
    private CanvasGroup canvasGroup5;
    public float fadeDuration = 1f; // 페이드 지속 시간
    public TMP_Text DeathEnding;
    List<string> deathPlayersInfo = new List<string>();
    public PhotonView PV;
    public static Scene5Manager m_instance; 
    public GameObject PlayerPrefab;

    public GameObject water1;
    public GameObject water2;
    private Vector3 originalSpawnPosition; // 캐릭터의 원래 스폰 위치를 저장할 변수
    private GameObject localPlayerInstance; // 로컬 플레이어의 캐릭터 객체에 대한 참조
    public GameObject waterimage;
    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;

    public static Scene5Manager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Scene5Manager>();
            }

            return m_instance;
        }
    }

    void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        PlayerMovement.isPositionFixed = false;

        Image1.SetActive(false);
        Image2.SetActive(false);    
        Image3.SetActive(false);
        Image4.SetActive(false);
        Image5.SetActive(false);
        canvasGroup1 = Image1.GetComponent<CanvasGroup>();
        canvasGroup2 = Image2.GetComponent<CanvasGroup>();
        canvasGroup3 = Image3.GetComponent<CanvasGroup>();
        canvasGroup4 = Image4.GetComponent<CanvasGroup>();
        canvasGroup5 = Image5.GetComponent<CanvasGroup>();

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("Scene5order", 1);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        StartCinematic();

    }
    public void StartCinematic()
    {
        cinematicCamera.Priority = 11;
        playerCamera.Priority = 9;

        Invoke("EndCinematicAndSpawnPlayer", 5f);
    }
    void EndCinematicAndSpawnPlayer()
    {
        cinematicCamera.Priority = 9;

        playerCamera.Priority = 11;
        StartTimer();
        AudioManager.instance.PlayAudio(0);


    }
    void Start()
    {

        PV = photonView;
        {

            PV = photonView;

            Vector3 Playerposition = new Vector3(109.5871f, -2.354059f, 4.142615f);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 150, 0));
            originalSpawnPosition = Playerposition;

            GameObject playerObj = PhotonNetwork.Instantiate(PlayerPrefab.name, Playerposition, rotation);

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            PlayerMovement.isPositionFixed = false;
            if (playerObj.GetComponent<PhotonView>().IsMine)
            {
                localPlayerInstance = playerObj; // 로컬 플레이어의 캐릭터 객체 참조 저장
                originalSpawnPosition = Playerposition; // 원래 스폰 위치 기록
            }


        }

    }

    private void FixedUpdate()
    {
      
            DebugCharacterMovementDistance();
        
    }
    void StartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time = 80;

            StartCoroutine(TimerCoroution());
        }
    }
    [PunRPC]
    void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }
    IEnumerator TimerCoroution()
    {
        var wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            if (time > 0)
                {
                    PV.RPC("ShowTimer", RpcTarget.All, time); //1초 마다 방 모두에게 전달
                    time -= 1;
                }
            if ( time == 10)
            {
                PV.RPC("PlayTenSecondsLeftAudio", RpcTarget.All);

            }

            if (time <= 0)
                {
                    PV.RPC("ShowTimer", RpcTarget.All, time); //1초 마다 방 모두에게 전달


                    break;
                }
        }
        StartCoroutine(WaterCoroution());



    }
    IEnumerator WaterCoroution()
    {
        float duration = 20f; 
        float elapsed = 0f;

        // 시작 위치와 끝 위치 설정
        Vector3 startWater1Pos = water1.transform.position;
        Vector3 endWater1Pos = new Vector3(112.47f, -2.12f, -34.91f);

        Vector3 startWater2Pos = water2.transform.position;
        Vector3 endWater2Pos = new Vector3(112.06f, 8.84f, 1.61f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            water1.transform.position = Vector3.Lerp(startWater1Pos, endWater1Pos, elapsed / duration);
            water2.transform.position = Vector3.Lerp(startWater2Pos, endWater2Pos, elapsed / duration);
            yield return null;

        }





        if (water1.transform.position == endWater1Pos && water2.transform.position == endWater2Pos)
        {
            PV.RPC("StartEndGameSequence", RpcTarget.All);

        }
    }
    [PunRPC]
    IEnumerator EndGameSequence()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        bool isScene5WallExists = GameObject.FindWithTag("Scene5Wall") != null;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int scene5ordersmall = (int)player.CustomProperties["Scene5order"];

            if (scene5ordersmall == 0 || (scene5ordersmall == 1 && isScene5WallExists))
            {
                string personality = player.CustomProperties["Personality"] as string;
                string nickname = player.NickName as string;

                deathPlayersInfo.Add("자칭 " + personality + " " + nickname + ",\n");
            }


        }
        string deathPlayersStr = string.Join("\n", deathPlayersInfo);

        PV.RPC("UpdateDeathPlayersInfo", RpcTarget.AllBuffered, deathPlayersStr);
        int scene5order = (int)PhotonNetwork.LocalPlayer.CustomProperties["Scene5order"];
        waterimage.SetActive(false);

        Image1.SetActive(true);
        AudioManager.instance.PlayAudio(2);

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
       
        AssignNewMasterClient();
        if (!isScene5WallExists && PhotonNetwork.LocalPlayer.CustomProperties["Scene5order"] as int? == 1)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            Image5.SetActive(true);
            Image4.SetActive(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        void AssignNewMasterClient()
        {
            if ((int)PhotonNetwork.MasterClient.CustomProperties["Scene5order"] != 1)
            {
                Player[] players = PhotonNetwork.PlayerList;

                foreach (Player player in players)
                {
                    if ((int)player.CustomProperties["Scene5order"] == 1)
                    {
                        PhotonNetwork.SetMasterClient(player);
                        break;
                    }
                }
            }
        }


    }
    [PunRPC]
    void PlayTenSecondsLeftAudio()
    {
        AudioManager.instance.PlayAudio(1);
    }
    [PunRPC]
    void ShowTimer(int number)
    {
        timerText.text = number.ToString(); //타이머 갱신
    }
    [PunRPC]
    void UpdateDeathPlayersInfo(string playersInfo)
    {
        if (string.IsNullOrEmpty(playersInfo))
        {
            DeathEnding.text = "\n\n\n던전을 탈출하셨습니다.";
        }
        else
        {
            DeathEnding.text = string.Join("\n", playersInfo) + "\n차오른 빗물에 빠져 죽었습니다.";
        }
    }
    [PunRPC]

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
    public void DebugCharacterMovementDistance()
    {
        Vector3 currentPos = localPlayerInstance.transform.position; 
        float distance = Vector3.Distance(originalSpawnPosition, currentPos); 
        int distanceRounded = Mathf.RoundToInt(distance); 

        distanceText.text = $"{distanceRounded}"; 
    }
    public void Drowning()
    {

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties.Add("Scene5order", 0);

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        
    }
}
