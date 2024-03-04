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
using System.Reflection;
using UnityEditor;
using System.Linq;
using Cinemachine;

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
    public GameObject EndImage1;
    public GameObject EndImage2;
    public GameObject EndImage3;
    public GameObject EndImage4;

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

    private CanvasGroup EndcanvasGroup1;
    private CanvasGroup EndcanvasGroup2;
    private CanvasGroup EndcanvasGroup3;
    private CanvasGroup EndcanvasGroup4;

    private CanvasGroup ruleText1;
    private CanvasGroup ruleText2;
    private CanvasGroup ruleText3;

    public float fadeDuration = 1f; // ���̵� ���� �ð�
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

    public Player winner;

    public TMP_Text PlayerHealth;
    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;
    void Awake()
    {

        Image1.SetActive(false);
        Image2.SetActive(false);
        Image3.SetActive(false);
        Image4.SetActive(false);
        Image5.SetActive(false);
        EndImage1.SetActive(false);
        EndImage2.SetActive(false);
        EndImage3.SetActive(false);
        EndImage4.SetActive(false);
        canvasGroup1 = Image1.GetComponent<CanvasGroup>();
        canvasGroup2 = Image2.GetComponent<CanvasGroup>();
        canvasGroup3 = Image3.GetComponent<CanvasGroup>();
        canvasGroup4 = Image4.GetComponent<CanvasGroup>();
        canvasGroup5 = Image5.GetComponent<CanvasGroup>();
        EndcanvasGroup1 = EndImage1.GetComponent<CanvasGroup>();
        EndcanvasGroup2 = EndImage2.GetComponent<CanvasGroup>();
        EndcanvasGroup3 = EndImage3.GetComponent<CanvasGroup>();
        EndcanvasGroup4 = EndImage4.GetComponent<CanvasGroup>();

        Rule1.SetActive(false);
        liveText1.gameObject.SetActive(false);
        liveText2.gameObject.SetActive(false);
        liveText3.gameObject.SetActive(false);
        ruleText1 = liveText1.GetComponent<CanvasGroup>();
        ruleText2 = liveText2.GetComponent<CanvasGroup>();
        ruleText3 = liveText3.GetComponent<CanvasGroup>();
        Round1Question.SetActive(false);
        Round1Result.SetActive(false);
        StartCinematic();


    }
    public void StartCinematic()
    {
        cinematicCamera.Priority = 11;
        playerCamera.Priority = 9;

        Invoke("EndCinematicAndSpawnPlayer", 8f);
    }
    void EndCinematicAndSpawnPlayer()
    {
        cinematicCamera.Priority = 9;

        playerCamera.Priority = 11;
        StartCoroutine(GameSequence());


    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        { // ������ Ŭ���̾�Ʈ������ ����
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable {
                { "Scene1order", 1 }
            };
                player.SetCustomProperties(initialProps);
            }
        }
        AudioManager.instance.PlayAudio(0);



    }
 
    IEnumerator GameSequence()
    {
        yield return StartCoroutine(TimerCoroutine(30));
        bool shouldEndGame = false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("Scene1order", out object orderObj) && orderObj is int order && order != 3)
            {
                string personality = player.CustomProperties["Personality"] as string;
                string nickname = player.NickName as string;
                PlayerMovement.isPositionFixed = false;


                deathPlayersInfo.Add("��Ī " + personality + " " + nickname + ",\n");
                string deathPlayersStr = string.Join("\n", deathPlayersInfo);
                PV.RPC("UpdateDeathPlayersInfo", RpcTarget.AllBuffered, deathPlayersStr);

                shouldEndGame = true;


                break; 
            }
        }

        // ������ �������� �ʴ� �÷��̾ ������ EndGameSequence ����
        if (shouldEndGame)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                PV.RPC("StartEndGameSequence2", RpcTarget.All);
            }
            yield break; // �߰� ������ �ߴ�
        }

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
        yield return StartCoroutine(RoundGame(6));


    }


    [PunRPC]
    void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }

    [PunRPC]
    void StartEndGameSequence2()
    {
        StartCoroutine(EndGameSequence2());
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
                if (duration == 10)
                {
                    PV.RPC("PlayTenSecondsLeftAudio", RpcTarget.All);
                }
                duration--;
                yield return wait;
            }

            PV.RPC("ShowTimer", RpcTarget.All, duration);
            PV.RPC("NotifyTimerEnd", RpcTarget.All);
        }
        else
        {
            yield return new WaitUntil(() => timerEnded);
        }




    }
    [PunRPC]
    void NotifyTimerEnd()
    {
        timerEnded = true; // ��� Ŭ���̾�Ʈ���� Ÿ�̸Ӱ� �������� ��Ÿ���� ������ true�� �����մϴ�.
                           // �ʿ��� ���, ���⿡�� ���� �ܰ�� �����ϴ� ������ ������ ���� �ֽ��ϴ�.
    }
    [PunRPC]
    void PlayTenSecondsLeftAudio()
    {
        AudioManager.instance.PlayAudio(1);                        
    }
    [PunRPC]

    IEnumerator EndGameSequence()
    {
        PressEText.gameObject.SetActive(false);

        Image1.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup1, 0f, 1f, fadeDuration));
        AudioManager.instance.PlayAudio(4);

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

        int scene3order = (int)PhotonNetwork.LocalPlayer.CustomProperties["Health"];

        AssignNewMasterClient3();
        if (scene3order > 0)
        {
            SceneManager.LoadScene("Scene4");
        }
        else if (scene3order <= 0)
        {
            Image5.SetActive(true);
            Image4.SetActive(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

    }
    [PunRPC]

    IEnumerator EndGameSequence2()
    {
        PressEText.gameObject.SetActive(false);
        EndImage1.SetActive(true);
        StartCoroutine(FadeCanvasGroup(EndcanvasGroup1, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        EndImage2.SetActive(true);
        StartCoroutine(FadeCanvasGroup(EndcanvasGroup2, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        EndImage1.SetActive(false);
        EndImage3.SetActive(true);
        StartCoroutine(FadeCanvasGroup(EndcanvasGroup3, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(4);
        EndImage2.SetActive(false);
        EndImage4.SetActive(true);
        StartCoroutine(FadeCanvasGroup(EndcanvasGroup4, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(5);
        EndImage3.SetActive(false);
        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        int scene3sitorder = (int)PhotonNetwork.LocalPlayer.CustomProperties["Scene1order"];

        AssignNewMasterClient3();
        if (scene3sitorder == 3)
        {
            SceneManager.LoadScene("Scene4");
        }
        else 
        {
            Image5.SetActive(true);
            EndImage4.SetActive(false);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

    }





    [PunRPC]
    void ShowTimer(int number)
    {
        timerText.text = number.ToString(); //Ÿ�̸� ����
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
            DeathEnding.text = string.Join("\n", playersInfo) + "\n�������� ó���� �޾� ����� ��ȭ�߽��ϴ�.";
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

    private IEnumerator RuleDescript()
    {
        timerText.gameObject.SetActive(false);
        Sit.SetActive(false);
        Sit2.SetActive(false);
        Rule1.SetActive(true);

        liveText1.gameObject.SetActive(true);
        liveText2.gameObject.SetActive(true);

        AudioManager.instance.PlayAudio(2);
        StartCoroutine(FadeCanvasGroup(ruleText1, 0f, 1f, fadeDuration));
        StartCoroutine(FadeCanvasGroup(ruleText2, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(14);
        liveText2.gameObject.SetActive(false);

        liveText3.gameObject.SetActive(true);
        AudioManager.instance.PlayAudio(3);

        StartCoroutine(FadeCanvasGroup(ruleText3, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(10);
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
        //�÷��̾ ������ ���ڸ� �־��ִ� �ؽ����̺����
        string inputnumber = PlayerInputField.text.Trim();
        inputnumber = inputnumber.Replace("\u200B", "");

        int playerPickValue = int.Parse(inputnumber);
        if (playerPickValue >= 0 && playerPickValue <= 100)
        {
            // ��ȿ�� ���� ���� ���ڸ� �������� ��
            playerPickNumbers.Add("ChooseNumber", playerPickValue);

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerPickNumbers);
            foreach (GameObject textObject in Round1Text)
            {
                textObject.SetActive(false);
            }
            Roundwait.SetActive(true);
        }
        else
        {
            // ��ȿ���� ���� ���ڸ� �����߰ų� ���ڸ� �������� �ʾ��� ��
            playerPickNumbers.Add("ChooseNumber", null);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerPickNumbers);
            foreach (GameObject textObject in Round1Text)
            {
                textObject.SetActive(false);
            }
            Roundwait.SetActive(true);

        }





    }

    private IEnumerator RoundGame(int round)
    {
        timerText.gameObject.SetActive(false);

        Round1Question.SetActive(false);

        Round1Result.SetActive(true);
        float total = 0;
        int count = 0;

        List<Player> playersWhoChoseZero = new List<Player>(); // 0�� ������ ��� �÷��̾�
        List<Player> playersWhoChoseHundred = new List<Player>(); // 100�� ������ ��� �÷��̾�
        Dictionary<int, List<Player>> numberChoices = new Dictionary<int, List<Player>>();
      
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("ChooseNumber", out object chooseNumberObj) && chooseNumberObj is int chooseNumber)
            {
                total += chooseNumber;
                count++;
                if (!numberChoices.ContainsKey(chooseNumber))
                {
                    numberChoices[chooseNumber] = new List<Player>();
                }
                numberChoices[chooseNumber].Add(player);
                if (chooseNumber == 0)
                {
                    playersWhoChoseZero.Add(player);
                }
                else if (chooseNumber == 100)
                {
                    playersWhoChoseHundred.Add(player);
                }
            }
        }
        int totalPlayers = PhotonNetwork.PlayerList.Length; // ��ü �÷��̾� ��
        int playersWhoChoseNumber = 0; // ���ڸ� ������ �÷��̾� ��
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("ChooseNumber", out object chooseNumberObj) && chooseNumberObj is int chooseNumber && chooseNumber >= 0)
            {
                // ��ȿ�� ���ڸ� ������ ��츸 ī��Ʈ
                playersWhoChoseNumber++;
            }
        }
        bool allChoseSameNumber = numberChoices.Count == 1 && playersWhoChoseNumber == totalPlayers;
        bool zeroAndHundredOnly = numberChoices.ContainsKey(0) && numberChoices.ContainsKey(100) && numberChoices.Keys.Count == 2;
        if (count == 0)
        {
            PlayerMovement.isPositionFixed = false;

            // ��� �÷��̾ ���ڸ� �������� �ʾ����Ƿ� End Game Sequence�� ����
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("StartEndGameSequence", RpcTarget.All);
            }
                yield break; // �ڷ�ƾ ����
        }
        float average = count > 0 ? total / count : 0;
        float resultNumber = average * 0.8f; // ���� ����� 0.8��

        numberChoices = numberChoices.Where(kvp => kvp.Value.Count == 1).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        Player closestPlayer = null;
        float closestDistance = float.MaxValue;
        bool exactMatch = false;
        foreach (var entry in numberChoices)
        {
            float distance = Mathf.Abs(entry.Key - resultNumber);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = entry.Value.First();
            }

            if (Mathf.RoundToInt(resultNumber) == Mathf.RoundToInt(entry.Key) && entry.Value.Count == 1)
            {
                exactMatch = true;
            }
        }


        // ���� ����� ����� �÷��̾� ã��
        if (allChoseSameNumber)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Playername[i].text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("ChooseNumber", out object chooseNumberObj))
                {
                    Playernumber[i].text = chooseNumberObj is int chooseNumber ? chooseNumber.ToString() : "N/A";
                }
                else
                {
                    Playernumber[i].text = "N/A";
                }
            }
            resultnumber.text = "���� ����";
            resultnotice.text = "��ΰ� ���� ���ڸ� ����";


        }
        else if (zeroAndHundredOnly)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Playername[i].text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("ChooseNumber", out object chooseNumberObj))
                {
                    Playernumber[i].text = chooseNumberObj is int chooseNumber ? chooseNumber.ToString() : "N/A";
                }
                else
                {
                    Playernumber[i].text = "N/A";
                }
            }
            resultnumber.text = "100";
            resultnotice.text = "0�� 100�� ������ �÷��̾ ����, 100�� ������ �÷��̾� �¸�";

        }
        else
        {
            if (closestPlayer != null)
            {
                resultnumber.text = resultNumber.ToString("F0");
                if (closestPlayer != null)
                {
                    resultnotice.text = $"��� ���� {average:F2} * 0.8 = {resultNumber:F2}�� \n��տ��� ���� �����, {closestPlayer.NickName} �¸�";
                }
                else
                {
                    resultnotice.text = "���� ����";
                }
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Playername[i].text = PhotonNetwork.PlayerList[i].NickName;
                    if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("ChooseNumber", out object chooseNumberObj))
                    {
                        Playernumber[i].text = chooseNumberObj is int chooseNumber ? chooseNumber.ToString() : "N/A";
                    }
                    else
                    {
                        Playernumber[i].text = "N/A";
                    }
                }
            }
        }
       

        yield return new WaitForSeconds(8);

        Round1Result.SetActive(false);
        if (zeroAndHundredOnly)
        {
            List<Player> winners = playersWhoChoseHundred.ToList();
            DecreaseHealthForAllExceptWinner(winners, false);

        }
        else if (allChoseSameNumber)
        {
            // ��� �÷��̾ ���� ���ڸ� ������ ���
            foreach (var player in PhotonNetwork.PlayerList)
            {
                // ��� �÷��̾��� ü�� 1 ����
                DecreaseHealthForAllExceptWinner(new List<Player>(), false);
            }
        }
        else
        {
            if (closestPlayer != null)
            {
                DecreaseHealthForAllExceptWinner(new List<Player> { closestPlayer }, exactMatch);
            }
        }
       

        Round1Question.SetActive(true);
        foreach (GameObject title in RoundTitles)
        {
            title.SetActive(false);
        }
        RoundTitles[round - 1].SetActive(true);
        timerText.gameObject.SetActive(true);
        foreach (GameObject textObject in Round1Text)
        {
            textObject.SetActive(true);

        }
        Roundwait.SetActive(false);


    }
    void AssignNewMasterClient3()
    {
        if ((int)PhotonNetwork.MasterClient.CustomProperties["Health"] <= 0 || (int)PhotonNetwork.MasterClient.CustomProperties["Scene1order"] != 3)
        {
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Player player in players)
            {
                if ((int)player.CustomProperties["Health"] > 0 && (int)player.CustomProperties["Scene1order"] == 3)
                {
                    PhotonNetwork.SetMasterClient(player);
                    break;
                }
            }
        }
    }
    private IEnumerator PrepareForRound()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "ChooseNumber", null } });
        yield return null;
    }
    void DecreaseHealthForAllExceptWinner(List<Player> winners, bool exactMatch)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            List<Player> lowestHealthPlayers = new List<Player>();
            int lowestHealth = int.MaxValue;

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!winners.Contains(player))
                {
                    int healthDecreaseAmount = exactMatch ? 2 : 1;
                    int currentHealth = player.CustomProperties.ContainsKey("Health") ? (int)player.CustomProperties["Health"] : 3;
                    int newHealth = Mathf.Max(currentHealth - healthDecreaseAmount, 0);

                    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { "Health", newHealth } };
                    if (newHealth < lowestHealth)
                    {
                        lowestHealth = newHealth;
                        lowestHealthPlayers.Clear();
                        lowestHealthPlayers.Add(player);
                    }
                    else if (newHealth == lowestHealth)
                    {
                        lowestHealthPlayers.Add(player);
                    }

                    player.SetCustomProperties(props);
                }
            }
                if (lowestHealthPlayers.Count > 0 && lowestHealth <= 0)
                {
                    int index = UnityEngine.Random.Range(0, lowestHealthPlayers.Count);
                    Player selectedPlayer = lowestHealthPlayers[index];

                    string personality = selectedPlayer.CustomProperties["Personality"] as string;
                    string nickname = selectedPlayer.NickName as string;
                    PlayerMovement.isPositionFixed = false;


                    deathPlayersInfo.Add("��Ī " + personality + " " + nickname + ",\n");
                        string deathPlayersStr = string.Join("\n", deathPlayersInfo);
                        PV.RPC("UpdateDeathPlayersInfo", RpcTarget.AllBuffered, deathPlayersStr);



                        PV.RPC("StartEndGameSequence", RpcTarget.All);
                    
                }
            }
        
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // ü�� ������ ����Ǿ����� Ȯ���մϴ�.
        if (changedProps.ContainsKey("Health"))
        {
            // ����� ü�� ���� �����ɴϴ�.
            int updatedHealth = (int)changedProps["Health"];

            // ���� ���� �÷��̾��� ü�� ������ ������Ʈ�մϴ�.
            if (targetPlayer == PhotonNetwork.LocalPlayer)
            {
                PlayerHealth.text = $"���� ü�� : {updatedHealth}";
            }
        }
    }
}