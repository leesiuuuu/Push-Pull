using Steamworks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class SteamworksManager : SingleMono<SteamworksManager>
{
    public EndPointSO EndpointSO;
    public bool IsInitialized { get; private set; } = false;
    protected override void Awake()
    {
        base.Awake();
        InitSteam();
    }

    private void OnApplicationQuit()
    {
        Logout().Forget();
    }

    private void InitSteam()
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
            Login().Forget();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Steam] 초기화 중 예외: {e.Message}");
        }
    }

    #region 

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