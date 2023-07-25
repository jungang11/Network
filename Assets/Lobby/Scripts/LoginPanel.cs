using Photon.Pun;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;

    private void OnEnable()
    {
        // Random Id 사용
        idInputField.text = string.Format("Player {0}", Random.Range(1000, 10000));
    }

    // Login 버튼 실행 함수
    public void Login()
    {
        // Id가 비어있을 때 진행하지 않음
        if(idInputField.text == "")
        {
            Debug.Log("Invalid Player Name");
            return;
        }

        // LocalPlayer -> 나 자신. / 나 자신의 NickName 설정
        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
        PhotonNetwork.ConnectUsingSettings(); // 접속 신청
    }
}
