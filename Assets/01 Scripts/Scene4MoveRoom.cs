using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scene4MoveRoom : MonoBehaviour
{
    public GameObject[] floorImages;
    public enum Door
    {
        Floor1,
        Floor2,
        Portal
    }
    public Door door;
    private void Awake()
    {
        foreach (GameObject floor in floorImages)
        {
            floor.SetActive(false);
        }
    }
    void Start()
    {

    }
    
    private void OnTriggerEnter(Collider Player)
    {
        Collider weaponCollider = Player.GetComponent<PlayerMovement>().maceweapon.GetComponentInChildren<BoxCollider>();
        weaponCollider.enabled = false;
        Vector3 Level2position = new Vector3(-39.1f, 8.945f, -82.767f);
        Vector3 Level3position = new Vector3(-39.1f, 28.809f, -82.767f);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        if (door == Door.Floor1)
        {

            StartCoroutine(ChangeFloor(0));

            Player.transform.position = Level2position;
            Player.transform.rotation = rotation;
            weaponCollider.enabled = true;


            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Scene4Manager.instance.Mummies[i].SetActive(true);
                Scene4Manager.instance.Mummies[i].GetComponent<Scene4Monster>().targetPlayer = players[i];

            }
        }
        else if (door == Door.Floor2)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                PhotonView pv = monster.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("ChangeStateToSearch", RpcTarget.All);
                }

            }
            StartCoroutine(ChangeFloor(1));

            Player.transform.position = Level3position;
            Player.transform.rotation = rotation;
            weaponCollider.enabled = true;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Scene4Manager.instance.Mummies2[i].SetActive(true);
                Scene4Manager.instance.Mummies2[i].GetComponent<Scene4Monster>().targetPlayer = players[i];

            }
        }

    }
    private IEnumerator ChangeFloor(int floor)
    {
        floorImages[floor].SetActive(true);
        yield return new WaitForSeconds(2);
        floorImages[floor].SetActive(false);

    }
}
