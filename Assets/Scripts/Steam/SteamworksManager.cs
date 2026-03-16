using Steamworks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class SteamworksManager : SingleMono<SteamworksManager>
{
    public EndPointSO EndpointSO;
    public bool IsInitialized { get; private set; } = false;

    private Callback<LobbyCreated_t> CALLBACK_lobbyCreated;


    private string roomNameTemp;
    private string roomPasswordTemp;

    protected override void Awake()
    {
        base.Awake();
        InitSteam(true);
    }

    private void OnApplicationQuit()
    {
        Logout().Forget();
    }

    private void InitSteam(bool isLogin = false)
    {
        try
        {
            if (!SteamManager.Initialized)
            {
                if (!SteamAPI.Init())
                {
                    Debug.LogError("[Steam] 초기화 실패: 스팀 클라이언트를 확인하세요.");
                    return;
                }
            }
            IsInitialized = true;
            Debug.Log("[Steam] Initialized Successfully");

            // 초기화 완료. 로그인을 실행합니다.
            if(isLogin) Login().Forget();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Steam] 초기화 중 예외: {e.Message}");
        }
    }

    #region 방 생성/입장 함수
    
    public async UniTask CreateRoom(string roomName, bool isPrivate, string password)
    {
        if (!IsInitialized) InitSteam();

        roomNameTemp = roomName;
        roomPasswordTemp = password;

        ELobbyType lobbyType = isPrivate ? ELobbyType.k_ELobbyTypePrivate : ELobbyType.k_ELobbyTypePublic;

        Debug.Log($"[Steam] 로비 생성 요청 중... (이름: {roomName})");

        CSteamID lobbyID = await CreateLobbyAsync(lobbyType);

        if (lobbyID == CSteamID.Nil)
        {
            Debug.LogError("[Steam] 로비 생성 실패");
            return;
        }

        // 로비 메타데이터 설정
        SteamMatchmaking.SetLobbyData(lobbyID, "name", roomName);

        if (!string.IsNullOrEmpty(password))
        {
            SteamMatchmaking.SetLobbyData(lobbyID, "isPasswordProtected", "true");
            SteamMatchmaking.SetLobbyData(lobbyID, "password", password);
        }
        else
        {
            SteamMatchmaking.SetLobbyData(lobbyID, "isPasswordProtected", "false");
        }

        SteamMatchmaking.SetLobbyData(lobbyID, "HostSteamID", SteamUser.GetSteamID().ToString());

        Debug.Log($"[Steam] 로비 생성 성공! ID: {lobbyID}");

    }

    private UniTask<CSteamID> CreateLobbyAsync(ELobbyType lobbyType)
    {
        var completionSource = new UniTaskCompletionSource<CSteamID>();

        CallResult<LobbyCreated_t> callResult = new CallResult<LobbyCreated_t>();

        callResult.Set(
            SteamMatchmaking.CreateLobby(lobbyType, 2),
            (result, bIOFailture) =>
            {
                callResult.Dispose();

                if (bIOFailture || result.m_eResult != EResult.k_EResultOK)
                {
                    Debug.LogError($"[Steam] 로비 생성 실패: {result.m_eResult}");
                    completionSource.TrySetResult(CSteamID.Nil);
                    return;
                }

                completionSource.TrySetResult(new CSteamID(result.m_ulSteamIDLobby));
            }
        );

        return completionSource.Task;
    }

    #endregion

    #region 로그인/로그아웃 함수

    private async UniTask Login()
    {
        if (!IsInitialized) InitSteam();

        LoginRequest body = new LoginRequest
        {
            steamTicket = GetAuthSessionTicket(),
            nickName = GetPersonaName()
        };

        Debug.Log(body.steamTicket);

        try
        {
            var result = await APIConnector.Post<Response<string>>(EndpointSO.Login, body);
            OnLoginSuccess(result.Data);
        }
        catch (Exception e)
        {
            OnLoginFailed(e.Message);
        }
    }

    private async UniTask Logout()
    {
        if (!IsInitialized) InitSteam();

        try
        {
            var result = await APIConnector.Post<Response<string>>(EndpointSO.Logout, null, false);

            // 로그아웃 성공 시 출력
            Debug.Log("로그아웃 완료");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion

    #region 로그인 성공/실패 함수
    private void OnLoginSuccess(string data)
    {
        Debug.Log($"Session ID : {data}");
        PlayerPrefs.SetString("sessionId", data);
    }

    private void OnLoginFailed(string log)
    {
        Debug.Log(log);
    }

    #endregion

    #region 스팀 데이터 관련 함수

    // private CSteamID GetSteamID() => SteamUser.GetSteamID();
    private string GetPersonaName() => SteamFriends.GetPersonaName();
    private string GetAuthSessionTicket()
    {
        byte[] ticketBuffer = new byte[1024];
        uint ticketSize;
        SteamNetworkingIdentity identity = default;

        HAuthTicket authTicket = SteamUser.GetAuthSessionTicket(ticketBuffer, ticketBuffer.Length, out ticketSize, ref identity);

        byte[] ticket = new byte[ticketSize];

        // 티켓 데이터 복사
        System.Array.Copy(ticketBuffer, ticket, ticketSize);

        // Hex 문자열로 변환
        string ticketHex = System.BitConverter.ToString(ticket).Replace("-", "");
        return ticketHex;
    }
    #endregion
}