using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Player player;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 3f);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void GetScore()
    {
        // ���� �߰�
    }

    public void ApplyLag(float lag)
    {
        // �Ÿ� = �ӷ� * �ð�
        transform.position += rb.velocity * lag;
    }
}
