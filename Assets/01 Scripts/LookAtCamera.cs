using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LookAtCamera : MonoBehaviour
{
    public Transform characterBody;
    public Transform m_Target;

    public float distanceFromTarget = 3.0f;
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 5f; // �¿� ȸ�� �ӵ�
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�
    public string fireButtonName = "Fire1"; // �߻縦 ���� �Է� ��ư �̸�
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�


    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        LookAround();
        Move();
        move = Input.GetAxis(moveAxisName);
        // rotate�� ���� �Է� ����
        rotate = Input.GetAxis(rotateAxisName);
        // fire�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName);

    }
    private void FixedUpdate()
    {
        // ȸ�� ����
        Rotate();
        // ������ ����
        Move();

        // �Է°��� ���� �ִϸ������� Move �Ķ���� ���� ����
        playerAnimator.SetFloat("Move", playerInput.move);
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = m_Target.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if ( x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        m_Target.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);//camAngle.x + mouseDelta.y �̰� ���� �÷����� ������ �Ʒ����� �ٲٴ� �ڵ�

    }
    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            Vector3 lookForward = new Vector3(m_Target.forward.x, 0f, m_Target.forward.z).normalized;
            Vector3 lookRight = new Vector3(m_Target.right.x, 0f, m_Target.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * moveSpeed;

        }



        // ��������� �̵��� �Ÿ� ���
        //Vector3 moveDistance =
        //   playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // ������ٵ� ���� ���� ������Ʈ ��ġ ����
        //playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
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