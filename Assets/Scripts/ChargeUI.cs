using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    Canvas canvas;

    Player1 player1;
    Player2 player2;

    public Image P1ChargeGage;
    public Image P1Gage;

    public Image P2ChargeGage;
    public Image P2Gage;

    public GameObject P1GrabRange;
    public GameObject P2GrabRange;

    [Header("SkillObjects")]
    public GameObject PushGlove1;
    public GameObject PushGlove2;

    public GameObject Player1Grab;
    public GameObject Player2Grab;


    Vector3 offset = new Vector3(0, 120, 0);
    private void Awake()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();

        player1 = GameObject.Find("Player1").GetComponent<Player1>();
        player2 = GameObject.Find("Player2").GetComponent<Player2>();

        P1ChargeGage = GameObject.Find("P1ChargeGage").GetComponent<Image>();
        P1Gage = GameObject.Find("P1Gage").GetComponent<Image>();

        P2ChargeGage = GameObject.Find("P2ChargeGage").GetComponent<Image>();
        P2Gage = GameObject.Find("P2Gage").GetComponent<Image>();

        P1GrabRange = GameObject.Find("P1GrabRange");
        P2GrabRange = GameObject.Find("P2GrabRange");

        PushGlove1 = GameObject.FindWithTag("Player1Glove");
        PushGlove2 = GameObject.FindWithTag("Player2Glove");

        Player1Grab = GameObject.FindWithTag("Player1Grab");
        Player2Grab = GameObject.FindWithTag("Player2Grab");
    }

    private void Start()
    {
        P1Gage.gameObject.SetActive(false);
        P2Gage.gameObject.SetActive(false);

        P1GrabRange.SetActive(false);
        P2GrabRange.SetActive(false);
    }

    private void Update()
    {
        Player1UI();
        Player2UI();

        Vector3 screenPos1 = Camera.main.WorldToScreenPoint(player1.transform.position);
        Vector3 screenPos2 = Camera.main.WorldToScreenPoint(player2.transform.position);

        Vector2 uiPos1, uiPos2;
        
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos1 + offset, Camera.main, out uiPos1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos2 + offset, Camera.main, out uiPos2);

        P1Gage.rectTransform.localPosition = uiPos1;
        P2Gage.rectTransform.localPosition = uiPos2;

        P1ChargeGage.fillAmount = player1.pushCharge / 35f;
        P2ChargeGage.fillAmount = player2.pushCharge / 35f;
    }

    void Player1UI()
    {
        //ÁÖ¸Ô UI
        if(PushGlove1.activeSelf)
        {
            if (Input.GetKey(KeyCode.E))
            {
                P1Gage.gameObject.SetActive(true);
            }
            else
            {
                P1Gage.gameObject.SetActive(false);
            }

        }

        //±×·¦ UI
        if(Player1Grab.activeSelf)
        {
            if (Input.GetKey(KeyCode.R))
            {
                P1GrabRange.SetActive(true);
            }
            else
            {
                P1GrabRange.SetActive(false);
            }
        }

    }

    void Player2UI()
    {
        //ÁÖ¸Ô UI
        if(PushGlove2.activeSelf)
        {
            if (Input.GetKey(KeyCode.Slash))
            {
                P2Gage.gameObject.SetActive(true);
            }
            else
            {
                P2Gage.gameObject.SetActive(false);
            }
        }


        //±×·¦ UI
        if(Player2Grab.activeSelf)
        {
            if (Input.GetKey(KeyCode.Period))
            {
                P2GrabRange.SetActive(true);
            }
            else
            {
                P2GrabRange.SetActive(false);
            }
        }

    }
}
