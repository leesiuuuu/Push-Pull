using UnityEngine;
using UnityEngine.UI;

public class ExChargeUi : MonoBehaviour
{
	Canvas canvas;

	public NewPlayer1 player1;
	public NewPlayer2 player2;

	public int player;

	public Image PChargeGage;
	public Image PGage;

	public GameObject PGrabRange;

	[Header("SkillObjects")]
	public GameObject PushGlove;
	public GameObject PlayerGrab;

	private Vector3 scale;


	Vector3 offset = new Vector3(0, 120, 0);
	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		canvas.worldCamera = Camera.main;
	}

	private void Start()
	{
		PGage.gameObject.SetActive(false);

		PGrabRange.SetActive(false);
	}

	private void Update()
	{
		PlayerUI();

		if(player == 1) PChargeGage.fillAmount = player1.pushCharge / 35f;
		else PChargeGage.fillAmount = player2.pushCharge / 35f;
	}

	void PlayerUI()
	{
		if (PushGlove.activeSelf)
		{
			if (!KeySetting.IsGamePad)
			{
				if(player == 1)
				{
					if (Input.GetKey(KeySetting.player1Keys[KeyAction.PUSH]))
					{
						PGage.gameObject.SetActive(true);
						Vector3 scale = PGage.rectTransform.localScale;
						if (transform.parent.localScale.x < 0)
						{
							PGage.rectTransform.localScale = new Vector3(-scale.y, scale.y, scale.z);
						}
						else
						{
							PGage.rectTransform.localScale = new Vector3(scale.y, scale.y, scale.z);
						}
					}
					else
					{
						PGage.gameObject.SetActive(false);
					}
				}
				else
				{
					if (Input.GetKey(KeySetting.player2Keys[KeyAction.PUSH]))
					{
						PGage.gameObject.SetActive(true);
						Vector3 scale = PGage.rectTransform.localScale;
						if (transform.parent.localScale.x < 0)
						{
							PGage.rectTransform.localScale = new Vector3(-scale.y, scale.y, scale.z);
						}
						else
						{
							PGage.rectTransform.localScale = new Vector3(scale.y, scale.y, scale.z);
						}
					}
					else
					{
						PGage.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				if(player == 1)
				{
					if (Input.GetButton("Push1"))
					{
						PGage.gameObject.SetActive(true);
						Vector3 scale = PGage.rectTransform.localScale;
						if (transform.parent.localScale.x < 0)
						{
							PGage.rectTransform.localScale = new Vector3(-scale.y, scale.y, scale.z);
						}
						else
						{
							PGage.rectTransform.localScale = new Vector3(scale.y, scale.y, scale.z);
						}
					}
					else
					{
						PGage.gameObject.SetActive(false);
					}
				}
				else
				{
					if (Input.GetButton("Push2"))
					{
						PGage.gameObject.SetActive(true);
						Vector3 scale = PGage.rectTransform.localScale;
						if (transform.parent.localScale.x < 0)
						{
							PGage.rectTransform.localScale = new Vector3(-scale.y, scale.y, scale.z);
						}
						else
						{
							PGage.rectTransform.localScale = new Vector3(scale.y, scale.y, scale.z);
						}
					}
					else
					{
						PGage.gameObject.SetActive(false);
					}
				}

			}
		}

		if (PlayerGrab.activeSelf)
		{
			if (!KeySetting.IsGamePad)
			{
				if(player == 1)
				{
					if (Input.GetKey(KeySetting.player1Keys[KeyAction.PULL]))
					{
						PGrabRange.SetActive(true);
					}
					else
					{
						PGrabRange.SetActive(false);
					}
				}
				else
				{
					if (Input.GetKey(KeySetting.player2Keys[KeyAction.PULL]))
					{
						PGrabRange.SetActive(true);
					}
					else
					{
						PGrabRange.SetActive(false);
					}
				}
			}
			else
			{
				if(player == 1)
				{
					if (Input.GetButton("Pull1"))
					{
						PGrabRange.SetActive(true);
					}
					else
					{
						PGrabRange.SetActive(false);
					}
				}
				else
				{
					if (Input.GetButton("Pull2"))
					{
						PGrabRange.SetActive(true);
					}
					else
					{
						PGrabRange.SetActive(false);
					}
				}
			}
		}

	}
}
