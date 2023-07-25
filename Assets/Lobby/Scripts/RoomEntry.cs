using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    private RoomInfo info;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        info = roomInfo;
        roomName.text = roomInfo.Name;
        currentPlayer.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
        joinRoomButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers; // ���� �� ���� �ʾ��� ��츸 Ŭ�� ����
    }

    public void JoinRoom()
    {
        // �κ� �ִٰ� �濡 �� ��� �κ� �������� (���� Ư¡). ������ ���� ��� Lobby ���� ������ ��� ����
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(info.Name);
    }
}
