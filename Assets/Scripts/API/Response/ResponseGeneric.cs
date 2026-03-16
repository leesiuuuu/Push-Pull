using Newtonsoft.Json;

[System.Serializable]
public class Response<T>
{
    [JsonProperty("errorCode")]
    public int ErrorCode { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }
}