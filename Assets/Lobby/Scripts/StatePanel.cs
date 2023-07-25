using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
    [SerializeField] RectTransform content; // LogText �� �� �ڸ�
    [SerializeField] TMP_Text logPrefab;    // LogText Prefab

    private ClientState state;  // ���� ����

    void Update()
    {
        // PhotonNetwork Class -> ������ ���� ��û, ��û, Ȯ���� �� ���
        // �� ���°� �ٲ��� �ʾҴٸ� (���� ���¿� ���ٸ�) return
        if (state == PhotonNetwork.NetworkClientState)
            return;

        // ���°� �ٲ������ LogText ���
        state = PhotonNetwork.NetworkClientState;

        TMP_Text newLog = Instantiate(logPrefab, content);
        newLog.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), state);
        Debug.Log(string.Format("[Photon] {0}", state));
    }

    // Log Message
    public void AddMessage(string message)
    {
        TMP_Text newLog = Instantiate(logPrefab, content);
        newLog.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), message);
        Debug.Log(string.Format("[Photon] {0}", message));
    }
}
