using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float countdownTimer;
    [SerializeField] float spawnStoneTime;

    private void Start()
    {
        // Normal GameMode
        if (PhotonNetwork.InRoom)
        {
            // ���� �÷��̾�� GameScene Start���� Load�� true�� �ٲ���
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        // Debug GameMode
        else
        {
            infoText.text = "Debug Mode";
            PhotonNetwork.LocalPlayer.NickName = $"Debug Player{Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false }; // ����� ����
        PhotonNetwork.JoinOrCreateRoom("Debug Room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(DebugGameSetupDelay());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    // ���� ���� �°� : ���̱׷��̼�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ������ �ٲ���� �� ������ �� ����� SpawnRoutine ����
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnStoneRoutine());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        // �ٲ� property�� Load�� ��
        if (changedProps.ContainsKey("Load"))
        {
            // ��� �÷��̾ �ε� �Ϸ�
            if(PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                if(PhotonNetwork.IsMasterClient)
                    // ���� �ð����� ���� (��ǻ�͸��� �ð��� �ٸ� �� ����) + ������ ��츸 ����
                    PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);
            }
            // �Ϻ� �÷��̾� �ε� �Ϸ�
            else
            {
                // �ٸ� �÷��̾��� �ε� �Ϸ���� ���
                Debug.Log($"Wait Players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
                infoText.text = $"Wait Players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
            }
        }
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("LoadTime"))
        {
            StartCoroutine(GameStartTimerRoutine());
        }
    }

    private IEnumerator GameStartTimerRoutine()
    {
        int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();
        while(countdownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainTime = (int)(countdownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
            infoText.text = $"All Player Loaded, Start count down {remainTime + 1}";
            yield return new WaitForEndOfFrame();
        }
        infoText.text = "Game Start";
        GameStart();

        yield return new WaitForSeconds(1f);
        infoText.text = "";
    }

    private void GameStart()
    {
        // TODO : game start
        Debug.Log("Normal Gamemode");
    }

    IEnumerator DebugGameSetupDelay()
    {
        // ������ �����ð� 1�� ����
        yield return new WaitForSeconds(1f);
        DebugGameStart();
    }

    private void DebugGameStart()
    {
        float angularStart = (360.0f / 8f) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        Vector3 position = new Vector3(x, 0.0f, z);
        Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

        PhotonNetwork.Instantiate("Player", position, rotation);

        // ȣ��Ʈ�� �� ���� �ڷ�ƾ ����
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnStoneRoutine());
    }

    IEnumerator SpawnStoneRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnStoneTime);

            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 position = new Vector3(direction.x, 0, direction.y) * 200f;

            Vector3 force = -position.normalized * 30f + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Vector3 torque = Random.insideUnitSphere.normalized * Random.Range(1f, 3f);
            // �߰����� ������ �� ���� (������ �ƴϾ ��)
            object[] instantiateData = { force, torque };

            // InstantiateRoomObject : �濡�� �ٰ��� �� ���� ������Ʈ(������ �߿�x)
            PhotonNetwork.InstantiateRoomObject("LargeStone", position, Quaternion.identity, 0, instantiateData);
        }
    }

    private int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.GetLoad())
                loadCount++;
        }
        return loadCount;
    }
}
