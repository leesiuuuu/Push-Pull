using UnityEngine;
using Steamworks; // Facepunch.Steamworks 라이브러리를 설치하면 쓸 수 있어!

public class SteamManager : MonoBehaviour
{
    void Start()
    {
        try
        {
            // 480(스페이스 워) 아이디로 스팀 초기화! (steam_appid.txt 역할도 알아서 해줌)
            SteamClient.Init(480);

            // 성공하면 콘솔창에 내 스팀 닉네임이 뜰 거야!
            Debug.Log("스팀 연결 성공! 접속한 유저: " + SteamClient.Name);
        }
        catch (System.Exception e)
        {
            // 스팀 클라이언트가 안 켜져있거나 오류가 나면 이쪽으로 빠짐
            Debug.LogError("스팀 연결 실패... 스팀이 켜져있나요? 원인: " + e.Message);
        }
    }

    void Update()
    {
        // 매 프레임마다 스팀에서 오는 연락(콜백)이 있는지 확인해주는 필수 코드
        SteamClient.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        // 게임이 꺼질 때 스팀 연결도 깔끔하게 끊어주기
        SteamClient.Shutdown();
    }
}