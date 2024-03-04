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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<UIManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    public Text [] NicknameTexts;
    private static UIManager m_instance; // �̱����� �Ҵ�� static ����

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
