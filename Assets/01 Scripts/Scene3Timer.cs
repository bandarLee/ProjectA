using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using System;

public class Scene3Timer : MonoBehaviourPunCallbacks
{
    public GameObject Image1;
    public GameObject Image2;
    public GameObject Image3;
    public GameObject Image4;
    public GameObject Image5;
    public GameObject Sit;
    public GameObject Sit2;
    public int time = 0;
    public TMP_Text timerText;

    public GameObject Rule1;
    public TMP_Text liveText1;
    public TMP_Text liveText2;
    public TMP_Text liveText3;
    public static bool RuleDescriptEnd = false;
    public TMP_Text PressEText;


    private CanvasGroup canvasGroup1;
    private CanvasGroup canvasGroup2;
    private CanvasGroup canvasGroup3;
    private CanvasGroup canvasGroup4;
    private CanvasGroup canvasGroup5;

    public TextMeshProUGUI InputArea;
    public int ValueInput;

    private CanvasGroup ruleText1;
    private CanvasGroup ruleText2;
    private CanvasGroup ruleText3;

    public float fadeDuration = 1f; // 페이드 지속 시간
    public TMP_Text DeathEnding;
    List<string> deathPlayersInfo = new List<string>();

    public GameObject Round1Question;
    public GameObject Round2Question;
    public GameObject Round3Question;
    public GameObject Round4Question;

    public PhotonView PV;

    void Awake()
    {

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
        Rule1.SetActive(false);
        liveText1.gameObject.SetActive(false);
        liveText2.gameObject.SetActive(false);
        liveText3.gameObject.SetActive(false);
        ruleText1 = liveText1.GetComponent<CanvasGroup>();
        ruleText2 = liveText2.GetComponent<CanvasGroup>();
        ruleText3 = liveText3.GetComponent<CanvasGroup>();
        Round1Question.SetActive(false);


    }

    void Start()
    {
        StartTimer();


    }

    void StartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time = 30;

            StartCoroutine(TimerCoroution(30, () => StartCoroutine(RuleDescript())));

        }

       
    }
    [PunRPC]
    void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }

    IEnumerator TimerCoroution(int duration, Action onComplete)
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


            if (time <= 0)
            {
                PV.RPC("ShowTimer", RpcTarget.All, time); //1초 마다 방 모두에게 전달



                break;
            }

        }


        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((int)player.CustomProperties["Scene3sitorder"] == 0)
            {
                string personality = player.CustomProperties["Personality"] as string;
                string nickname = player.NickName as string;


                deathPlayersInfo.Add("자칭 " + personality + " " + nickname + ",\n");


            }


        }
        string deathPlayersStr = string.Join("\n", deathPlayersInfo);

        PV.RPC("UpdateDeathPlayersInfo", RpcTarget.AllBuffered, deathPlayersStr);



        PV.RPC("StartEndGameSequence", RpcTarget.All);



    }
    [PunRPC]

    IEnumerator EndGameSequence()
    {
        PressEText.gameObject.SetActive(false);
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

        int scene1order = (int)PhotonNetwork.LocalPlayer.CustomProperties["Scene1order"];
        if (scene1order == 1)
        {
            SceneManager.LoadScene("Scene3");
        }
        else if (scene1order == 0)
        {
            Image5.SetActive(true);
            Image4.SetActive(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

    }


 



    [PunRPC]
    void ShowTimer(int number)
    {
        timerText.text = number.ToString(); //타이머 갱신
    }
    [PunRPC]
    void UpdateDeathPlayersInfo(string playersInfo)
    {
        DeathEnding.text = string.Join("\n", playersInfo) + "\n불타버려 던전 탐사 1분만에 죽었습니다.";
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

    private IEnumerator RuleDescript()
    {
        Sit.SetActive(false);
        Sit2.SetActive(false);
        Rule1.SetActive(true);
        liveText1.gameObject.SetActive(true);
        liveText2.gameObject.SetActive(true);

        StartCoroutine(FadeCanvasGroup(ruleText1, 0f, 1f, fadeDuration));
        StartCoroutine(FadeCanvasGroup(ruleText2, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(6);
        liveText2.gameObject.SetActive(false);

        liveText3.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(ruleText3, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(6);
        Rule1.SetActive(false);
        RuleDescriptEnd = true;
        Round1Question.SetActive(true);

    }
}
    