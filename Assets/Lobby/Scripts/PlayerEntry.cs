using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] Button playerReadyButton;

    private Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "Ready" : "";    // Ready�� �Ǿ��־����� Ready, �ƴϸ� �������
        playerReadyButton.gameObject.SetActive(player.IsLocal);
    }

    public void Ready()
    {
        bool ready = player.GetReady();
        ready = !ready; // �ݴ� ��Ȳ ������ֱ�
        player.SetReady(ready);
    }
}
