using System.Collections;
using Steamworks;
using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System;

public class SteamworksManager : SingleMono<SteamworksManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    [ContextMenu("DDD")]
    public void LoginDDD()
    {
        Login().Forget();
    }

    public EndPointSO EndpointSO;

    public bool Init()
    {
        // 스팀 메니저가 초기화되지 않았을 시 초기화 하기
        if (!SteamManager.Initialized)
        {
            if (!SteamAPI.Init())
            {
                Debug.LogError("[Steam] 초기화 실패");
                Application.Quit();
                return false;
            }
            Debug.Log("[Steam] Initialized");
            return true;
        }

        Debug.Log("[Steam] Initialized");
        return true;
    }

    public async UniTask Login()
    {
        if (!SteamManager.Initialized) Init();


        LoginRequest body = new LoginRequest
        {
            steamTicket = GetAuthSessionTicket(),
            nickName = GetPersonaName()
        };

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
    private void OnLoginSuccess(string data)
    {
        Debug.Log($"Session ID : {data}");
        PlayerPrefs.SetString("sessionId", data);
    }

    private void OnLoginFailed(string log)
    {
        Debug.Log(log);
    }

    // private CSteamID GetSteamID() => SteamUser.GetSteamID();
    private string GetPersonaName() => SteamFriends.GetPersonaName();
    private string GetAuthSessionTicket()
    {
        byte[] ticketBuffer = new byte[1024];
        uint ticketSize;
        SteamNetworkingIdentity identity = new SteamNetworkingIdentity();

        HAuthTicket authTicket = SteamUser.GetAuthSessionTicket(ticketBuffer, ticketBuffer.Length, out ticketSize, ref identity);

        byte[] ticket = new byte[ticketSize];

        // 티켓 데이터 복사
        System.Array.Copy(ticketBuffer, ticket, ticketSize);

        // Hex 문자열로 변환
        string ticketHex = System.BitConverter.ToString(ticket).Replace("-", "");
        return ticketHex;
    }
}