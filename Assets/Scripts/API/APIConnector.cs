using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

// API 메서드를 실행하고, 응답을 반환하는 클래스입니다.
public static class APIConnector
{
    private static string baseUrl = "http://gsmsv-1.yujun.kr:21754/api/v1/";
    
    public static UniTask<T> Get<T>(string endpoint)
    {
        return SendRequest<T>(endpoint, "GET");
    }

    public static UniTask<T> Post<T>(string endpoint, object body, bool needSession = false)
    {
        return SendRequest<T>(endpoint, "POST", body, needSession);
    }

    public static UniTask<T> Patch<T>(string endpoint)
    {
        return SendRequest<T>(endpoint, "PATCH");
    }


    private static async UniTask<T> SendRequest<T>(string endpoint, string method, object body = null, bool needSession = false)
    {
        using UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, method);

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if(body != null)
        {
            string json = JsonConvert.SerializeObject(body);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(data);
        }

        if (needSession && PlayerPrefs.HasKey("sessionId"))
        {
            request.SetRequestHeader("Session-Id", PlayerPrefs.GetString("sessionId"));
        }

        await request.SendWebRequest().ToUniTask(cancellationToken: Application.exitCancellationToken);

        if (request.result != UnityWebRequest.Result.Success)
            throw new Exception(request.error);

        return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
    }
}
