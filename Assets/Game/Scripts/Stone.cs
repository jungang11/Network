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

        // ���� �����ϴ°�x -> PhotonNetwork�� ����
        if(transform.position.magnitude > 200f)
            PhotonNetwork.Destroy(photonView);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) // PhotonNetwork.IsMasterClient
            return;

        if (other.gameObject.name == "Bullet(Clone)")
        {
            // �浹 �� ����
            // �Ѿ� ���������� ���� �߰�
            // �÷��̾ �� ���ӿ�����Ʈ�� ��ȣ�ۿ��Ҷ��� �� ���ӿ�����Ʈ�� ��ü�� �Ǿ� �۵��ϴ°��� ��Ȱ�ϰ� �����
            other.GetComponent<Bullet>().GetScore();
            PhotonNetwork.Destroy(photonView);
        }
    }
}
