using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField maxPlayerInputField;

    private void OnEnable()
    {
        createRoomPanel.SetActive(false);
    }

    public void CreateRoomMenu()
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomConfirm()
    {
        string roomName = roomNameInputField.text;
        // �� �̸��� ��������� �� ���� ���� ����
        if(roomName == "")
            roomName = $"Room {Random.Range(1000, 1000)}";

        // �ִ� �ο� �� �������� ������ 8
        int maxPlayer = maxPlayerInputField.text == "" ? 8 : int.Parse(maxPlayerInputField.text);
        // �ִ� �ο� �� ���� ������ 8
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };   // �ο� �� ����
        PhotonNetwork.CreateRoom(roomName, options); // ���� ���� ���� ���� (�н�����, ��������, �ο� �� ��)
    }

    public void CreateRoomCancel()
    {
        createRoomPanel.SetActive(false);
    }

    public void RandomMatching()
    {
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        // ���� ���� / �� ����
        PhotonNetwork.JoinRandomOrCreateRoom(roomName : name, roomOptions : options);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        PhotonNetwork.Disconnect(); // �г��� ���� �ʿ� X, ������ ���� ���� ��û
    }
}
