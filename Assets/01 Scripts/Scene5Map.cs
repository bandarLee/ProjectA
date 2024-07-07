using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene5Map : MonoBehaviour
{
    public GameObject maptriggertext;
    public GameObject mapimage1;
    public GameObject mapimage2;

    public enum Map
    {
        Map1,
        Map2
    }

    public Map map;
    private bool isPlayerInside = false;

    private void Awake()
    {
        mapimage1.SetActive(false);
        mapimage2.SetActive(false);
        maptriggertext.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E)) 
        {
            ToggleMapImage(); 
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateMapImages();
        }
    }

    private void ToggleMapImage()
    {
        if (map == Map.Map1)
        {
            mapimage1.SetActive(!mapimage1.activeSelf); 
        }
        else if (map == Map.Map2)
        {
            mapimage2.SetActive(!mapimage2.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isPlayerInside = true;
                maptriggertext.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isPlayerInside = false;
                maptriggertext.SetActive(false);
                mapimage1.SetActive(false);
                mapimage2.SetActive(false);
            }
        }
    }
    private void DeactivateMapImages()
    {
        mapimage1.SetActive(false);
        mapimage2.SetActive(false);
    }
}
