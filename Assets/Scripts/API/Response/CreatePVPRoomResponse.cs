/// <summary>
/// pvp 방 생성 API입니다.
/// 세션을 필요로 합니다.
/// </summary>
[System.Serializable]
public class CreatePVPRoomResponse
{
    public string roomCode;
    public string roomName;
    public bool isPrivate;
}