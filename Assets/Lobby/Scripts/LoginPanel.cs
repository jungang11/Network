using Photon.Pun;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;

    private void OnEnable()
    {
        // Random Id ���
        idInputField.text = string.Format("Player {0}", Random.Range(1000, 10000));
    }

    // Login ��ư ���� �Լ�
    public void Login()
    {
        // Id�� ������� �� �������� ����
        if(idInputField.text == "")
        {
            Debug.Log("Invalid Player Name");
            return;
        }

        // LocalPlayer -> �� �ڽ�. / �� �ڽ��� NickName ����
        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
        PhotonNetwork.ConnectUsingSettings(); // ���� ��û
    }
}
