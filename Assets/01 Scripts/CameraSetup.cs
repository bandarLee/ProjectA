using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CameraSetup : MonoBehaviourPun
{
    public Transform playerHead1; // Playerhead1를 참조하기 위한 변수
    public Transform playerHead2; // Playerhead2를 참조하기 위한 변수
    public Transform playerHead3; // Playerhead2를 참조하기 위한 변수

    void Start()
    {
        if (photonView.IsMine)
        {
            {
                CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

                // 현재 씬의 이름에 따라서 카메라를 설정
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
