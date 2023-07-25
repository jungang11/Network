using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent; // RoomEntry�� �� �ڸ�
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomInfo> roomDictionary; // ����� ����� �����ϱ� ���� Dictionary

    public void Awake()
    {
        roomDictionary = new Dictionary<string, RoomInfo>();
    }

    private void OnDisable()
    {   
        // �κ� ���� �� ���� �κ��� ��Ȳ�� �����Ǵ� ���� ����
        roomDictionary.Clear();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Clear room list
        for(int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }

        // Update room info
        foreach (RoomInfo roomInfo in roomList)
        {
            // ���� �� ����� ����(RemovedFromList) + ���� ����� ���� ��� + ���� ������ ���(���û���)
            if (roomInfo.RemovedFromList || !roomInfo.IsVisible || !roomInfo.IsOpen)
            {
                // ���� �־��� ��쿡��
                if (roomDictionary.ContainsKey(roomInfo.Name))
                {
                    // Dictionary���� �ش� �̸��� ���� ������
                    roomDictionary.Remove(roomInfo.Name);
                }
                continue;
            }

            // ���� �ڷᱸ���� �־����� (�׳� �̸��� �ִ� ���̸� �ֽ����� ����)
            if (roomDictionary.ContainsKey(roomInfo.Name))
            {
                roomDictionary[roomInfo.Name] = roomInfo;
            }
            // ���� �ڷᱸ���� �������� (���� ���� ������ ���)
            else
            {
                roomDictionary.Add(roomInfo.Name, roomInfo);
            }
        }

        // Create room list
        foreach (RoomInfo roomInfo in roomDictionary.Values)
        {
            // roomContent �ڸ��� roomEntryPrefab ����
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            entry.SetRoomInfo(roomInfo); // �� ���� ����
        }
    }

    // Leave Button
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
