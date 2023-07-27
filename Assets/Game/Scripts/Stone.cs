using UnityEngine;
using Photon.Pun;

public class Stone : MonoBehaviourPun
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(photonView.InstantiationData != null)
        {
            Vector3 force = (Vector3)photonView.InstantiationData[0];
            Vector3 torque = (Vector3)photonView.InstantiationData[1];

            rb.AddForce(force, ForceMode.Impulse);
            rb.AddTorque(torque, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) // PhotonNetwork.IsMasterClient
            return;

        // 나만 삭제하는것x -> PhotonNetwork로 삭제
        if(transform.position.magnitude > 200f)
            PhotonNetwork.Destroy(photonView);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) // PhotonNetwork.IsMasterClient
            return;

        if (other.gameObject.name == "Bullet(Clone)")
        {
            // 충돌 시 반응
            // 총알 소유주한테 점수 추가
            // 플레이어가 룸 게임오브젝트와 상호작용할때는 룸 게임오브젝트가 주체가 되어 작동하는것이 원활하게 진행됨
            other.GetComponent<Bullet>().GetScore();
            PhotonNetwork.Destroy(photonView);
        }
    }
}
