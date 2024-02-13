using UnityEngine;
using Photon.Pun;
// �÷��̾� ĳ���͸� �����ϱ� ���� ����� �Է��� ����
// ������ �Է°��� �ٸ� ������Ʈ���� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviourPun
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�
    public string fireButtonName = "Fire1"; // �߻縦 ���� �Է� ��ư �̸�
    // �� �Ҵ��� ���ο����� ����
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�
 

    // �������� ����� �Է��� ����
    private void Update()
    {
        if (!photonView.IsMine) {  return; }
        // fire�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName);
        rotate = Input.GetAxis(rotateAxisName);

        move = Input.GetAxis(moveAxisName);

            

        
    }
}