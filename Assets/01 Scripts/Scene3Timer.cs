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


    private CanvasGroup ruleText1;
    private CanvasGroup ruleText2;
    private CanvasGroup ruleText3;

    public float fadeDuration = 1f; // 페이드 지속 시간
    public TMP_Text DeathEnding;
    List<string> deathPlayersInfo = new List<string>();

    public GameObject Round1Question;
    public GameObject Round1Result;

    public GameObject Roundwait;

    public GameObject[] Round1Text;
    public TMP_Text[] Playername;
    public TMP_Text[] Playernumber;
    public TMP_Text resultnumber;
    public TMP_Text resultnotice;

    public GameObject[] RoundTitles;
    private bool timerEnded = false; 
    public TMP_Text PlayerInputField;
    
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
        Round1Result.SetActive(false);


}

void Start()
    {
        StartCoroutine(GameSequence());


    }
    IEnumerator GameSequence()
    {
        yield return StartCoroutine(TimerCoroutine(30)); 
        yield return StartCoroutine(RuleDescript());
        yield return PrepareForRound();
        yield return StartCoroutine(TimerCoroutine(30));
        yield return StartCoroutine(RoundGame(2));
        yield return PrepareForRound();

        yield return StartCoroutine(TimerCoroutine(30));
        yield return StartCoroutine(RoundGame(3));
        yield return PrepareForRound();

        yield return StartCoroutine(TimerCoroutine(30));
        yield return StartCoroutine(RoundGame(4));
        yield return PrepareForRound();

        yield return StartCoroutine(TimerCoroutine(30));
        yield return StartCoroutine(RoundGame(5));
        yield return PrepareForRound();

        yield return StartCoroutine(TimerCoroutine(30));


    }


    [PunRPC]
    void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }

    IEnumerator TimerCoroutine(int duration)
    {
        timerEnded = false;
        var wait = new WaitForSeconds(1f);

        if (PhotonNetwork.IsMasterClient)
        {
            while (duration > 0)
            {
                PV.RPC("ShowTimer", RpcTarget.All, duration);
                duration--;
                yield return wait;
            }

            PV.RPC("ShowTimer", RpcTarget.All, duration); // 마지막 시간 업데이트
            PV.RPC("NotifyTimerEnd", RpcTarget.All); // 모든 클라이언트에 타이머 종료 알림
        }
        else
        {
            // 마스터 클라이언트가 아닌 경우, 마스터 클라이언트의 타이머 종료 알림을 기다립니다.
            yield return new WaitUntil(() => timerEnded); // `timerEnded`는 타이머 종료 상태를 나타내는 bool 변수입니다.
        }
    

    // 타이머가 끝나면 룰 설명 시작




    /*foreach (Player player in PhotonNetwork.PlayerList)
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



    PV.RPC("StartEndGameSequence", RpcTarget.All);*/

}

    [PunRPC]
    void NotifyTimerEnd()
    {
        timerEnded = true; // 모든 클라이언트에서 타이머가 끝났음을 나타내는 변수를 true로 설정합니다.
                           // 필요한 경우, 여기에서 다음 단계로 진행하는 로직을 구현할 수도 있습니다.
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
        timerText.gameObject.SetActive(false);
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
        Roundwait.SetActive(false);
        foreach (GameObject title in RoundTitles)
        {
            title.SetActive(false);
        }
        RoundTitles[0].SetActive(true);
        timerText.gameObject.SetActive(true);


    }

    public void EnterNumber()
    {
        ExitGames.Client.Photon.Hashtable playerPickNumbers = new ExitGames.Client.Photon.Hashtable();
        //플레이어가 선택한 숫자를 넣어주는 해쉬테이블생성
            string inputnumber = PlayerInputField.text.Trim();
            inputnumber = inputnumber.Replace("\u200B", "");

            int playerPickValue = int.Parse(inputnumber);

            playerPickNumbers.Add("ChooseNumber", playerPickValue);

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerPickNumbers);
            foreach (GameObject textObject in Round1Text)
            {
                textObject.SetActive(false);
            }
            Roundwait.SetActive(true);


    }

    private IEnumerator RoundGame(int round)
    {
        timerText.gameObject.SetActive(false);

        Round1Question.SetActive(false);
       
        Round1Result.SetActive(true);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Playername[i].text = PhotonNetwork.PlayerList[i].NickName; 

            object chooseNumberObj;
            if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("ChooseNumber", out chooseNumberObj))
            {
                if (chooseNumberObj is int chooseNumber)
                {
                    Playernumber[i].text = chooseNumber.ToString();
                }
                else
                {
                    Playernumber[i].text = "N/A"; //이때 이제, Null값이니까 Flag를 세워서 WinFlag를 만들어
                }
            }
            else
            {
                Playernumber[i].text = "N/A"; //이제, Null값이니까 Flag를 세워서 WinFlag를 만들어
            }
        }

        yield return new WaitForSeconds(8);

        Round1Result.SetActive(false);
        Round1Question.SetActive(true);
        foreach (GameObject title in RoundTitles)
        {
            title.SetActive(false);
        }
        RoundTitles[round-1].SetActive(true);
        timerText.gameObject.SetActive(true);
        foreach (GameObject textObject in Round1Text)
        {
            textObject.SetActive(true);

        }
        Roundwait.SetActive(false);


    }
    private IEnumerator PrepareForRound()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "ChooseNumber", null } });
        yield return null;
    }
}
