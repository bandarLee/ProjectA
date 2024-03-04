using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Scene4Monster : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 4f;
    private float rushSpeed = 15f;
    public int health = 5; // 몬스터의 초기 체력
    public GameObject WaitImage;

    private Vector3 rushDirection;


    private bool isRushing = false;
    public enum Scene4MonsterState
    {
        Idle,
        Search,
        Trace,
        Rush,
        Damaged,
        Die
    }

    private Scene4MonsterState currentState = Scene4MonsterState.Idle;
    public GameObject targetPlayer = null;
    public enum Scene4MonsterFloor
    {
        Second,
        Third
    }
    public Scene4MonsterFloor currentFloor = Scene4MonsterFloor.Second;

    private new void OnEnable()
    {
        StartCoroutine(TimerCoroutine(10));
    }

    void Update()
    {
        Quaternion currentrotation = transform.rotation;
        transform.rotation = Quaternion.Euler(new Vector3(-179.983f, transform.rotation.y, 180.526f));
        if (currentFloor == Scene4MonsterFloor.Second)
        {

            Vector3 currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, 8.93f, currentPosition.z);
        }
        if (currentFloor == Scene4MonsterFloor.Third)
        {
            Vector3 currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, 28.71f, currentPosition.z);
        }
        switch (currentState)
        {
            case Scene4MonsterState.Trace:
                TracePlayer();
                break;
            case Scene4MonsterState.Rush:
                RushTowardsPlayer();
                break;
            case Scene4MonsterState.Search:
                FindNearestPlayer();
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {
            photonView.RPC("TakeDamage", RpcTarget.All, 1);
        }
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(waitForPeopleDieTime(other.gameObject)); 
        }
    }
    void TracePlayer()
    {
        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(targetPlayer.transform.position, transform.position);
            if (distanceToPlayer <= 3f && !isRushing)
            {
                transform.position = targetPlayer.transform.position + targetPlayer.transform.forward * 10;
                rushDirection = (targetPlayer.transform.position - transform.position).normalized;
                LookAtTarget(targetPlayer.transform.position);
                isRushing = true;
                currentState = Scene4MonsterState.Rush;
            }
            else if (!isRushing)
            {
                MoveTowardsPlayer(targetPlayer);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
    void MoveTowardsPlayer(GameObject player)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        LookAtTarget(player.transform.position);
    }
    void RushTowardsPlayer()
    {
        if (!isRushing) return;

        LookAtTarget(targetPlayer.transform.position);

        transform.position += rushDirection * rushSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPlayer.transform.position) > 10f)
        {
            isRushing = false;
            StartCoroutine(RushCooldown());
        }
    }

    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        float yThreshold = 20f;

        foreach (GameObject player in players)
        {
            float playerY = player.transform.position.y;
            bool isEligible = (currentFloor == Scene4MonsterFloor.Second && playerY <= yThreshold) ||
                              (currentFloor == Scene4MonsterFloor.Third && playerY > yThreshold);

            if (isEligible)
            {
                PhotonView pv = player.GetComponent<PhotonView>();
                if (pv != null && pv.Owner.CustomProperties.ContainsKey("Status"))
                {
                    int playerStatus = (int)pv.Owner.CustomProperties["Status"];
                    if (playerStatus == 2 || playerStatus == 1) continue;
                }
                float distance = Vector3.Distance(player.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    nearestPlayer = player;
                    minDistance = distance;
                }
            }
        }
        targetPlayer = nearestPlayer;

        if (targetPlayer != null)
        {
            currentState = Scene4MonsterState.Trace;
        }
        else
        {
            currentState = Scene4MonsterState.Idle;
        }
    }

    IEnumerator RushCooldown()
    {
        yield return new WaitForSeconds(2f);
        currentState = Scene4MonsterState.Trace;
        isRushing = false;
    }
    IEnumerator TimerCoroutine(int duration)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(duration);
            currentState = Scene4MonsterState.Trace;
        }
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (currentState == Scene4MonsterState.Die) return; 
        health -= damage;

        if (health <= 0)
        {
            currentState = Scene4MonsterState.Die;
            photonView.RPC("RequestMonsterDeath", RpcTarget.MasterClient);
        }
        else
        {
            currentState = Scene4MonsterState.Damaged;
            StartCoroutine(DamageEffectCoroutine());
        }
    }
    [PunRPC]

    void RequestMonsterDeath()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }
    [PunRPC]
    public void ChangeStateToSearch()
    {

        currentState = Scene4MonsterState.Search;
        isRushing = false;
    }
    IEnumerator DamageEffectCoroutine()
    {
        moveSpeed = 1f;


        yield return new WaitForSeconds(1f);


        moveSpeed = 4f;
        isRushing = false;

        currentState = Scene4MonsterState.Trace;




    }
    private IEnumerator waitForPeopleDieTime(GameObject playerHit)
    {

        { 
       
            UpdatePlayerStatus(playerHit, 2); 
            PhotonView photonview = playerHit.GetComponent<PhotonView>();

            if (photonview.IsMine)
            {
                PlayerMovement.isPositionFixed = true;
                WaitImage.SetActive(true);

                StartCoroutine(RotatePlayerOverTime(playerHit.gameObject, Quaternion.Euler(-86f, -127f, 0f), 3));

            }
            yield return null;

        }
        Scene4Manager.instance.CheckPlayersAndLoadScene();


    }
    void UpdatePlayerStatus(GameObject player, int status)
    {
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "Status", status }
        };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }
    IEnumerator RotatePlayerOverTime(GameObject player, Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = player.transform.rotation;
        float time = 0f;

        while (time < duration)
        {
            player.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.transform.rotation = targetRotation;
    }
}
