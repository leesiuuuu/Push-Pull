using System.Collections.Generic;

/// <summary>
/// 전체 방 정보 조회 API입니다.
/// </summary>
[System.Serializable]
public class GetRoomsInfoResponse
{
    public List<GetRoomInfoResponse> rooms;
}