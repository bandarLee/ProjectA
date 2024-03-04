using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviourPun
{
    public static float moveSpeed = 5f; // �յ� �������� �ӵ�
    private float rotateSpeed = 90f; // �¿� ȸ�� �ӵ�
    public PhotonView PV; // PhotonView ������Ʈ�� ���� ����

    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    public static bool isPositionFixed { get; set; } = false;
    public GameObject playerHead; // PlayerHead�� Transform ������Ʈ�� �Ҵ�
    private float lastRotationAngle = 0f; // ������ ȸ�� ������ ����
    public bool attackKey = false;
    private bool isAttacking = false; // ���� ������ ���θ� ��Ÿ���� �÷���
    private bool isJumping = false; // ���� ������ ���θ� ��Ÿ���� �÷���

    public GameObject maceweapon; // PlayerHead�� Transform ������Ʈ�� �Ҵ�
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        maceweapon.GetComponentInChildren<BoxCollider>().enabled = false;
        maceweapon.SetActive(false);
    }
    private void Update()
    {
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!isPositionFixed)
            {
                

                Rotate();
            }

        }
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!isPositionFixed)
        {
            if (Input.GetKey(KeyCode.F10))
            {
                moveSpeed = 20f;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = 5f;
            }
            if (Input.GetMouseButton(0) && !isAttacking && attackKey)
            {
                StartCoroutine(AttackCoroutine());
            }
            if (Input.GetKey(KeyCode.Space) && !isJumping)
            {
                StartCoroutine(JumpCoroutine());
            }
            Move();
            HandleRotation(); 
            playerAnimator.SetFloat("Move", Mathf.Abs(playerInput.move) + Mathf.Abs(playerInput.rotate));
            

        }
        if (isPositionFixed)
        {
            playerAnimator.SetFloat("Move", 0);
            

        }



    }
    private void HandleRotation()
    {
        float currentRotation = 0f;

        if (!isPositionFixed)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                currentRotation = 45f; // �ϵ���
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                currentRotation = -45f; // ������
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                currentRotation = 45f; // ������
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                currentRotation = -45f; // �ϼ���
            }
            else if (Input.GetKey(KeyCode.D))
            {
                currentRotation = 90f; // ����
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentRotation = -90f; // ����
            }

            // ȸ���� �ʿ��� ���
            if (currentRotation != lastRotationAngle)
            {
                RotatePlayer(currentRotation - lastRotationAngle);
                lastRotationAngle = currentRotation;
            }

            // Ű�� ���� �ʱ� ���·� ����
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                RotatePlayer(-lastRotationAngle);
                lastRotationAngle = 0f; // ���� �ʱ�ȭ
            }
        }
    }

    private void RotatePlayer(float angle)
    {
        float currentY = transform.eulerAngles.y;
        Quaternion originalHeadRotation = playerHead.transform.rotation;

        transform.rotation = Quaternion.Euler(0, currentY + angle, 0);
        playerHead.transform.rotation = originalHeadRotation;

    }
    private void Move()
    {
        float moveinput = (Mathf.Abs(playerInput.move) + Mathf.Abs(playerInput.rotate));
        if (moveinput > 1)
            {
                moveinput = 1;
            }
        if (Input.GetKey(KeyCode.S))
            {
            moveinput = -moveinput;
            }
        Vector3 moveDistance = moveinput * transform.forward * moveSpeed * Time.deltaTime;


        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    

    }
    public IEnumerator AttackCoroutine()
    {
        maceweapon.GetComponentInChildren<BoxCollider>().enabled = true;

        isAttacking = true;
        playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);
        maceweapon.GetComponentInChildren<BoxCollider>().enabled = false;

        isAttacking = false;

    }
    public IEnumerator JumpCoroutine()
    {
        isJumping = true;
        playerAnimator.SetTrigger("Jump");

        yield return new WaitForSeconds(1.4f);
        isJumping = false;


    }

    public void Rotate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float turn = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float lookUp = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        transform.Rotate(0f, turn, 0f);
        playerHead.transform.Rotate(-lookUp, 0f, 0f);
        var headEuler = playerHead.transform.localEulerAngles;
        headEuler.x = (headEuler.x > 180) ? headEuler.x - 360 : headEuler.x; 
        headEuler.x = Mathf.Clamp(headEuler.x, -30f, 30f);
        playerHead.transform.localEulerAngles = headEuler;

    }

    [PunRPC]
    public void SetWeaponActive(bool isActive)
    {
        maceweapon.SetActive(isActive);
        attackKey = isActive; 
    }

    public void ChangeWeaponState(bool newState)
    {
        photonView.RPC("SetWeaponActive", RpcTarget.AllBuffered, newState);
    }

}
