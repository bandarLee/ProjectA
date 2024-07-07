using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 10f; // 회전 속도

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // X축(좌우) 회전 적용
        transform.Rotate(Vector3.up * mouseX);

        // Y축(상하) 회전은 선택적 적용
        // 카메라 또는 카메라 부모의 X축 회전을 조정하여 상하 회전을 제어할 수 있습니다.
        // 예를 들어, 카메라가 바라보는 각도에 제한을 두려면, 여기에 추가 로직을 구현합니다.
    }
}