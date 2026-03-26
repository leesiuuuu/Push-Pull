using UnityEngine;
using Mirror;
using Steamworks;
using Steamworks.Data;

public class SteamLobby : MonoBehaviour
{
    // 스팀 이벤트(콜백) 연결하기
    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }

    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
    }

    public async void HostSteamLobby()
    {
        Debug.Log("스팀 로비 생성 요청 중...");
        // 최대 2명까지 들어올 수 있는 방 생성
        var lobby = await SteamMatchmaking.CreateLobbyAsync(2);
        if (!lobby.HasValue)
        {
            Debug.LogError("방 생성 실패!");
            return;
        }
    }

    // 방이 성공적으로 만들어졌을 때 스팀이 알려주는 신호
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK) return;

        // 방을 '친구 전용'으로 설정
        lobby.SetFriendsOnly();
        // 손님들이 찾아올 수 있게 내 스팀 ID를 방 정보에 메모해둠
        lobby.SetData("HostAddress", SteamClient.SteamId.ToString());

        // 미러(Mirror) 엔진에게 방장(Host)으로 게임을 켜라고 명령
        NetworkManager.singleton.StartHost();
        Debug.Log("방 생성 완료! 친구 초대를 해보세요.");
    }

    // 친구가 스팀 오버레이에서 내 프로필 우클릭 -> [게임 참가]를 눌렀을 때
    private async void OnGameLobbyJoinRequested(Lobby lobby, SteamId friendId)
    {
        Debug.Log("친구가 초대를 수락함! 방 입장 시도 중...");
        RoomEnter joinedLobby = await lobby.Join();
        if (joinedLobby != RoomEnter.Success)
        {
            Debug.LogError("방 입장 실패...");
        }
    }

    // 방에 입장(접속) 완료했을 때
    private void OnLobbyEntered(Lobby lobby)
    {
        // 내가 방장이면 이미 게임을 켰으니까 패스
        if (NetworkManager.singleton.isNetworkActive) return;

        // 방장의 스팀 ID를 읽어와서 미러(Mirror)에 목적지 주소로 입력
        string hostAddress = lobby.GetData("HostAddress");
        NetworkManager.singleton.networkAddress = hostAddress;

        // 미러 엔진에게 손님(Client)으로 접속하라고 명령
        NetworkManager.singleton.StartClient();
        Debug.Log("친구 방 접속 완료!!");
    }
}