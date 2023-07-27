using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

// PhotonNetwork ���� CallBack �� �ް� �ʹ�(���������� ������ �����ϰ� �ʹ�) -> MonoBehaviourPunCallbacks ���
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room, Register }

    [SerializeField] StatePanel statePanel;

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;
    [SerializeField] RegisterPanel registerPanel;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
            OnConnectedToMaster();
        else if (PhotonNetwork.InRoom)
            OnJoinedRoom();
        else if (PhotonNetwork.InLobby)
            OnJoinedLobby();
        else
            OnDisconnected(DisconnectCause.None);
    }

    public override void OnConnectedToMaster()
    {
        // ���ӵ��� �� �ൿ
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // ������ ������ �� �ൿ
        SetActivePanel(Panel.Login);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // �� ����� ���� �ݹ� �Լ�
        SetActivePanel(Panel.Menu);
        Debug.Log($"Create room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinedRoom()
    {
        // �� ����� ���� -> �濡 ���Ƿ� OnJoinedRoom ȣ��
        SetActivePanel(Panel.Room);

        // �濡 �� �� ready ���� / ���� ���� �� �����ص� ��
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        PhotonNetwork.AutomaticallySyncScene = true; // true : ������ ���� �ڵ����� ����
        roomPanel.UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false; // ���� ���� �� ����

        // ���� ������ ��
        SetActivePanel(Panel.Menu);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // �濡 �÷��̾ ������ ��
        roomPanel.UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // �濡�� �÷��̾ ������ ��
        roomPanel.UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ���� �÷��̾ �ٲ���� ��
        roomPanel.UpdatePlayerList();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        // �÷��̾� ������Ƽ�� ����Ǿ��� ��(����� Ready ��Ȳ)
        roomPanel.UpdatePlayerList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // �� ���� ����
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� �� ���� ����
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join random room failed with error({returnCode}) : {message}");
        statePanel.AddMessage($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinedLobby()
    {
        // Join Lobby
        SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()
    {
        // Leave Lobby
        SetActivePanel(Panel.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // �� ����� ���ŵ� �� ���� ȣ��Ǵ� �Լ�
        lobbyPanel.UpdateRoomList(roomList);
    }

    public void OnRegisterPanel()
    {
        SetActivePanel(Panel.Register);
    }

    public void OnRegisterCancel()
    {
        SetActivePanel(Panel.Login);
    }

    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
        registerPanel.gameObject?.SetActive(panel == Panel.Register);
    }
}
