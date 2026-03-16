/// <summary>
/// 방 생성 API입니다.
/// 세션을 필요로 합니다.
/// </summary>
public class CreateRoomRequest
{
    public string lobbyId;
    public string roomName;
    public bool isPrivate;
    public string password;
}