using Steamworks;
using TMPro;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    public string playerName;

    [SerializeField] private TMP_Text idText;
    private InputPlayer player;
    private Vector3 initialTextScale;
    private bool lastFacingLeft;

    private void Awake()
    {
        player = GetComponentInParent<InputPlayer>();
        initialTextScale = transform.localScale;

        if (SteamManager.Initialized)
        {
            playerName = SteamFriends.GetPersonaName();
        }
        else
        {
            playerName = string.Empty;
            Debug.LogWarning("Steam is not initialized, so PlayerID could not read the local Steam ID.");
        }
    }

    private void Start()
    {
        if (idText != null)
        {
            idText.text = playerName;
        }

        ApplyTextFlip(force: true);
    }

    private void Update()
    {
        ApplyTextFlip();
    }

    private void ApplyTextFlip(bool force = false)
    {
        if (player == null) return;

        bool facingLeft = player.transform.localScale.x < 0f;
        if (!force && facingLeft == lastFacingLeft) return;

        lastFacingLeft = facingLeft;

        Vector3 textScale = initialTextScale;
        textScale.x = facingLeft ? -Mathf.Abs(initialTextScale.x) : Mathf.Abs(initialTextScale.x);
        transform.localScale = textScale;
    }
}
