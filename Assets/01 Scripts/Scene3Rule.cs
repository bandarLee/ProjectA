using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scene3Rule : MonoBehaviour
{
    public GameObject PressE;
    public GameObject Sit;
    public GameObject Sit2;
    public GameObject HiddenRule1;
    public GameObject HiddenRule2;
    public GameObject HiddenRule3;
    public GameObject HiddenRule4;
    public GameObject SpecialRule1;
    public GameObject SpecialRule2;
    public GameObject SpecialRule3;


    void Start()
    {
        PressE.SetActive(false);
        Sit.SetActive(true);
        Sit2.SetActive(false);
        HiddenRule1.SetActive(false);
        HiddenRule2.SetActive(false);
        HiddenRule3.SetActive(false);
        HiddenRule4.SetActive(false);
        SpecialRule1.SetActive(false);
        SpecialRule2.SetActive(false);
        SpecialRule3.SetActive(false);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (gameObject.name)
            {
                case "Chair1":
                case "Chair2":
                case "Chair3":
                case "Chair4":
                case "Chair5":
                case "Chair6":
                    Sit.SetActive(false);
                    Sit2.SetActive(true);
                    break;
                case "Rule1":
                case "Rule2":
                case "Rule3":
                case "Rule4":
                case "HiddenRule1":
                case "HiddenRule2":
                case "HiddenRule3":
                    Sit.SetActive(false);
                    PressE.SetActive(true);
                    break;
                default:
                    break;
            }

            if (Input.GetKey(KeyCode.E))
            {
                switch (gameObject.name)
                {
                    case "Chair1":
                    case "Chair2":
                    case "Chair3":
                    case "Chair4":
                    case "Chair5":
                    case "Chair6":
                        SitDecision();
                        collision.gameObject.transform.position = transform.position;
                        collision.gameObject.transform.LookAt(GameObject.Find("JudgeLeg").transform);

                        break;
                    case "Rule1":
                        HiddenRule1.SetActive(true);
                        break;
                    case "Rule2":
                        HiddenRule2.SetActive(true);
                        break;
                    case "Rule3":
                        HiddenRule3.SetActive(true);
                        break;
                    case "Rule4":
                        HiddenRule4.SetActive(true);
                        break;
                    case "HiddenRule1":
                        SpecialRule1.SetActive(true);
                        break;
                    case "HiddenRule2":
                        SpecialRule2.SetActive(true);
                        break;
                    case "HiddenRule3":
                        SpecialRule3.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
        if (Scene3Timer.RuleDescriptEnd)
        {
            PressE.SetActive(false);
            Sit.SetActive(false);
            Sit2.SetActive(false);
            HiddenRule1.SetActive(false);
            HiddenRule2.SetActive(false);
            HiddenRule3.SetActive(false);
            HiddenRule4.SetActive(false);
            SpecialRule1.SetActive(false);
            SpecialRule2.SetActive(false);
            SpecialRule3.SetActive(false);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        PressE.SetActive(false);
        Sit.SetActive(true);
        Sit2.SetActive(false);
        HiddenRule1.SetActive(false);
        HiddenRule2.SetActive(false);
        HiddenRule3.SetActive(false);
        HiddenRule4.SetActive(false);
        SpecialRule1.SetActive(false);
        SpecialRule2.SetActive(false);
        SpecialRule3.SetActive(false);

    }
    void Update()
    {

    }
    void SitDecision()
    {

        PlayerMovement.isPositionFixed = true;
    }
}
