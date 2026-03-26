using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    // 스팀웍스 닷넷 전용 콜백(이벤트) 변수들
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start()
    {
        // 스팀이 정상적으로 켜지지 않았다면 로비 기능도 중지
        if (!SteamManager.Initialized) { return; }

        // 스팀에서 이벤트가 발생했을 때 실행할 함수들 연결
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // [방 파기 버튼]을 누르면 실행될 함수
    public void HostSteamLobby()
    {
        Debug.Log("스팀 로비 생성 요청 중...");
        // 친구 전용 로비 생성 (인원수는 미러 매니저 세팅 따라감)
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, NetworkManager.singleton.maxConnections);
    }

    // 방이 성공적으로 만들어졌을 때
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("방 생성 실패!");
            return;
        }

        Debug.Log("방 생성 완료! 친구 초대를 해보세요.");

        // 미러(Mirror) 엔진에게 방장(Host)으로 켜라고 명령
        NetworkManager.singleton.StartHost();

        // 방 정보에 내 스팀 ID를 메모해둠 (손님들이 찾아올 목적지)
        CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(lobbyId, "HostAddress", SteamUser.GetSteamID().ToString());
    }

    // 친구가 스팀 오버레이에서 [게임 참가]를 눌렀을 때
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("친구가 초대를 수락함! 방 입장 시도 중...");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    // 방에 성공적으로 입장했을 때
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        // 내가 방장이면 이미 호스트로 켜져 있으니까 패스!
        if (NetworkManager.singleton.isNetworkActive) { return; }

        Debug.Log("친구 방 접속 완료!! 미러 클라이언트 연결 중...");

        // 방장의 스팀 ID를 읽어와서 목적지로 설정
        CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        string hostAddress = SteamMatchmaking.GetLobbyData(lobbyId, "HostAddress");

        NetworkManager.singleton.networkAddress = hostAddress;

        // 미러 엔진에게 손님(Client)으로 접속하라고 명령
        NetworkManager.singleton.StartClient();
    }
}