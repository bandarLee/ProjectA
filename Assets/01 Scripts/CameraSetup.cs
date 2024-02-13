using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CameraSetup : MonoBehaviourPun
{
    public Transform playerHead1; // Playerhead1�� �����ϱ� ���� ����
    public Transform playerHead2; // Playerhead2�� �����ϱ� ���� ����
    public Transform playerHead3; // Playerhead2�� �����ϱ� ���� ����

    void Start()
    {
        if (photonView.IsMine)
        {
            {
                CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

                // ���� ���� �̸��� ���� ī�޶� ����
                string sceneName = SceneManager.GetActiveScene().name;
                if (sceneName == "Main")
                {
                    followCam.Follow = playerHead1;
                    followCam.LookAt = playerHead1;
                }
                else if (sceneName == "Scene1")
                {
                    followCam.Follow = playerHead2;
                    followCam.LookAt = playerHead2;
                }
                else if (sceneName == "Scene3")
                {
                    followCam.Follow = playerHead3;
                    followCam.LookAt = playerHead3;
                }
            }
        }

    }
    }
