using UnityEngine;
using Photon.Pun;

// �÷��̾� ĳ���͸� ����� �Է¿� ���� �����̴� ��ũ��Ʈ
public class PlayerMovement : MonoBehaviourPun
{
    public static float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 90f; // �¿� ȸ�� �ӵ�

    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    public static bool isPositionFixed { get; set; } = false;


    private void Start()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!isPositionFixed)
        {
            Rotate();
            Move();
        }
        // �Է°��� ���� �ִϸ������� Move �Ķ���� ���� ����
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // �Է°��� ���� ĳ���͸� �յڷ� ������
    private void Move()
    {
        // ��������� �̵��� �Ÿ� ���
        Vector3 moveDistance =
            playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // ������ٵ� ���� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // �Է°��� ���� ĳ���͸� �¿�� ȸ��
    private void Rotate()
    {
        // ��������� ȸ���� ��ġ ���
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // ������ٵ� ���� ���� ������Ʈ ȸ�� ����
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
}