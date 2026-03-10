using System.Collections;
using Steamworks;
using UnityEngine;

public class SteamworksManager : SingleMono<SteamworksManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

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
}