using Newtonsoft.Json;

[System.Serializable]
public class LoginResponse
{
    [JsonProperty("sessionId")]
    public string SessionId;
}