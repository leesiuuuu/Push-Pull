using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    // 자식 참조
    private InputPlayer inputPlayer;
    private Grab grab;
    private Rigidbody2D childRb;
    private UnityEngine.InputSystem.PlayerInput childPlayerInput;

=    private Animator anim;

    // ── SyncVar ──────────────────────────────────

    [SyncVar(hook = nameof(OnFlipChanged))]
    private bool syncFlip = false;

    [SyncVar(hook = nameof(OnGlovePosChanged))]
    private Vector3 syncGloveLocalPos;

    // ─────────────────────────────────────────────

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputPlayer = GetComponentInChildren<InputPlayer>();
        grab = GetComponentInChildren<Grab>();

        if (inputPlayer != null)
        {
            childRb = inputPlayer.GetComponent<Rigidbody2D>();
            childPlayerInput = inputPlayer.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        }
    }

    // ───────────────────────────────────────────
    // Mirror 콜백 - 로컬/원격 초기화
    // ───────────────────────────────────────────

    public override void OnStartLocalPlayer()
    {
=        if (childPlayerInput != null) childPlayerInput.enabled = true;
        if (childRb != null) childRb.isKinematic = false;
        Debug.Log("[NetworkPlayer] 로컬 플레이어 초기화 완료");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer)
        {
            if (childPlayerInput != null) childPlayerInput.enabled = false;
            if (childRb != null) childRb.isKinematic = true;
            Debug.Log("[NetworkPlayer] 원격 플레이어 초기화 완료");
        }
    }

    // ───────────────────────────────────────────
    // 플립 동기화
    // ───────────────────────────────────────────

    private void OnFlipChanged(bool oldVal, bool newVal)
    {
        if (isLocalPlayer) return;
        ApplyFlipToChild(newVal);
    }

    public void SyncFlip(bool isFlipped)
    {
        ApplyFlipToChild(isFlipped);
        CmdSyncFlip(isFlipped);
    }

    private void ApplyFlipToChild(bool isFlipped)
    {
        if (inputPlayer == null) return;
        Vector3 scale = inputPlayer.transform.localScale;
        scale.x = isFlipped ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        inputPlayer.transform.localScale = scale;
    }

    [Command]
    private void CmdSyncFlip(bool isFlipped)
    {
        syncFlip = isFlipped;
    }

    // ───────────────────────────────────────────
    // 애니메이션 동기화
    // ───────────────────────────────────────────

    public void PlayAnimLocal(string animName)
    {
        if (inputPlayer == null) return;

        if (InputPlayer.GloveAnimStates.Contains(animName))
            inputPlayer.GloveAnim?.Play(animName);
        else
            anim?.Play(animName);
    }

    public void SyncAnim(string animName)
    {
        CmdPlayAnimation(animName);
    }

    [Command]
    private void CmdPlayAnimation(string animName)
    {
        RpcPlayAnimation(animName);
    }

    [ClientRpc]
    private void RpcPlayAnimation(string animName)
    {
        if (isLocalPlayer) return;
        PlayAnimLocal(animName);
    }

    // ───────────────────────────────────────────
    // 글러브 위치 동기화
    // ───────────────────────────────────────────

    private void OnGlovePosChanged(Vector3 oldVal, Vector3 newVal)
    {
        if (isLocalPlayer) return;
        if (grab != null)
            grab.transform.localPosition = newVal;
    }

    public void SyncGlovePos(Vector3 localPos)
    {
        CmdUpdateGlovePos(localPos);
    }

    [Command]
    private void CmdUpdateGlovePos(Vector3 localPos)
    {
        syncGloveLocalPos = localPos;
    }

    // ───────────────────────────────────────────
    // 상대 플레이어 끌기
    // ───────────────────────────────────────────

    public void SyncMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        CmdMoveTarget(targetNetId, targetPos);
    }

    [Command]
    private void CmdMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        RpcMoveTarget(targetNetId, targetPos);
    }

    [ClientRpc]
    private void RpcMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        if (NetworkServer.spawned.TryGetValue(targetNetId, out NetworkIdentity identity))
            identity.transform.position = targetPos;
    }

    // ───────────────────────────────────────────
    // 밀치기
    // ───────────────────────────────────────────

    public void SyncApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        CmdApplyPush(targetNetId, dir, power);
    }

    [Command]
    private void CmdApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        RpcApplyPush(targetNetId, dir, power);
    }

    [ClientRpc]
    private void RpcApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        if (!NetworkServer.spawned.TryGetValue(targetNetId, out NetworkIdentity identity)) return;
        if (identity.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
        {
            rigid.AddForce(dir * power, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up * power / 2f, ForceMode2D.Impulse);
        }
    }
}