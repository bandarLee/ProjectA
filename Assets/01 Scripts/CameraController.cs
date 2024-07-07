using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 10f; // ȸ�� �ӵ�

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // X��(�¿�) ȸ�� ����
        transform.Rotate(Vector3.up * mouseX);

        // Y��(����) ȸ���� ������ ����
        // ī�޶� �Ǵ� ī�޶� �θ��� X�� ȸ���� �����Ͽ� ���� ȸ���� ������ �� �ֽ��ϴ�.
        // ���� ���, ī�޶� �ٶ󺸴� ������ ������ �η���, ���⿡ �߰� ������ �����մϴ�.
    }
}