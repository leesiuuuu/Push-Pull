using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

public class PlayerID : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    private TMP_Text idText;
    private InputPlayer player;
    private Vector3 initialTextScale;
    private bool lastFacingLeft;

    private void Awake()
    {
        idText = GetComponent<TMP_Text>();
        player = GetComponentInParent<InputPlayer>();
        initialTextScale = transform.localScale;
    }

    public override void OnStartLocalPlayer()
    {
        if (!SteamManager.Initialized) return;

        string myName = SteamFriends.GetPersonaName();
        CmdSetPlayerName(myName);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (idText != null)
            idText.text = playerName;

        ApplyTextFlip(true);
    }

    [Command]
    private void CmdSetPlayerName(string newName)
    {
        playerName = newName;
    }

    private void OnNameChanged(string oldName, string newName)
    {
        if (idText != null)
            idText.text = newName;
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
