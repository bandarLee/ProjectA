using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<UIManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    public Text [] NicknameTexts;
    private static UIManager m_instance; // 싱글톤이 할당될 static 변수

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void UpdateMemberList(string member)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            NicknameTexts[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

}
